var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "./L3.WorkItemHistory.Resources", "WorkItemTracking/Scripts/TFS.WorkItemTracking.Controls", "Presentation/Scripts/TFS/TFS.UI.Controls", "WorkItemTracking/Scripts/TFS.WorkItemTracking", "Presentation/Scripts/TFS/TFS.Core", "Presentation/Scripts/TFS/Generated/TFS.WorkItemTracking.Constants", "Presentation/Scripts/TFS/TFS.Html", "Presentation/Scripts/TFS/TFS.UI.Controls.Common"], function(require, exports, FHResources, WIControl, __Controls__, __WITOM__, __Core__, __WITConstants__, __TFS_Html__, __CommonControls__) {
    var Controls = __Controls__;
    var WITOM = __WITOM__;
    var Core = __Core__;
    var WITConstants = __WITConstants__;
    var TFS_Html = __TFS_Html__;
    var CommonControls = __CommonControls__;

    var LogTabs;
    (function (LogTabs) {
        LogTabs[LogTabs["Discussion"] = 0] = "Discussion";
        LogTabs[LogTabs["All_Canges"] = 1] = "All_Canges";
        LogTabs[LogTabs["Field_History"] = 2] = "Field_History";
    })(LogTabs || (LogTabs = {}));

    var WIHistoryLogControl = (function (_super) {
        __extends(WIHistoryLogControl, _super);
        function WIHistoryLogControl() {
            _super.apply(this, arguments);
        }
        WIHistoryLogControl.prototype._init = function () {
            _super.prototype._init.call(this);
            var that = this;
            var idBase = Controls.getId();
            var fieldHistoryId = "fieldHistory_" + idBase;

            var tabButtonHost = $(this._tabHost).find("ul");
            $("<li/>").appendTo(tabButtonHost).append($("<a/>").attr("href", "#" + fieldHistoryId).text("Field History"));
            this._fieldHistory = $("<div/>").addClass("fieldHistory-host details-host").attr("id", fieldHistoryId).appendTo(this._tabHost);

            this._tabHost.tabs({
                selected: 0,
                show: function (e, ui) {
                    that._loadTab(ui.index);
                }
            });
        };

        WIHistoryLogControl.prototype._loadTab = function (index) {
            if (!this._workItem) {
                return;
            }
            _super.prototype._loadTab.call(this, index);
            if (index === 2 /* Field_History */) {
                if (this._isFieldHistoryControlNotCreated()) {
                    this._createFieldHistoryControl();
                }
                this._loadFieldHistory();
            }
        };

        WIHistoryLogControl.prototype._isFieldHistoryControlNotCreated = function () {
            return !this._fieldHistoryCombo;
        };

        WIHistoryLogControl.prototype._createFieldHistoryControl = function () {
            var fhFields = this._getWorkItemHistoryFields(this._workItem);

            var idBase = Controls.getId();
            var fhComboId = "fieldHistory-fieldname" + idBase;

            var $label = $("<label/>").attr("for", fhComboId).html("Field Name:");
            $label.appendTo(this._fieldHistory);

            var comboOptions = {};
            this._fieldHistoryCombo = Controls.BaseControl.createIn(CommonControls.Combo, this._fieldHistory, $.extend({
                source: fhFields,
                sorted: false,
                change: Core.delegate(this, function () {
                    this._loadTab(2 /* Field_History */);
                    return true;
                }),
                id: fhComboId
            }, comboOptions));
        };

        WIHistoryLogControl.prototype._getWorkItemHistoryFields = function (workItem) {
            var fhFields = [];
            workItem.fields.forEach(function (value, index) {
                if (isHistoryField(value, -1)) {
                    fhFields.push(new FieldHistoryItem(value.fieldDefinition.id, value.fieldDefinition.name));
                }
            });

            Core.ArrayUtils.sortIfNotSorted(fhFields, Core.StringUtils.localeIgnoreCaseComparer);
            return fhFields;
        };

        WIHistoryLogControl.prototype._loadFieldHistory = function () {
            this._clearFieldHistoryContainer();

            var selectedIndex = this._fieldHistoryCombo.getSelectedIndex();
            if (selectedIndex === -1) {
                return;
            }

            var wiFields = this._workItem.fields;
            var actions = this._workItem.getHistory().getActions();
            var selectedField = this._fieldHistoryCombo.getBehavior().getDataSource().getItem(selectedIndex);

            var details = new DetailsViewCreator();
            actions.forEach(function (actionSet, index) {
                details.addRow(wiFields, actionSet, selectedField.id);
            });
            this._fieldHistory.append(details.create());
        };

        WIHistoryLogControl.prototype._reset = function () {
            _super.prototype._reset.call(this);
            this._clearFieldHistoryContainer();
        };

        WIHistoryLogControl.prototype._clearFieldHistoryContainer = function () {
            $(this._fieldHistory).find("div.container").remove();
        };
        return WIHistoryLogControl;
    })(WIControl.WorkItemLogControl);
    exports.WIHistoryLogControl = WIHistoryLogControl;

    var FieldHistoryItem = (function () {
        function FieldHistoryItem(id, name) {
            this.id = id;
            this.name = name;
        }
        FieldHistoryItem.prototype.toString = function () {
            return this.name;
        };
        return FieldHistoryItem;
    })();

    var DetailsViewCreator = (function () {
        function DetailsViewCreator() {
        }
        DetailsViewCreator.prototype._getOrCreateTable = function () {
            if (!this.table) {
                this.table = $("<table/>").addClass("detail-list").append(this._createHeaderRow());
            }
            return this.table;
        };
        DetailsViewCreator.prototype._createHeaderRow = function () {
            var $result = $("<tr/>");
            $("<td/>").text(FHResources.Revision).appendTo($result);
            $("<td/>").text(FHResources.ChangedDate).appendTo($result);
            $("<td/>").text(FHResources.ChangedBy).appendTo($result);
            $("<td/>").text(FHResources.NewValue).appendTo($result);
            $("<td/>").text(FHResources.OldValue).appendTo($result);
            return $result;
        };
        DetailsViewCreator.prototype._createRow = function (field, revision, actionSet) {
            var $result = $("<tr/>");
            $("<td/>").text(revision + 1).appendTo($result);
            $("<td/>").text(Core.DateUtils.localeFormat(actionSet.getChangedDate(), "F")).appendTo($result);
            $("<td/>").text(actionSet.changedByName).appendTo($result);
            this._appendValueText($("<td/>").appendTo($result), this._getFieldValueText(field, revision), field.fieldDefinition.type);
            this._appendValueText($("<td/>").appendTo($result), this._getFieldValueText(field, revision - 1), field.fieldDefinition.type);
            return $result;
        };
        DetailsViewCreator.prototype._getFieldValueText = function (field, rev) {
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
        };
        DetailsViewCreator.prototype._appendValueText = function ($element, value, fieldType) {
            if (fieldType === WITConstants.FieldType.Html) {
                fixLinkTargets($element.html(TFS_Html.HtmlNormalizer.normalize(value)));
            } else {
                $element.text(value);
            }
        };
        DetailsViewCreator.prototype.addRow = function (wiFields, actionSet, fieldHistoryId) {
            if (actionSet && actionSet.fieldAction) {
                var revision = actionSet.fieldAction.index;
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
                });
            }
        };
        DetailsViewCreator.prototype.create = function () {
            var result = $("<div/>").addClass("container");
            if (this.table) {
                this.table.appendTo(result);
            } else {
                result.html(FHResources.NoChangesForField);
            }
            return result;
        };
        return DetailsViewCreator;
    })();

    function fixLinkTargets(element) {
        $("a", element).each(function () {
            $(this).attr("target", "_blank");
        });
        return element;
    }

    function isHistoryField(field, revision) {
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
            var val = field.workItem.getFieldValueByRevision(field.fieldDefinition.id, revision);
            if (WITOM.Field.isEmpty(val)) {
                return false;
            }
        }
        return true;
    }

    TFS.tfsModuleLoaded("L3.WorkItemHistory.Controls", exports);
});
