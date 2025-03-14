using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace MugEngine.Tuner;

public static class MTuner<T> where T : struct
{
	public static T I = new();

	/// <summary>
	/// Load values from XML.
	/// </summary>
	/// <param name="xmlPath"></param>
	public static void LoadValues(string xmlPath)
	{
		var xDoc = XDocument.Load(xmlPath);
		I = (T)DeserializeStruct(xDoc.Root, typeof(T));
	}



	/// <summary>
	/// Recursively deserialise a struct
	/// </summary>
	private static object DeserializeStruct(XElement element, Type structType)
	{
		object resultBox = Activator.CreateInstance(structType);

		foreach (FieldInfo field in structType.GetFields(BindingFlags.Public | BindingFlags.Instance))
		{
			XElement fieldElement = element.Element(field.Name);

			if(fieldElement is null)
			{
				MugDebug.Warning("Couldn't find field of name {0}", field.Name);
				continue;
			}

			if (field.FieldType.IsPrimitive)
			{
				// This is a primitive type
				object value = Parse(fieldElement.Value, field.FieldType);
				field.SetValue(resultBox, value);
			}
			else
			{
				// This is a nested struct - recursively deserialize it
				object nestedValue = DeserializeStruct(fieldElement, field.FieldType);
				field.SetValue(resultBox, nestedValue);
			}
		}

		return resultBox;
	}



	/// <summary>
	/// Parse the info.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="targetType"></param>
	/// <returns></returns>
	private static object Parse(string value, Type targetType)
	{
		if (targetType == typeof(float))
		{
			return float.Parse(value, CultureInfo.InvariantCulture);
		}
		else if (targetType == typeof(int))
		{
			return int.Parse(value, CultureInfo.InvariantCulture);
		}
		// Add other specific type conversions as needed
		return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
	}



	/// <summary>
	/// Save values into an xml file.
	/// </summary>
	public static void SaveValues(string xmlPath)
	{
		XDocument xDoc = new();
		XElement root = SerializeStruct(I, "Tuning");

		xDoc.Add(root);
		xDoc.Save(xmlPath);
	}



	/// <summary>
	/// Serialise a struct and all members recursively.
	/// </summary>
	private static XElement SerializeStruct(object structData, string nodeName)
	{
		var root = new XElement(nodeName);

		foreach (FieldInfo field in structData.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
		{
			MugDebug.Assert(field.FieldType.IsValueType, "Reference type cannot be serialised");
			var value = field.GetValue(structData);

			if (field.FieldType.IsPrimitive)
			{
				// This is a primitive type
				root.Add(new XElement(field.Name,
					new XAttribute("type", field.FieldType.Name),
					value.ToString()));
			}
			else
			{
				// This is a nested struct - recursively serialize it
				root.Add(SerializeStruct(value, field.Name));
			}
		}

		return root;
	}
}

