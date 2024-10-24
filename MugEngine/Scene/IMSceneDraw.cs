using MugEngine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugEngine.Scene
{
	public interface IMSceneDraw
	{
		public void Draw(MScene scene, MDrawInfo info);
	}
}
