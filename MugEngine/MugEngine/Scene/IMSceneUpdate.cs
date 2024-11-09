using MugEngine.Types;

namespace MugEngine.Scene
{
	public interface IMSceneUpdate
	{
		public void Update(MScene scene, MUpdateInfo info);
	}
}
