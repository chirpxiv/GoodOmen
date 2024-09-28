using System;

using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility.Signatures;

using GoodOmen.Structs;

namespace GoodOmen.Core;

public class HookManager : IDisposable {
	public bool Enabled => this.CreateOmenHook.IsEnabled;

	public event VfxSpawnEvent? OnSpawn;
	public unsafe delegate void VfxSpawnEvent(VfxResourceInstance* instance);
	
	[Signature("E8 ?? ?? ?? ?? 48 89 84 FB ?? ?? ?? ?? 48 85 C0 74 53", DetourName = nameof(CreateOmenDetour))]
	private Hook<CreateOmenDelegate> CreateOmenHook = null!;
	private unsafe delegate VfxData* CreateOmenDelegate(uint a1, nint a2, nint a3, float a4, int a5, int a6, float a7, int a8, char isEnemy, char a10);
	
	public HookManager(
		IGameInteropProvider interop
	) {
		interop.InitializeFromAttributes(this);
	}

	public void SetEnabled(bool enable) {
		if (enable)
			this.CreateOmenHook.Enable();
		else
			this.CreateOmenHook.Disable();
	}
	
	private unsafe VfxData* CreateOmenDetour(uint a1, nint a2, nint a3, float a4, int a5, int a6, float a7, int a8, char isEnemy, char a10) {
		var vfx = this.CreateOmenHook.Original(a1, a2, a3, a4, a5, a6, a7, a8, isEnemy, a10);
		if (isEnemy == 1 && vfx != null) {
			var instance = vfx->Instance;
			if (instance != null) this.OnSpawn?.Invoke(instance);
		}
		return vfx;
	}

	public void Dispose() {
		this.CreateOmenHook.Disable();
		this.CreateOmenHook.Dispose();
	}
}