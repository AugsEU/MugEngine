using System.Xml;

namespace MugEngine.Core;

public static class MXMLClassLoader<T> where T : class, IMFromXMLNode<T>
{
	public static T FromFile(string path)
	{
		path = Path.Join("@Data", path);
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(path);
		XmlNode rootNode = xmlDoc.LastChild;

		return T.FromNode(rootNode);
	}
}

