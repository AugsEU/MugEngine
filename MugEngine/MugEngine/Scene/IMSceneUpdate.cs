using MugEngine.Core.Types;

namespace MugEngine.Scene
{
	public interface IMSceneUpdate
	{
		public void Update(MScene scene, MUpdateInfo info);
	}
}
