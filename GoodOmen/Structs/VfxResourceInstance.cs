using System.Numerics;
using System.Runtime.InteropServices;

namespace GoodOmen.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct VfxResourceInstance {
	[FieldOffset(0xA0)] public Vector4 Color;
}