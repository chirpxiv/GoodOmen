using System.Numerics;

using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

using ImGuiNET;

using GLib.Widgets;

using GoodOmen.Core;

namespace GoodOmen.Interface;

public class ConfigWindow : Window {
	private readonly IDalamudPluginInterface _dpi;
	private readonly Config _config;
	private readonly OmenManager _omens;
	private readonly DutySelect _dutySelect;

	private ushort _newDutyId;
	private Config.DutySetup _newEntry = new();
	
	public ConfigWindow(
		IDalamudPluginInterface dpi,
		Config config,
		OmenManager omens,
		DutySelect dutySelect
	) : base("GoodOmen") {
		this._dpi = dpi;
		this._config = config;
		this._omens = omens;
		this._dutySelect = dutySelect;
	}

	public override void OnClose() {
		this._dpi.SavePluginConfig(this._config);
	}

	public override void Draw() {
		var enabled = this._omens.Enabled;
		if (ImGui.Checkbox("Enabled", ref enabled))
			this._omens.Enabled = enabled;
		
		ImGui.Spacing();

		if (ImGui.CollapsingHeader("Global Settings"))
			this.DrawGlobal();
		
		ImGui.Spacing();

		if (ImGui.CollapsingHeader("Duty Overrides"))
			this.DrawDuties();
	}

	private void DrawGlobal() {
		ImGui.Spacing();
		ImGui.Checkbox("Enable global override", ref this._config.GlobalColor);
		using var _ = ImRaii.Disabled(!this._config.GlobalColor);
		ImGui.ColorEdit4("Tint", ref this._config.Color);
	}

	private void DrawDuties() {
		ImGui.Spacing();

		using var table = ImRaii.Table("##dutyTable", 4, ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.NoSavedSettings);
		if (!table.Success) return;
        
		ImGui.TableSetupColumn("##Status");
		ImGui.TableSetupColumn("Duty", ImGuiTableColumnFlags.WidthStretch);
		ImGui.TableSetupColumn("Tint", ImGuiTableColumnFlags.WidthStretch);
		ImGui.TableSetupColumn("##Delete");
		ImGui.TableHeadersRow();

		var frameHeight = ImGui.GetFrameHeight();
		var buttonSize = new Vector2(frameHeight, frameHeight);
		
		// Draw existing entries

		foreach (var (id, entry) in this._config.DutyColors) {
			using var imId = ImRaii.PushId($"##DutyEntry_{id}");
			
			ImGui.TableNextRow();

			// Enabled
			ImGui.TableSetColumnIndex(0);
			ImGui.Checkbox($"##EntryEnabled_{id}", ref entry.Enabled);
			
			// Duty name
			ImGui.TableSetColumnIndex(1);
			ImGui.Text(this._dutySelect.GetDutyName(id));
			
			// Tint
			
			ImGui.TableSetColumnIndex(2);
			ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - ImGui.GetStyle().ItemSpacing.X);
			ImGui.ColorEdit4($"##EntryTint_{id}", ref entry.Color);
			
			// Delete
			ImGui.TableSetColumnIndex(3);
			
			var safety = ImGui.IsKeyDown(ImGuiKey.ModCtrl) && ImGui.IsKeyDown(ImGuiKey.ModShift);
			using (var _ = ImRaii.Disabled(!safety)) {
				if (Buttons.IconButton(FontAwesomeIcon.Trash, buttonSize)) {
					this._config.DutyColors.Remove(id);
					return;
				}
			}
			
			var hovering = ImGui.IsWindowHovered() && ImGui.IsMouseHoveringRect(ImGui.GetItemRectMin(), ImGui.GetItemRectMax());
			if (!safety && hovering) {
				using var _ = ImRaii.Tooltip();
				ImGui.Text("Click while holding Ctrl and Shift to delete this entry.");
			}
		}
		
		// Add new entry
		
		ImGui.TableNextRow();
		
		// Add button
		ImGui.TableSetColumnIndex(0);
		using (var _ = ImRaii.Disabled(this._newDutyId == 0))
			if (Buttons.IconButton(FontAwesomeIcon.Plus, buttonSize))
				this.AddNewEntry();

		// Duty select
		ImGui.TableSetColumnIndex(1);
		this._dutySelect.Draw(ref this._newDutyId);

		// Tint
		ImGui.TableSetColumnIndex(2);
		ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - ImGui.GetStyle().ItemSpacing.X);
		ImGui.ColorEdit4("##NewEntryTint", ref this._newEntry.Color);
	}

	private void AddNewEntry() {
		var prev = this._config.DutyColors[this._newDutyId] = this._newEntry;
		this._newDutyId = 0;
		this._newEntry = new Config.DutySetup {
			Color = prev.Color
		};
	}
}