using System.Threading.Tasks;

namespace MugEngine.Scene;

/// <summary>
/// A game object is something that exists within the game.
/// </summary>
public abstract class MGameObject : IMUpdate, IMDraw, IMBounds
{
	#region rMembers

	protected MScene mScene;
	protected MGameObjectManager mParent;
	protected Vector2 mPosition;
	protected Point mSize;

	MLayerMask mLayers = new MLayerMask(1);
	bool mEnabled = true;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create game object at position.
	/// </summary>
	public MGameObject(Vector2 position)
	{
		mPosition = position;
		mSize = new Point(1, 1);
	}

	#endregion rInit





	#region rScene

	/// <summary>
	/// Set the scene we are in.
	/// </summary>
	public void SetScene(MScene scene)
	{
		mScene = scene;
		mParent = mScene.GO;
	}



	/// <summary>
	/// Get the scene we are in.
	/// </summary>
	protected MScene Scene()
	{
		return mScene;
	}



	/// <summary>
	/// Get the game object manager that manages this game object.
	/// </summary>
	protected MGameObjectManager GO()
	{
		return mParent;
	}


	/// <summary>
	/// Called once the object is fully constructed
	/// </summary>
	public virtual void PostInitSetup()
	{

	}

	#endregion rScene






	#region rUpdate

	/// <summary>
	/// Update the game object
	/// </summary>
	public abstract void Update(MUpdateInfo info);


	/// <summary>
	/// Kill this entity.
	/// </summary>
	public virtual void Kill()
	{
		// By default we just delete instantly.
		GO().QueueDelete(this);
	}

	#endregion rUpdate





	#region rDraw

	/// <summary>
	/// Update the draw object
	/// </summary>
	public abstract void Draw(MDrawInfo info);

	#endregion rDraw





	#region rCollision

	/// <summary>
	/// Get bounding box for this entity.
	/// </summary>
	public Point BoundsSize()
	{
		return mSize;
	}



	/// <summary>
	/// Get collider in world coordinates
	/// </summary>
	public Rectangle BoundsRect()
	{
		return new Rectangle(mPosition.ToPoint(), BoundsSize());
	}



	/// <summary>
	/// Centre of mass
	/// </summary>
	public Vector2 GetCentreOfMass()
	{
		return BoundsRect().Center.ToVector2();
	}

	#endregion rCollision





	#region rLayer

	/// <summary>
	/// Get all layers this entity is on.
	/// </summary>
	public MLayerMask GetLayerMask()
	{
		return mLayers;
	}
	

	
	/// <summary>
	/// Are these two objects on the same layer?
	/// </summary>
	public bool InteractsWith(MGameObject other)
	{
		return mLayers.InteractsWith(other.mLayers);
	}



	/// <summary>
	/// Is this object part of this mask?
	/// </summary>
	public bool InteractsWith(MLayerMask mask)
	{
		return mLayers.InteractsWith(mask);
	}



	/// <summary>
	/// Set interaction layers specifically.
	/// Clears all other layers.
	/// </summary>
	public void SetLayer<T>(T layer) where T : Enum
	{
		mLayers.SetLayer(layer);
	}



	/// <summary>
	/// Add to an interaction layer.
	/// Does not clear other layers
	/// </summary>
	public void AddLayer<T>(T layer) where T : Enum
	{
		mLayers.AddLayer(layer);
	}



	/// <summary>
	/// Remove from an interaction layer.
	/// Does not clear other layers.
	/// </summary>
	public void RemoveLayer<T>(T layer) where T : Enum
	{
		mLayers.RemoveLayer(layer);
	}

	#endregion rLayer





	#region rEnable

	/// <summary>
	/// Is this enabled?
	/// </summary>
	public bool IsEnabled()
	{
		return mEnabled;
	}



	/// <summary>
	/// Is this enabled?
	/// </summary>
	public void SetEnabled(bool enabled)
	{
		mEnabled = enabled;
	}

	#endregion rEnable





	#region rUtil

	/// <summary>
	/// Get a position
	/// </summary>
	public Vector2 GetPos()
	{
		return mPosition;
	}



	/// <summary>
	/// Set the position of this game object
	/// </summary>
	public void SetPos(Vector2 pos)
	{
		mPosition = pos;
	}



	/// <summary>
	/// Get the size
	/// </summary>
	public Point GetSize()
	{
		return mSize;
	}


	/// <summary>
	/// Set the size.
	/// </summary>
	public void SetSize(Point size)
	{
		mSize = size;
	}

	#endregion rUtil
}

