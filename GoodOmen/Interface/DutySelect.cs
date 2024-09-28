using System;
using System.Collections.Generic;
using System.Linq;

using Dalamud.Interface.Utility.Raii;
using Dalamud.Plugin.Services;

using Lumina.Excel;
using Lumina.Excel.GeneratedSheets2;

using GLib.Popups;

using ImGuiNET;

namespace GoodOmen.Interface;

public class DutySelect {
	private readonly ExcelSheet<ContentFinderCondition> _content;
	private List<ContentFinderCondition>? _duties;

	private readonly PopupList<ContentFinderCondition> _popupList;
	
	public DutySelect(
		IDataManager data
	) {
		this._content = data.GetExcelSheet<ContentFinderCondition>()!;
		this._popupList = new PopupList<ContentFinderCondition>("##dutySelect", DrawItem).WithSearch(SearchPredicate);
	}

	public void Draw(ref ushort id) {
		var name = id == 0 ? "Select duty" : this.GetDutyName(id);

		bool opening;
		using (var combo = ImRaii.Combo("##DutySelect", name)) {
			opening = combo.Success;
			if (opening) ImGui.CloseCurrentPopup();
		}
		if (opening) this._popupList.Open();

		this._duties ??= this.GetDuties().ToList();
		if (this._popupList.Draw(this._duties, out var selected))
			id = (ushort)selected!.RowId;
	}
	
	public string GetDutyName(ushort id) {
		var row = this._content.GetRow(id);
		if (row == null) return "Unknown";
		
		var name = row.Name.RawString;
		if (name.StartsWith("the "))
			name = name[0].ToString().ToUpper() + name[1..];
		return name;
	}

	private IEnumerable<ContentFinderCondition> GetDuties() => this._content.Where(entry => {
		var type = entry.ContentType.Row;
		return type is (>= 2 and <= 5) or 9 or 10 or 21 or 26 or 28 or 30;
	});

	private static bool DrawItem(ContentFinderCondition row, bool focus) {
		return ImGui.Selectable(row.Name.RawString, focus);
	}

	private static bool SearchPredicate(ContentFinderCondition row, string query)
		=> row.Name.RawString.Contains(query, StringComparison.InvariantCultureIgnoreCase);
}