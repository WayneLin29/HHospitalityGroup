<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM101070.aspx.cs" Inherits="Page_LM101070" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LeaveAndOvertimeCustomization.LeaveAndOvertimeSetupMaint" PrimaryView="setup">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="setup" ID="tab" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="General Setting">
                <Template>
                    <px:PXLayoutRule GroupCaption="NUMBERING SETTING" runat="server" ID="CstPXLayoutRule1" StartGroup="True" LabelsWidth="L" ControlSize=""></px:PXLayoutRule>
                    <px:PXSelector runat="server" ID="CstPXSelector3" DataField="LeaveRequestSequenceID" AllowEdit="True"></px:PXSelector>
                    <px:PXSelector runat="server" ID="CstPXSelector21" DataField="OvertimeSequenceID" AllowEdit="True"></px:PXSelector>
                    <px:PXLayoutRule GroupCaption="OVERTIME" runat="server" ID="CstPXLayoutRule2" StartGroup="True" LabelsWidth="M" ControlSize=""></px:PXLayoutRule>
                    <px:PXNumberEdit ID="editMaxOTinWorkDay" DataField="MaxOTinWorkDay" runat="server" AlignLeft="True"></px:PXNumberEdit>
                    <px:PXNumberEdit ID="editMaxOTinHoliday" DataField="MaxOTinHoliday" runat="server" AlignLeft="True"></px:PXNumberEdit>
                    <px:PXNumberEdit ID="editMaxOTPerMonth" DataField="MaxOTPerMonth" runat="server" AlignLeft="True"></px:PXNumberEdit>
                    <px:PXNumberEdit ID="editOTFactor" DataField="OTFactor" runat="server" AlighLeft="True"></px:PXNumberEdit>
                    <px:PXNumberEdit ID="editHolidayFactor" DataField="HolidayFactor" runat="server" AlighLeft="True"></px:PXNumberEdit>
                    <px:PXNumberEdit ID="editNationalholidayFactor" DataField="NationalholidayFactor" runat="server" AlighLeft="True"></px:PXNumberEdit>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Leave Approval">
                <Template>
                    <px:PXPanel ID="panelApproval" runat="server">
                        <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="XM" />
                        <px:PXCheckBox ID="chkXXRequestApproval" runat="server" AlignLeft="True" Checked="True" DataField="LeaveRequestApproval" CommitChanges="True" />
                    </px:PXPanel>
                    <px:PXGrid ID="gridApproval" runat="server" DataSourceID="ds" SkinID="Details" Width="100%">
                        <AutoSize Enabled="True" />
                        <Levels>
                            <px:PXGridLevel DataMember="setupApproval">
                                <RowTemplate>
                                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                                    <px:PXSelector ID="edAssignmentMapID" runat="server" DataField="AssignmentMapID" AllowEdit="True" CommitChanges="True" />
                                    <px:PXSelector ID="edAssignmentNotificationID" runat="server" DataField="AssignmentNotificationID" AllowEdit="True" />
                                    <px:PXSelector ID="edCancelNotificationID" runat="server" DataField="CancelNotificationID" AllowEdit="True" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="AssignmentMapID" Width="250px" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="AssignmentNotificationID" Width="250px" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="CancelNotificationID" Width="250px" RenderEditorText="True" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Overtime Approval">
                <Template>
                    <px:PXPanel ID="panelOTApproval" runat="server">
                        <px:PXLayoutRule runat="server" LabelsWidth="S" ControlSize="XM" />
                        <px:PXCheckBox ID="chkOTApproval" runat="server" AlignLeft="True" Checked="True" DataField="OvertimeApproval" CommitChanges="True" />
                    </px:PXPanel>
                    <px:PXGrid ID="gridOTApproval" runat="server" DataSourceID="ds" SkinID="Details" Width="100%">
                        <AutoSize Enabled="True" />
                        <Levels>
                            <px:PXGridLevel DataMember="OTSetupApproval">
                                <RowTemplate>
                                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
                                    <px:PXSelector ID="edOTAssignmentMapID" runat="server" DataField="AssignmentMapID" AllowEdit="True" CommitChanges="True" />
                                    <px:PXSelector ID="edOTAssignmentNotificationID" runat="server" DataField="AssignmentNotificationID" AllowEdit="True" />
                                    <px:PXSelector ID="edOTCancelNotificationID" runat="server" DataField="CancelNotificationID" AllowEdit="True" />
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="AssignmentMapID" Width="250px" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="AssignmentNotificationID" Width="250px" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="CancelNotificationID" Width="250px" RenderEditorText="True" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXTab>
</asp:Content>
