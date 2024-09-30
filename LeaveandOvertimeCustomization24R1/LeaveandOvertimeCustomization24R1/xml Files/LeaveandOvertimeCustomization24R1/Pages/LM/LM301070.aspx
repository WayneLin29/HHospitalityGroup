<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM301070.aspx.cs"
    Inherits="Page_LM301070" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource EnableAttributes="true" ID="ds" runat="server" Visible="True" Width="100%" TypeName="LeaveAndOvertimeCustomization.LeaveRequestEntry" PrimaryView="document" HeaderDescriptionField="FormCaptionDescription">
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
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" Enabled="False" />
            <px:PXCheckBox ID="edHold" runat="server" DataField="Hold" CommitChanges="true"></px:PXCheckBox>
            <px:PXSelector ID="edRequestEmployeeID" runat="server" DataField="RequestEmployeeID"></px:PXSelector>
            <px:PXSelector ID="edWorkgroupID" runat="server" DataField="WorkgroupID"></px:PXSelector>
            <px:PXSelector ID="edLeaveType" runat="server" DataField="LeaveType" CommitChanges="true"></px:PXSelector>
            <px:PXLayoutRule runat="server" ColumnSpan="4" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="SM" />
            <px:PXLayoutRule runat="server" Merge="True" ControlSize="XM" LabelsWidth="S" />
            <px:PXDateTimeEdit ID="edLeaveStart_Date" runat="server" CommitChanges="True" DataField="LeaveStart_Date" />
            <px:PXDateTimeEdit ID="edLeaveStart_Time" runat="server" CommitChanges="True" DataField="LeaveStart_Time" TimeMode="True" SuppressLabel="True" />
            <px:PXLayoutRule runat="server" Merge="True" ControlSize="XM" LabelsWidth="S" />
            <px:PXDateTimeEdit ID="edLeaveEnd_Date" runat="server" CommitChanges="True" DataField="LeaveEnd_Date" />
            <px:PXDateTimeEdit ID="edLeaveEnd_Time" runat="server" CommitChanges="True" DataField="LeaveEnd_Time" TimeMode="True" SuppressLabel="True" />
            <px:PXLayoutRule runat="server" ControlSize="XM" LabelsWidth="SM" />
            <px:PXSelector ID="edDelegateEmployeeID" runat="server" DataField="DelegateEmployeeID" Size="M"></px:PXSelector>
            <px:PXDateTimeEdit ID="edRequestDate" runat="server" DataField="RequestDate"></px:PXDateTimeEdit>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="XM" StartGroup="True" />
            <px:PXPanel ID="XX" runat="server" RenderStyle="Simple">
                <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="M" />
                <px:PXTextEdit ID="edDuration" runat="server" DataField="Duration"></px:PXTextEdit>
                <px:PXTextEdit ID="edApprovedLeave" runat="server" DataField="ApprovedLeaveTimes"></px:PXTextEdit>
                <px:PXTextEdit ID="edPendingApprovalLeave" runat="server" DataField="PendingApprovalTimes"></px:PXTextEdit>
                <px:PXTextEdit ID="edEntitledAnnualLeave" runat="server" DataField="EntitledAnnualLeaveTimes"></px:PXTextEdit>
                <px:PXTextEdit ID="edRemainingAvailableHours" runat="server" DataField="RemainingAvailableHours"></px:PXTextEdit>
            </px:PXPanel>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="400px" Style="z-index: 100;" Width="100%" DataMember="transaction" DataSourceID="ds">
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
            <px:PXTabItem Text="Pending Approve Request" BindingContext="form" RepaintOnDemand="false">
                <Template>
                    <px:PXGrid ID="grid" runat="server" Style="z-index: 100;" Width="100%" DataSourceID="ds" Caption="Pending Approve Request" SkinID="Details" Height="300px">
                        <Levels>
                            <px:PXGridLevel DataMember="Transaction">
                                <Columns>
                                    <px:PXGridColumn DataField="LumLeaveRequest__RefNbr" />
                                    <px:PXGridColumn DataField="LumLeaveRequest__Status" Width="200px" />
                                    <px:PXGridColumn DataField="EPEmployee__AcctName" Width="150px" />
                                    <px:PXGridColumn DataField="LeaveType" Width="120px" />
                                    <px:PXGridColumn DataField="LumLeaveRequest__Description" Width="120px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LumLeaveRequest__LeaveStart" Width="200px" />
                                    <px:PXGridColumn DataField="LumLeaveRequest__LeaveEnd" Width="200px" />
                                </Columns>
                                <RowTemplate>
                                    <px:PXSelector runat="server" ID="edRefNbr2" DataField="LumLeaveRequest__RefNbr" CommitChanges="true" AllowEdit="true"></px:PXSelector>
                                </RowTemplate>
                                <Mode AllowAddNew="false" AllowDelete="false" AllowUpdate="false" />
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
    </px:PXTab>
</asp:Content>
