using System;
using System.ComponentModel;

namespace VirtualKeyPad
{
	[TypeConverter(typeof(EnumConverter))]
	[Serializable]
	[Flags]
	public enum VirtualKeyPadMode
	{
		Disabled = 0,
		Touch = 1,
		Mouse = 2,
		TouchAndMouse = Touch | Mouse
	}
}