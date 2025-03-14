using MugEngine.Core;

namespace MugEngine.Scene;

public interface IMSceneUpdate
{
	public void Update(MScene scene, MUpdateInfo info);
}

