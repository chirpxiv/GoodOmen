using System;

using Dalamud.Game.Command;
using Dalamud.Plugin.Services;

using GoodOmen.Interface;

namespace GoodOmen.Core;

public class CommandHandler : IDisposable {
	private const string CommandName = "/goodomen";
	
	private readonly ICommandManager _cmd;
	private readonly GuiManager _gui;
	
	public CommandHandler(
		ICommandManager cmd,
		GuiManager gui
	) {
		this._cmd = cmd;
		this._gui = gui;
	}

	public void Register() {
		this._cmd.AddHandler(CommandName, new CommandInfo(this.HandleCommand) {
			HelpMessage = "Opens the GoodOmen configuration window."
		});
	}

	private void HandleCommand(string _, string args) {
		this._gui.ToggleConfigWindow();
	}

	public void Dispose() {
		this._cmd.RemoveHandler(CommandName);
	}
}