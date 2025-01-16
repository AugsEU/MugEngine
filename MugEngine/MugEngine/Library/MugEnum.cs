using System.Reflection;

namespace MugEngine.Library
{
	/// <summary>
	/// Helper function to convert enums into different values.
	/// </summary>
	public static class MugEnum
	{
		#region rGeneral

		/// <summary>
		/// Get an enum from a string
		/// </summary>
		static public T GetEnumFromString<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value);
		}



		/// <summary>
		/// Get number of enums
		/// </summary>
		static public int EnumLength(Type enumType)
		{
			return Enum.GetNames(enumType).Length;
		}



		/// <summary>
		/// Get iterator over enum
		/// </summary>
		static public IEnumerable<T> EnumIter<T>() where T : Enum
		{
			foreach (T value in Enum.GetValues(typeof(T)))
			{
				yield return value;
			}
		}



		/// <summary>
		/// Get file path attribute
		/// </summary>
		public static string GetFilePath(this Enum value)
		{
			FieldInfo field = value.GetType().GetField(value.ToString());

			MFilePathAttribute attribute = field.GetCustomAttribute<MFilePathAttribute>();

			return attribute?.mPath;
		}

		#endregion rGeneral
	}



	/// <summary>
	/// File path attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	sealed class MFilePathAttribute : Attribute
	{
		public readonly string mPath;

		public MFilePathAttribute(string path)
		{
			mPath = path;
		}
	}
}
