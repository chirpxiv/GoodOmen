using System.Runtime.InteropServices;

namespace GoodOmen.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct VfxData {
	[FieldOffset(0x1B8)] public unsafe VfxResourceInstance* Instance;
}