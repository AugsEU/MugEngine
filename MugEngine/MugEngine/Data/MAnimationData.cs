using System.Globalization;
using System.Xml;
using MugEngine.Core;
using static MugEngine.Graphics.MAnimation;

namespace MugEngine.Data;

    /// <summary>
    /// Utility class for loading animations from data
    /// </summary>
    internal class MAnimationData
{
	private struct FrameData
	{
		public string mTexturePath;
		public Rectangle mRect;
		public float mDuration;

		public FrameData(string texturePath)
		{
			mTexturePath = texturePath;
			mRect = Rectangle.Empty;
			mDuration = 100.0f;
		}

		public FrameData()
		{

		}
	}

	PlayType mPlayType;
	int mNumRepeats;
	FrameData[] mFrameData;

	public MAnimationData()
	{
		mPlayType = PlayType.Forward;
		mFrameData = null;
		mNumRepeats = 1;
	}

	public MAnimationData(string filePath)
	{
		LoadFromFile(filePath);
	}

	private void LoadFromFile(string filePath)
	{
		string extention = Path.GetExtension(filePath);
		switch (extention)
		{
			case "":
			{
				// Load as single texture
				mFrameData = new FrameData[] { new FrameData(filePath) };
				mPlayType = PlayType.Forward;
				break;
			}
			case ".max": // Mono Animation XML
			{
				LoadFromXML("@Data/" + filePath);
				break;
			}
			default:
				throw new NotImplementedException();
		}
	}

	private void LoadFromXML(string XMLPath)
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(XMLPath);
		XmlNode rootNode = xmlDoc.LastChild;

		mPlayType = MugXML.GetEnum(rootNode["type"], PlayType.Forward);
		mNumRepeats = MugXML.GetInt(rootNode["repeats"]);

		// Parse frames.
		XmlNodeList frameNodes = rootNode.SelectNodes("frame");
		mFrameData = new FrameData[frameNodes.Count];

		foreach (XmlNode frameNode in frameNodes)
		{
			int idx = int.Parse(MugXML.GetStringAttrib(frameNode, "id"), CultureInfo.InvariantCulture);

			FrameData frameData = new FrameData();
			frameData.mTexturePath = MugXML.GetString(frameNode["texture"]);
			frameData.mRect = MugXML.GetRectangle(frameNode);
			frameData.mDuration = MugXML.GetFloat(frameNode["duration"]);

			mFrameData[idx] = frameData;
		}
	}

	public MAnimation GenerateAnimation()
	{
		Frame[] animationFrames = new Frame[mFrameData.Length];

		// Special case for 1 frame, means we loaded from a texture not a .max file.
		if (mFrameData.Length == 1 && mFrameData[0].mRect == Rectangle.Empty)
		{
			Texture2D texture = MData.I.Load<Texture2D>(mFrameData[0].mTexturePath);
			MTexturePart texPart = new MTexturePart(texture);
			animationFrames[0] = new Frame(texPart, mFrameData[0].mDuration);
		}
		else
		{
			for (int i = 0; i < mFrameData.Length; ++i)
			{
				Texture2D texture = MData.I.Load<Texture2D>(mFrameData[i].mTexturePath);
				MTexturePart texPart = new MTexturePart(texture, mFrameData[i].mRect);
				animationFrames[i] = new Frame(texPart, mFrameData[i].mDuration);
			}
		}

		return new MAnimation(mPlayType, mNumRepeats, animationFrames);
	}
}

