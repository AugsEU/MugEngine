using System.Xml;

namespace MugEngine.Core;

public interface IMFromXMLNode<T> where T : IMFromXMLNode<T>
{
	static abstract T FromNode(XmlNode node);
}

