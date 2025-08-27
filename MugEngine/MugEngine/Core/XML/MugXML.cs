using System.Globalization;
using System.Xml;

namespace MugEngine.Core;

/// <summary>
/// Utility to parse simple types from XML
/// </summary>
public static class MugXML
{
	/// <summary>
	/// Parse float from xml node. Default = Zero
	/// </summary>
	static public float GetFloat(XmlNode node, float defaultVal = 0.0f)
	{
		if (node is null)
		{
			return defaultVal;
		}

		return float.Parse(node.InnerText, CultureInfo.InvariantCulture.NumberFormat);
	}



	/// <summary>
	/// Parse int from xml node. Default = Zero
	/// </summary>
	static public int GetInt(XmlNode node, int defaultVal = 0)
	{
		if (node is null)
		{
			return defaultVal;
		}

		return int.Parse(node.InnerText, CultureInfo.InvariantCulture.NumberFormat);
	}


	/// <summary>
	/// Parse UInt64 from xml node. Default = Zero
	/// </summary>
	static public ulong GetUInt64(XmlNode node, ulong defaultVal = 0u)
	{
		if (node is null)
		{
			return defaultVal;
		}

		return ulong.Parse(node.InnerText, CultureInfo.InvariantCulture.NumberFormat);
	}



	/// <summary>
	/// Get inner text of node
	/// </summary>
	static public string GetString(XmlNode node, string defaultVal = "")
	{
		if (node is null)
		{
			return defaultVal;
		}

		return node.InnerText;
	}



	/// <summary>
	/// Parse a string attribute
	/// </summary>
	static public string GetStringAttrib(XmlNode node, string attribID, string defaultVal = "")
	{
		if (node is null)
		{
			return defaultVal;
		}

		attribID = attribID.ToLower();
		XmlAttribute attribObj = node.Attributes[attribID];

		if (attribObj is null)
		{
			return defaultVal;
		}

		return attribObj.Value;
	}



	/// <summary>
	/// Parse vector from xml node. Default = Zero
	/// </summary>
	static public Vector2 GetVector2(XmlNode node)
	{
		XmlNode xNode = node.SelectSingleNode("x");
		XmlNode yNode = node.SelectSingleNode("y");
		return new Vector2(GetFloat(xNode), GetFloat(yNode));
	}



	/// <summary>
	/// Parse point from xml node. Default = zero
	/// </summary>
	static public Point GetPoint(XmlNode node)
	{
		XmlNode xNode = node.SelectSingleNode("x");
		XmlNode yNode = node.SelectSingleNode("y");
		return new Point(GetInt(xNode), GetInt(yNode));
	}




	/// <summary>
	/// Parse point from xml node. Default = zero
	/// </summary>
	static public Rectangle GetRectangle(XmlNode node)
	{
		XmlNode xNode = node.SelectSingleNode("x");
		XmlNode yNode = node.SelectSingleNode("y");
		XmlNode wNode = node.SelectSingleNode("width");
		XmlNode hNode = node.SelectSingleNode("height");

		return new Rectangle(GetInt(xNode), GetInt(yNode), GetInt(wNode), GetInt(hNode));
	}



	/// <summary>
	/// Parse point from xml node. Default = zero
	/// </summary>
	static public MRect2f GetRect2f(XmlNode node)
	{
		XmlNode wNode = node.SelectSingleNode("width");
		XmlNode hNode = node.SelectSingleNode("height");

		return new MRect2f(GetVector2(node), GetFloat(wNode), GetFloat(hNode));
	}



	/// <summary>
	/// Parse texture from xml node. Default = Dummy
	/// </summary>
	static public Texture2D GetTexture(XmlNode node)
	{
		if (node is null)
		{
			return null;
		}

		return MData.I.Load<Texture2D>(node.InnerText);
	}


	/// <summary>
	/// Parse texture from xml node. Default = Dummy
	/// </summary>
	static public Texture2D GetTexture(XmlNode node, Texture2D defaultValue)
	{
		if (node is null)
		{
			return defaultValue;
		}

		return MData.I.Load<Texture2D>(node.InnerText);
	}



	/// <summary>
	/// Parse hex colour from xml node. Default = Black
	/// </summary>
	static public Color GetColor(XmlNode node)
	{
		if (node is null)
		{
			return Color.White;
		}

		return MugColor.HEXToColor(node.InnerXml);
	}



	/// <summary>
	/// Parse hex colour from xml node with specific default
	/// </summary>
	static public Color GetColor(XmlNode node, Color defaultVal)
	{
		if (node is null)
		{
			return defaultVal;
		}

		return MugColor.HEXToColor(node.InnerXml);
	}



	/// <summary>
	/// Parse node as enum
	/// </summary>
	static public T GetEnum<T>(XmlNode node, T defaultValue = default)
	{
		if (node is null)
		{
			return defaultValue;
		}

		string enumStr = GetString(node);
		return MugEnum.GetEnumFromString<T>(enumStr);
	}
}

