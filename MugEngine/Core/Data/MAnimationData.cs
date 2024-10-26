using MugEngine.Graphics;
using System.Globalization;
using System.Xml;

namespace MugEngine.Core
{
	/// <summary>
	/// Utility class for loading animations from data
	/// </summary>
	class AnimationData
	{
		MAnimation.PlayType mPlayType;
		(string, float)[] mTexturePaths;

		public AnimationData()
		{
			mPlayType = MAnimation.PlayType.Repeat;
			mTexturePaths = null;
		}

		public AnimationData(string filePath)
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
					mTexturePaths = new (string, float)[] { (filePath, 1.0f) };
					mPlayType = MAnimation.PlayType.OneShot;
					break;
				}
				case ".max": // Mono Animation XML
				{
					LoadFromXML("Content/" + filePath);
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

			string type = rootNode.Attributes["type"].Value.ToLower();

			mPlayType = type == "repeat" ? MAnimation.PlayType.Repeat : MAnimation.PlayType.OneShot;

			XmlNodeList textureNodes = rootNode.ChildNodes;

			mTexturePaths = new (string, float)[textureNodes.Count];

			int idx = 0;
			foreach (XmlNode textureNode in textureNodes)
			{
				XmlAttribute timeAttrib = textureNode.Attributes["time"];
				float time = timeAttrib is not null ? float.Parse(timeAttrib.Value, CultureInfo.InvariantCulture.NumberFormat) : 1.0f;
				mTexturePaths[idx++] = (textureNode.InnerText, time);
			}
		}

		public MAnimation GenerateMAnimation()
		{
			// To do: Load animation.
			return null;// new MAnimation(mPlayType, mTextures);
		}
	}
}
