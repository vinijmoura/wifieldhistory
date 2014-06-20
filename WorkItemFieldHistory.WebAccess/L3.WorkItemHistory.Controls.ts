/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/jqueryui/jqueryui.d.ts" />

import FHResources = require("./L3.WorkItemHistory.Resources");
import WIControl = require("WorkItemTracking/Scripts/TFS.WorkItemTracking.Controls");
import __Controls__ = require("Presentation/Scripts/TFS/TFS.UI.Controls");
import __WITOM__ = require("WorkItemTracking/Scripts/TFS.WorkItemTracking");
import __Core__ = require("Presentation/Scripts/TFS/TFS.Core");
import __WITConstants__ = require("Presentation/Scripts/TFS/Generated/TFS.WorkItemTracking.Constants");
import __TFS_Html__ = require("Presentation/Scripts/TFS/TFS.Html");
import __CommonControls__ = require("Presentation/Scripts/TFS/TFS.UI.Controls.Common");

var Controls = __Controls__;
var WITOM = __WITOM__;
var Core = __Core__;
var WITConstants = __WITConstants__;
var TFS_Html = __TFS_Html__;
var CommonControls = __CommonControls__;


enum LogTabs { Discussion, All_Canges, Field_History }

export class WIHistoryLogControl extends WIControl.WorkItemLogControl {
    _fieldHistory: any;
    _fieldHistoryCombo: any;

    _init() {
        super._init();
        var that = this;
        var idBase = Controls.getId();
        var fieldHistoryId = "fieldHistory_" + idBase;

        var tabButtonHost = $(this._tabHost).find("ul");
        $("<li/>").appendTo(tabButtonHost).append($("<a/>").attr("href", "#" + fieldHistoryId).text("Field History"));
        this._fieldHistory = $("<div/>").addClass("fieldHistory-host details-host").attr("id", fieldHistoryId).appendTo(this._tabHost);

        // chama de novo o método tabs para incluir a nova tab criada aqui
        this._tabHost.tabs({
            selected: 0,
            show: function (e, ui) {
                that._loadTab(ui.index);
            }
        });
    }

    _loadTab(index: number): void {
        if (!this._workItem) {
            return;
        }
        super._loadTab(index);
        if (index === LogTabs.Field_History) {
            if (this._isFieldHistoryControlNotCreated()) {
                this._createFieldHistoryControl();
            }
            this._loadFieldHistory();
        }
    }

    _isFieldHistoryControlNotCreated(): boolean {
        return !this._fieldHistoryCombo;
    }

    _createFieldHistoryControl(): void {
        var fhFields = this._getWorkItemHistoryFields(this._workItem);

        var idBase: number = Controls.getId();
        var fhComboId: string = "fieldHistory-fieldname" + idBase;

        var $label = $("<label/>").attr("for", fhComboId).html("Field Name:");
        $label.appendTo(this._fieldHistory);

        // cria o combo de fieldnames
        var comboOptions = {};
        this._fieldHistoryCombo = Controls.BaseControl.createIn(CommonControls.Combo, this._fieldHistory, $.extend({
            source: fhFields,
            sorted: false,
            change: Core.delegate(this, function () {
                this._loadTab(LogTabs.Field_History);
                return true;
            }),
            id: fhComboId
        }, comboOptions));
    }

    private _getWorkItemHistoryFields(workItem): FieldHistoryItem[] {
        var fhFields: FieldHistoryItem[] = [];
        workItem.fields.forEach(function (value, index: number) {
            if (isHistoryField(value, -1)) {
                fhFields.push(new FieldHistoryItem(value.fieldDefinition.id, value.fieldDefinition.name));
            }
        });

        // faz sort aqui, porque o BaseDataSource.getItemIndex tem um bug quando o combo é ordenado.
        Core.ArrayUtils.sortIfNotSorted(fhFields, Core.StringUtils.localeIgnoreCaseComparer);
        return fhFields;
    }

    _loadFieldHistory(): void {
        this._clearFieldHistoryContainer();

        var selectedIndex: number = this._fieldHistoryCombo.getSelectedIndex();
        if (selectedIndex === -1) {
            return;
        }

        var wiFields = this._workItem.fields;
        var actions = this._workItem.getHistory().getActions();
        var selectedField:FieldHistoryItem = this._fieldHistoryCombo.getBehavior().getDataSource().getItem(selectedIndex);

        var details = new DetailsViewCreator();
        actions.forEach(function (actionSet, index) {
            details.addRow(wiFields, actionSet, selectedField.id);
        });
        this._fieldHistory.append(details.create());
    }

    _reset(): void {
        super._reset();
        this._clearFieldHistoryContainer();
    }

    _clearFieldHistoryContainer(): void {
        $(this._fieldHistory).find("div.container").remove();
    }
}

class FieldHistoryItem {
    id: number;
    name: string
    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
    toString(): string {
        return this.name;
    }
}

class DetailsViewCreator {
    private table;

    constructor() {
    }
    private _getOrCreateTable(): JQuery {
        if (!this.table) {
            this.table = $("<table/>").addClass("detail-list").append(this._createHeaderRow());
        }
        return this.table;
    }
    private _createHeaderRow(): JQuery {
        var $result = $("<tr/>");
        $("<td/>").text(FHResources.Revision).appendTo($result);
        $("<td/>").text(FHResources.ChangedDate).appendTo($result);
        $("<td/>").text(FHResources.ChangedBy).appendTo($result);
        $("<td/>").text(FHResources.NewValue).appendTo($result);
        $("<td/>").text(FHResources.OldValue).appendTo($result);
        return $result;
    }
    private _createRow(field, revision, actionSet): JQuery {
        var $result = $("<tr/>");
        $("<td/>").text(revision + 1).appendTo($result);
        $("<td/>").text(Core.DateUtils.localeFormat(actionSet.getChangedDate(), "F")).appendTo($result);
        $("<td/>").text(actionSet.changedByName).appendTo($result);
        this._appendValueText($("<td/>").appendTo($result), this._getFieldValueText(field, revision), field.fieldDefinition.type);
        this._appendValueText($("<td/>").appendTo($result), this._getFieldValueText(field, revision - 1), field.fieldDefinition.type);
        return $result;
    }
    private _getFieldValueText(field, rev:number): string {
        var result = field.workItem.getFieldValueByRevision(field.fieldDefinition.id, rev);
        var fieldType = field.fieldDefinition.type;
        if (WITOM.Field.isEmpty(result)) {
            result = "";
        } else {
            if (fieldType === WITConstants.FieldType.DateTime) {
                result = Core.DateUtils.localeFormat(result, "F");
            } else {
                result = "" + result;
            }
        }
        return result;
    }
    private _appendValueText($element, value, fieldType) {
        if (fieldType === WITConstants.FieldType.Html) {
            fixLinkTargets($element.html(TFS_Html.HtmlNormalizer.normalize(value)));
        } else {
            $element.text(value);
        }
    }
    addRow(wiFields, actionSet, fieldHistoryId) {
        if (actionSet && actionSet.fieldAction) {
            var revision: number = actionSet.fieldAction.index;
            var that = this;
            wiFields.forEach(function (field, index) {
                if (field.fieldDefinition.id == fieldHistoryId && isHistoryField(field, revision)) {
                    var changedInRev = field.isChangedInRevision(revision);
                    if (!changedInRev && typeof (changedInRev) !== "boolean") {
                        var value = field.workItem.getFieldValueByRevision(field.fieldDefinition.id, revision);
                        var originalValue = field.workItem.getFieldValueByRevision(field.fieldDefinition.id, revision - 1);
                        changedInRev = value !== originalValue;
                    }
                    if (changedInRev) {
                        var tbl = that._getOrCreateTable();
                        tbl.append(that._createRow(field, revision, actionSet));
                    }
                }
            }
        )}
    }
    create(): JQuery {
        var result = $("<div/>").addClass("container");
        if (this.table) {
            this.table.appendTo(result);
        } else {
            result.html(FHResources.NoChangesForField);
        }
        return result;
    }
}

// funções requeridas e não exportadas pelo modulo TFS.WorkItemTracking.Controls
// copiadas sem alterações
function fixLinkTargets(element) {
    $("a", element).each(function () {
        $(this).attr("target", "_blank");
    });
    return element;
}

function isHistoryField(field, revision:number) {
    if (!field.fieldDefinition) {
        return false;
    }
    switch (field.fieldDefinition.id) {
        case WITConstants.CoreField.Id:
        case WITConstants.CoreField.History:
        case WITConstants.CoreField.ChangedDate:
        case WITConstants.CoreField.RevisedDate:
        case WITConstants.CoreField.ChangedBy:
        case WITConstants.CoreField.AuthorizedAs:
        case WITConstants.CoreField.AuthorizedDate:
        case WITConstants.CoreField.Watermark:
        case WITConstants.DalFields.PersonID:
        case WITConstants.CoreField.RelatedLinkCount:
        case WITConstants.CoreField.HyperLinkCount:
        case WITConstants.CoreField.ExternalLinkCount:
        case WITConstants.CoreField.AttachedFileCount:
            return false;
    }
    if (field.workItem.isExtensionMarkerField(field.fieldDefinition)) {
        return false;
    }
    if (revision === 0) {
        var val:string = field.workItem.getFieldValueByRevision(field.fieldDefinition.id, revision);
        if (WITOM.Field.isEmpty(val)) {
            return false;
        }
    }
    return true;
}

TFS.tfsModuleLoaded("L3.WorkItemHistory.Controls", exports);
