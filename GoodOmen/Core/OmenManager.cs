using System;
using System.Numerics;

using FFXIVClientStructs.FFXIV.Client.Game;

using GoodOmen.Structs;

namespace GoodOmen.Core;

public class OmenManager : IDisposable {
	private readonly Config _config;
	private readonly HookManager _hooks;

	public bool Enabled {
		get => this._hooks.Enabled;
		set {
			this._config.Enabled = value;
			this._hooks.SetEnabled(value);
		}
	}
	
	public OmenManager(
		Config config,
		HookManager hooks
	) {
		this._config = config;
		this._hooks = hooks;
	}

	public unsafe void Initialize() {
		this._hooks.SetEnabled(this._config.Enabled);
		this._hooks.OnSpawn += this.OnSpawn;
	}

	private unsafe void OnSpawn(VfxResourceInstance* instance) {
		if (instance->Color.Equals(Vector4.One) && this.TryGetColor(out var color))
			instance->Color = color;
	}

	private unsafe bool TryGetColor(out Vector4 color) {
		var id = GameMain.Instance()->CurrentContentFinderConditionId;
		if (this._config.DutyColors.TryGetValue(id, out var setup) && setup.Enabled) {
			color = setup.Color;
			return true;
		}
		
		color = this._config.Color;
		return this._config.GlobalColor;
	}

	public void Dispose() {
		this._hooks.Dispose();
	}
}