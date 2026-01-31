
using ImGuiNET;
using MugEngine.Tuner;
using System.Reflection;
using TracyWrapper;

namespace MugEngine.Core;

public class MIGTunerWindow<T> : MImGuiWindow where T : struct
{
	Dictionary<Type, FieldInfo[]> mFieldInfos;
	object mTuneStructBox;
	string mXmlPath;

	Dictionary<FieldInfo, (float, float)> mFloatRanges;

	public MIGTunerWindow(string xmlPath) : base("Tuner")
	{
		mXmlPath = xmlPath;
		mFieldInfos = new();
		mTuneStructBox = null;
		mFloatRanges = new();
	}

	protected override void AddWindowCommands(GameTime time)
	{
		Profiler.PushProfileZone("Tuner values", ZoneC.ORANGE);
		
		mTuneStructBox = MTuner<T>.I;
		RenderStruct(mTuneStructBox);
		MTuner<T>.I = (T)mTuneStructBox;

		ImGui.Separator();

		if (ImGui.Button("Load"))
		{
			MTuner<T>.LoadValues(mXmlPath);
		}
		ImGui.SameLine();
		if (ImGui.Button("Save"))
		{
			MTuner<T>.SaveValues(mXmlPath);
		}
		ImGui.SameLine();
		if (ImGui.Button("Bounds"))
		{
			mFloatRanges.Clear();
		}

		Profiler.PopProfileZone();
	}

	void RenderStruct(object obj)
	{
		Type type = obj.GetType();

		if (!mFieldInfos.TryGetValue(type, out var fields))
		{
			fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
			mFieldInfos.Add(type, fields);
		}

		foreach (FieldInfo field in fields)
		{
			object value = field.GetValue(obj);
			field.GetHashCode();

			if (field.FieldType == typeof(float))
			{
				float floatValue = (float)value;
				(float min, float max) = GetFloatMinMax(field, floatValue);
				if (ImGui.SliderFloat(field.Name, ref floatValue, min, max))
				{
					field.SetValueDirect(__makeref(obj), floatValue);
				}
			}
			else if (field.FieldType == typeof(int))
			{
				int intValue = (int)value;
				if (ImGui.InputInt(field.Name, ref intValue))
				{
					field.SetValueDirect(__makeref(obj), intValue);
				}
			}
			else if(field.FieldType == typeof(bool))
			{
				bool boolValue = (bool)value;
				if (ImGui.Checkbox(field.Name, ref boolValue))
				{
					field.SetValueDirect(__makeref(obj), boolValue);
				}
			}
			else if (field.FieldType.IsValueType) // Nested struct
			{
				if (ImGui.CollapsingHeader(field.Name))
				{
					object nestedObj = value;
					RenderStruct(nestedObj);
					field.SetValueDirect(__makeref(obj), nestedObj);
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}

	(float, float) GetFloatMinMax(FieldInfo field, float value)
	{
		if (mFloatRanges.TryGetValue(field, out var minMax))
		{
			return minMax;
		}

		minMax = (0.0f, value * 3.0f);
		if (value < 0.0f)
		{
			minMax = (value * 3.0f, value * 3.0f);
		}
		else if(value == 0.0f)
		{
			minMax = (0.0f, 1.0f);
		}


		mFloatRanges.Add(field, minMax);
		return minMax;
	}
}

