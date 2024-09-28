using System.Collections.Generic;
using System.Numerics;

using Dalamud.Configuration;

namespace GoodOmen;

public class Config : IPluginConfiguration {
	public int Version { get; set; } = 1;

	public bool Enabled = true;

	public bool GlobalColor = false;
	public Vector4 Color = new(1.0f);

	public Dictionary<ushort, DutySetup> DutyColors = new();

	public class DutySetup {
		public bool Enabled = true;
		public Vector4 Color = new(1.0f);
	}
}