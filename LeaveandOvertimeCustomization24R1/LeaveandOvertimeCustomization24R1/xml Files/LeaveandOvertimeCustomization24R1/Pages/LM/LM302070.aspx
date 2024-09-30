<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM302070.aspx.cs"
    Inherits="Page_LM302070" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource EnableAttributes="true" ID="ds" runat="server" Visible="True" Width="100%" TypeName="LeaveAndOvertimeCustomization.Graph.OvertimeRequestEntry" PrimaryView="document" HeaderDescriptionField="FormCaptionDescription">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="document" Caption="Document Summary"
        NoteIndicator="True" FilesIndicator="True" ActivityIndicator="True" ActivityField="NoteActivity"
        BPEventsIndicator="True" DefaultControlID="edRefNbr" TabIndex="100">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="SM" />
            <px:PXSelector runat="server" ID="edRefNbr" DataField="RefNbr" CommitChanges="true" FilterByAllFields="True" AutoRefresh="True"></px:PXSelector>
            <px:PXDropDown ID="edOvertimeType" runat="server" DataField="OvertimeType"></px:PXDropDown>
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" Enabled="False" />
            <px:PXCheckBox ID="edHold" runat="server" DataField="Hold" CommitChanges="true"></px:PXCheckBox>
            <px:PXSelector ID="edRequestEmployeeID" runat="server" DataField="RequestEmployeeID"></px:PXSelector>
            <px:PXSelector ID="edWorkgroupID" runat="server" DataField="WorkgroupID"></px:PXSelector>
            <px:PXLayoutRule runat="server" ColumnSpan="3" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="SM" />
            <px:PXLayoutRule runat="server" Merge="True" ControlSize="XM" LabelsWidth="S" />
            <px:PXDateTimeEdit ID="edOvertimeStart_Date" runat="server" CommitChanges="True" DataField="OvertimeStart_Date" />
            <px:PXDateTimeEdit ID="edOvertimeStart_Time" runat="server" CommitChanges="True" DataField="OvertimeStart_Time" TimeMode="True" SuppressLabel="True" />
            <px:PXLayoutRule runat="server" Merge="True" ControlSize="XM" LabelsWidth="S" />
            <px:PXDateTimeEdit ID="edOvertimeEnd_Date" runat="server" CommitChanges="True" DataField="OvertimeEnd_Date" />
            <px:PXDateTimeEdit ID="edOvertimeEnd_Time" runat="server" CommitChanges="True" DataField="OvertimeEnd_Time" TimeMode="True" SuppressLabel="True" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M" />
            <px:PXLabel ID="edempty" runat="server"></px:PXLabel>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M" />
            <px:PXTextEdit ID="edWorkDayDuration" runat="server" DataField="WorkDayDuration"></px:PXTextEdit>
            <px:PXTextEdit ID="edHolidayDuration" runat="server" DataField="HolidayDuration"></px:PXTextEdit>
            <px:PXTextEdit ID="edNationalHolidayDuration" runat="server" DataField="NationalHolidayDuration"></px:PXTextEdit>
            <px:PXDateTimeEdit ID="edRequestDate" runat="server" DataField="RequestDate"></px:PXDateTimeEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="400px" Style="z-index: 100;" Width="100%" DataMember="Approval" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Approvals" BindingContext="form" RepaintOnDemand="false">
                <Template>
                    <px:PXGrid ID="gridApproval" runat="server" DataSourceID="ds" Width="100%" SkinID="DetailsInTab" NoteIndicator="True" Style="left: 0px; top: 0px;">
                        <AutoSize Enabled="True" />
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                        <Levels>
                            <px:PXGridLevel DataMember="Approval">
                                <Columns>
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctCD" />
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctName" />
                                    <px:PXGridColumn DataField="WorkgroupID" />
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctCD" />
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctName" />
                                    <px:PXGridColumn DataField="ApproveDate" />
                                    <px:PXGridColumn DataField="Status" AllowNull="False" AllowUpdate="False" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="Reason" AllowUpdate="False" />
                                    <px:PXGridColumn DataField="AssignmentMapID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="RuleID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="StepID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CreatedDateTime" Visible="false" SyncVisible="false" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
    </px:PXTab>
</asp:Content>

