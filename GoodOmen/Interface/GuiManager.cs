using System;

using Dalamud.Interface;
using Dalamud.Interface.Windowing;

namespace GoodOmen.Interface;

public class GuiManager : IDisposable {
	private readonly IUiBuilder _ui;
	
	private readonly WindowSystem _windows = new();
	private readonly ConfigWindow _cfgWindow;
	
	public GuiManager(
		ConfigWindow cfgWindow,
		IUiBuilder ui
	) {
		this._cfgWindow = cfgWindow;
		this._windows.AddWindow(this._cfgWindow);
		
		this._ui = ui;
		this._ui.Draw += this._windows.Draw;
		this._ui.OpenConfigUi += this.ToggleConfigWindow;
	}

	public void ToggleConfigWindow() {
		this._cfgWindow.Toggle();
	}

	public void Dispose() {
		this._ui.Draw -= this._windows.Draw;
		this._ui.OpenConfigUi -= this.ToggleConfigWindow;
	}
}