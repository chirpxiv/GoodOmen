using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using GoodOmen.Core;
using GoodOmen.Interface;

namespace GoodOmen;

public sealed class GoodOmen : IDalamudPlugin {
	private readonly IDalamudPluginInterface _dpi;
    
	private readonly Config _config;
	private readonly OmenManager _omens;
	private readonly GuiManager _gui;
	private readonly CommandHandler _cmd;
	
	public GoodOmen(
		IDalamudPluginInterface dpi,
		IGameInteropProvider interop,
		ICommandManager cmd,
		IDataManager data
	) {
		this._dpi = dpi;
		this._config = dpi.GetPluginConfig() as Config ?? new Config();
		
		var hooks = new HookManager(interop);
		this._omens = new OmenManager(this._config, hooks);
		this._omens.Initialize();

		var dutySelect = new DutySelect(data);
		var cfgWindow = new ConfigWindow(this._dpi, this._config, this._omens, dutySelect);
		this._gui = new GuiManager(cfgWindow, dpi.UiBuilder);

		this._cmd = new CommandHandler(cmd, this._gui);
		this._cmd.Register();
	}
	
	public void Dispose() {
		this._omens.Dispose();
		this._gui.Dispose();
		this._cmd.Dispose();
		this._dpi.SavePluginConfig(this._config);
	}
}
