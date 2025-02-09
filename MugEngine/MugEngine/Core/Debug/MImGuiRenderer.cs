﻿using ImGuiNET;
using System.Runtime.InteropServices;

namespace MugEngine.Core
{
	/// <summary>
	/// ImGui renderer for use with XNA-likes (FNA & MonoGame)
	/// </summary>
	public class MImGuiRenderer
	{
		#region rConstants

		const float WHEEL_DELTA = 120;
		readonly static Keys[] ALL_KEYS = Enum.GetValues<Keys>();

		#endregion rConstants




		#region rMembers

		Game mGame;

		// Graphics
		GraphicsDevice mGraphicsDevice;

		BasicEffect mBasicEffect;
		RasterizerState mRasterizerState;

		byte[] mVertexData;
		VertexBuffer mVertexBuffer;
		int mVertexBufferSize;

		byte[] mIndexData;
		IndexBuffer mIndexBuffer;
		int mIndexBufferSize;

		VertexDeclaration mDeclaration;
		readonly int mVertexSize;

		// Textures
		Dictionary<IntPtr, Texture2D> mLoadedTextures;

		int mTextureId;
		IntPtr? mFontTextureId;

		// Input
		int mScrollWheelValue;
		int mHorizontalScrollWheelValue;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create the ImGuiRenderer
		/// </summary>
		public MImGuiRenderer(Game game)
		{
			nint context = ImGui.CreateContext();
			ImGui.SetCurrentContext(context);

			mGame = game ?? throw new ArgumentNullException(nameof(game));
			mGraphicsDevice = game.GraphicsDevice;

			mLoadedTextures = new Dictionary<IntPtr, Texture2D>();

			mRasterizerState = new RasterizerState()
			{
				CullMode = CullMode.None,
				DepthBias = 0,
				FillMode = FillMode.Solid,
				MultiSampleAntiAlias = false,
				ScissorTestEnable = true,
				SlopeScaleDepthBias = 0
			};

			SetupInput();

			unsafe { mVertexSize = sizeof(ImDrawVert); }

			mDeclaration = new VertexDeclaration(
				mVertexSize,

				// Position
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),

				// UV
				new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

				// Color
				new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
			);

			RebuildFontAtlas();
		}



		/// <summary>
		/// Creates a texture and loads the font data from ImGui. Should be called when the <see cref="GraphicsDevice" /> is initialized but before any rendering is done
		/// </summary>
		unsafe void RebuildFontAtlas()
		{
			// Get font texture from ImGui
			ImGuiIOPtr io = ImGui.GetIO();
			io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out int width, out int height, out int bytesPerPixel);

			// Copy the data to a managed array
			var pixels = new byte[width * height * bytesPerPixel];
			unsafe { Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length); }

			// Create and register the texture as an XNA texture
			var tex2d = new Texture2D(mGraphicsDevice, width, height, false, SurfaceFormat.Color);
			tex2d.SetData(pixels);

			// Should a texture already have been build previously, unbind it first so it can be deallocated
			if (mFontTextureId.HasValue) UnbindTexture(mFontTextureId.Value);

			// Bind the new texture to an ImGui-friendly id
			mFontTextureId = BindTexture(tex2d);

			// Let ImGui know where to find the texture
			io.Fonts.SetTexID(mFontTextureId.Value);
			io.Fonts.ClearTexData(); // Clears CPU side texture data
		}



		/// <summary>
		/// Setup key input event handler.
		/// </summary>
		protected virtual void SetupInput()
		{
			var io = ImGui.GetIO();

			mGame.Window.TextInput += (s, a) =>
			{
				if (a.Character == '\t') return;
				io.AddInputCharacter(a.Character);
			};
		}

		#endregion rInit





		#region rDraw

		/// <summary>
		/// Sets up ImGui for a new frame, should be called at frame start
		/// </summary>
		public virtual void BeforeLayout(GameTime gameTime)
		{
			ImGui.GetIO().DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			UpdateInput();

			ImGui.NewFrame();
		}



		/// <summary>
		/// Asks ImGui for the generated geometry data and sends it to the graphics pipeline, should be called after the UI is drawn using ImGui.** calls
		/// </summary>
		public virtual void AfterLayout()
		{
			ImGui.Render();

			unsafe { RenderDrawData(ImGui.GetDrawData()); }
		}



		/// <summary>
		/// Gets the geometry as set up by ImGui and sends it to the graphics device
		/// </summary>
		void RenderDrawData(ImDrawDataPtr drawData)
		{
			// Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, vertex/texcoord/color pointers
			var lastViewport = mGraphicsDevice.Viewport;
			var lastScissorBox = mGraphicsDevice.ScissorRectangle;
			var lastRasterizer = mGraphicsDevice.RasterizerState;
			var lastDepthStencil = mGraphicsDevice.DepthStencilState;
			var lastBlendFactor = mGraphicsDevice.BlendFactor;
			var lastBlendState = mGraphicsDevice.BlendState;

			mGraphicsDevice.BlendFactor = Color.White;
			mGraphicsDevice.BlendState = BlendState.NonPremultiplied;
			mGraphicsDevice.RasterizerState = mRasterizerState;
			mGraphicsDevice.DepthStencilState = DepthStencilState.None;

			// Handle cases of screen coordinates != from framebuffer coordinates (e.g. retina displays)
			drawData.ScaleClipRects(ImGui.GetIO().DisplayFramebufferScale);

			// Setup projection
			mGraphicsDevice.Viewport = new Viewport(0, 0, mGraphicsDevice.PresentationParameters.BackBufferWidth, mGraphicsDevice.PresentationParameters.BackBufferHeight);

			UpdateBuffers(drawData);

			RenderCommandLists(drawData);

			// Restore modified state
			mGraphicsDevice.Viewport = lastViewport;
			mGraphicsDevice.ScissorRectangle = lastScissorBox;
			mGraphicsDevice.RasterizerState = lastRasterizer;
			mGraphicsDevice.DepthStencilState = lastDepthStencil;
			mGraphicsDevice.BlendState = lastBlendState;
			mGraphicsDevice.BlendFactor = lastBlendFactor;
		}

		unsafe void UpdateBuffers(ImDrawDataPtr drawData)
		{
			if (drawData.TotalVtxCount == 0)
			{
				return;
			}

			// Expand buffers if we need more room
			if (drawData.TotalVtxCount > mVertexBufferSize)
			{
				mVertexBuffer?.Dispose();

				mVertexBufferSize = (int)(drawData.TotalVtxCount * 1.5f);
				mVertexBuffer = new VertexBuffer(mGraphicsDevice, mDeclaration, mVertexBufferSize, BufferUsage.None);
				mVertexData = new byte[mVertexBufferSize * mVertexSize];
			}

			if (drawData.TotalIdxCount > mIndexBufferSize)
			{
				mIndexBuffer?.Dispose();

				mIndexBufferSize = (int)(drawData.TotalIdxCount * 1.5f);
				mIndexBuffer = new IndexBuffer(mGraphicsDevice, IndexElementSize.SixteenBits, mIndexBufferSize, BufferUsage.None);
				mIndexData = new byte[mIndexBufferSize * sizeof(ushort)];
			}

			// Copy ImGui's vertices and indices to a set of managed byte arrays
			int vtxOffset = 0;
			int idxOffset = 0;

			for (int n = 0; n < drawData.CmdListsCount; n++)
			{
				ImDrawListPtr cmdList = drawData.CmdLists[n];

				fixed (void* vtxDstPtr = &mVertexData[vtxOffset * mVertexSize])
				fixed (void* idxDstPtr = &mIndexData[idxOffset * sizeof(ushort)])
				{
					Buffer.MemoryCopy((void*)cmdList.VtxBuffer.Data, vtxDstPtr, mVertexData.Length, cmdList.VtxBuffer.Size * mVertexSize);
					Buffer.MemoryCopy((void*)cmdList.IdxBuffer.Data, idxDstPtr, mIndexData.Length, cmdList.IdxBuffer.Size * sizeof(ushort));
				}

				vtxOffset += cmdList.VtxBuffer.Size;
				idxOffset += cmdList.IdxBuffer.Size;
			}

			// Copy the managed byte arrays to the gpu vertex- and index buffers
			mVertexBuffer.SetData(mVertexData, 0, drawData.TotalVtxCount * mVertexSize);
			mIndexBuffer.SetData(mIndexData, 0, drawData.TotalIdxCount * sizeof(ushort));
		}




		unsafe void RenderCommandLists(ImDrawDataPtr drawData)
		{
			mGraphicsDevice.SetVertexBuffer(mVertexBuffer);
			mGraphicsDevice.Indices = mIndexBuffer;

			int vtxOffset = 0;
			int idxOffset = 0;

			for (int n = 0; n < drawData.CmdListsCount; n++)
			{
				ImDrawListPtr cmdList = drawData.CmdLists[n];

				for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
				{
					ImDrawCmdPtr drawCmd = cmdList.CmdBuffer[cmdi];

					if (drawCmd.ElemCount == 0)
					{
						continue;
					}

					if (!mLoadedTextures.ContainsKey(drawCmd.TextureId))
					{
						throw new InvalidOperationException($"Could not find a texture with id '{drawCmd.TextureId}', please check your bindings");
					}

					mGraphicsDevice.ScissorRectangle = new Rectangle(
						(int)drawCmd.ClipRect.X,
						(int)drawCmd.ClipRect.Y,
						(int)(drawCmd.ClipRect.Z - drawCmd.ClipRect.X),
						(int)(drawCmd.ClipRect.W - drawCmd.ClipRect.Y)
					);

					var effect = UpdateEffect(mLoadedTextures[drawCmd.TextureId]);

					foreach (var pass in effect.CurrentTechnique.Passes)
					{
						pass.Apply();

						mGraphicsDevice.DrawIndexedPrimitives(
							primitiveType: PrimitiveType.TriangleList,
							baseVertex: (int)drawCmd.VtxOffset + vtxOffset,
							startIndex: (int)drawCmd.IdxOffset + idxOffset,
							primitiveCount: (int)drawCmd.ElemCount / 3
						);
					}
				}

				vtxOffset += cmdList.VtxBuffer.Size;
				idxOffset += cmdList.IdxBuffer.Size;
			}
		}

		#endregion rDraw




		#region rUpdate

		/// <summary>
		/// Updates the <see cref="Effect" /> to the current matrices and texture
		/// </summary>
		protected virtual Effect UpdateEffect(Texture2D texture)
		{
			mBasicEffect = mBasicEffect ?? new BasicEffect(mGraphicsDevice);

			var io = ImGui.GetIO();

			mBasicEffect.World = Matrix.Identity;
			mBasicEffect.View = Matrix.Identity;
			mBasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0f, io.DisplaySize.X, io.DisplaySize.Y, 0f, -1f, 1f);
			mBasicEffect.TextureEnabled = true;
			mBasicEffect.Texture = texture;
			mBasicEffect.VertexColorEnabled = true;

			return mBasicEffect;
		}



		/// <summary>
		/// Sends XNA input state to ImGui
		/// </summary>
		protected virtual void UpdateInput()
		{
			if (!mGame.IsActive) return;

			var io = ImGui.GetIO();

			var mouse = Mouse.GetState();
			var keyboard = Keyboard.GetState();
			io.AddMousePosEvent(mouse.X, mouse.Y);
			io.AddMouseButtonEvent(0, mouse.LeftButton == ButtonState.Pressed);
			io.AddMouseButtonEvent(1, mouse.RightButton == ButtonState.Pressed);
			io.AddMouseButtonEvent(2, mouse.MiddleButton == ButtonState.Pressed);
			io.AddMouseButtonEvent(3, mouse.XButton1 == ButtonState.Pressed);
			io.AddMouseButtonEvent(4, mouse.XButton2 == ButtonState.Pressed);

			io.AddMouseWheelEvent(
				(mouse.HorizontalScrollWheelValue - mHorizontalScrollWheelValue) / WHEEL_DELTA,
				(mouse.ScrollWheelValue - mScrollWheelValue) / WHEEL_DELTA);
			mScrollWheelValue = mouse.ScrollWheelValue;
			mHorizontalScrollWheelValue = mouse.HorizontalScrollWheelValue;

			foreach (var key in ALL_KEYS)
			{
				if (TryMapKeys(key, out ImGuiKey imguikey))
				{
					io.AddKeyEvent(imguikey, keyboard.IsKeyDown(key));
				}
			}

			io.DisplaySize = new System.Numerics.Vector2(mGraphicsDevice.PresentationParameters.BackBufferWidth, mGraphicsDevice.PresentationParameters.BackBufferHeight);
			io.DisplayFramebufferScale = new System.Numerics.Vector2(1f, 1f);
		}

		#endregion rUpdate





		#region rUtils

		/// <summary>
		/// Creates a pointer to a texture, which can be passed through ImGui calls such as <see cref="ImGui.Image" />. That pointer is then used by ImGui to let us know what texture to draw
		/// </summary>
		public virtual IntPtr BindTexture(Texture2D texture)
		{
			var id = new IntPtr(mTextureId++);

			mLoadedTextures.Add(id, texture);

			return id;
		}



		/// <summary>
		/// Removes a previously created texture pointer, releasing its reference and allowing it to be deallocated
		/// </summary>
		public virtual void UnbindTexture(IntPtr textureId)
		{
			mLoadedTextures.Remove(textureId);
		}



		/// <summary>
		/// Map MG key enum to ImGui key, return false if not found.
		/// </summary>
		bool TryMapKeys(Keys key, out ImGuiKey imguikey)
		{
			//Special case not handed in the switch...
			//If the actual key we put in is "None", return none and true. 
			//otherwise, return none and false.
			if (key == Keys.None)
			{
				imguikey = ImGuiKey.None;
				return true;
			}

			imguikey = key switch
			{
				Keys.Back => ImGuiKey.Backspace,
				Keys.Tab => ImGuiKey.Tab,
				Keys.Enter => ImGuiKey.Enter,
				Keys.CapsLock => ImGuiKey.CapsLock,
				Keys.Escape => ImGuiKey.Escape,
				Keys.Space => ImGuiKey.Space,
				Keys.PageUp => ImGuiKey.PageUp,
				Keys.PageDown => ImGuiKey.PageDown,
				Keys.End => ImGuiKey.End,
				Keys.Home => ImGuiKey.Home,
				Keys.Left => ImGuiKey.LeftArrow,
				Keys.Right => ImGuiKey.RightArrow,
				Keys.Up => ImGuiKey.UpArrow,
				Keys.Down => ImGuiKey.DownArrow,
				Keys.PrintScreen => ImGuiKey.PrintScreen,
				Keys.Insert => ImGuiKey.Insert,
				Keys.Delete => ImGuiKey.Delete,
				>= Keys.D0 and <= Keys.D9 => ImGuiKey._0 + (key - Keys.D0),
				>= Keys.A and <= Keys.Z => ImGuiKey.A + (key - Keys.A),
				>= Keys.NumPad0 and <= Keys.NumPad9 => ImGuiKey.Keypad0 + (key - Keys.NumPad0),
				Keys.Multiply => ImGuiKey.KeypadMultiply,
				Keys.Add => ImGuiKey.KeypadAdd,
				Keys.Subtract => ImGuiKey.KeypadSubtract,
				Keys.Decimal => ImGuiKey.KeypadDecimal,
				Keys.Divide => ImGuiKey.KeypadDivide,
				>= Keys.F1 and <= Keys.F24 => ImGuiKey.F1 + (key - Keys.F1),
				Keys.NumLock => ImGuiKey.NumLock,
				Keys.Scroll => ImGuiKey.ScrollLock,
				Keys.LeftShift => ImGuiKey.ModShift,
				Keys.LeftControl => ImGuiKey.ModCtrl,
				Keys.LeftAlt => ImGuiKey.ModAlt,
				Keys.OemSemicolon => ImGuiKey.Semicolon,
				Keys.OemPlus => ImGuiKey.Equal,
				Keys.OemComma => ImGuiKey.Comma,
				Keys.OemMinus => ImGuiKey.Minus,
				Keys.OemPeriod => ImGuiKey.Period,
				Keys.OemQuestion => ImGuiKey.Slash,
				Keys.OemTilde => ImGuiKey.GraveAccent,
				Keys.OemOpenBrackets => ImGuiKey.LeftBracket,
				Keys.OemCloseBrackets => ImGuiKey.RightBracket,
				Keys.OemPipe => ImGuiKey.Backslash,
				Keys.OemQuotes => ImGuiKey.Apostrophe,
				Keys.BrowserBack => ImGuiKey.AppBack,
				Keys.BrowserForward => ImGuiKey.AppForward,
				_ => ImGuiKey.None,
			};

			return imguikey != ImGuiKey.None;
		}

		#endregion rUtils
	}
}
