<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM101071.aspx.cs" Inherits="Page_LM101071" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LeaveAndOvertimeCustomization.LeaveTypeMaint" PrimaryView="leaveTypeList">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="leaveTypeList">
                <Columns>
                    <px:PXGridColumn DataField="LeaveType" Width="180"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Description" Width="250"></px:PXGridColumn>
                    <px:PXGridColumn DataField="MaxLeaveDays" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="MinLeaveHour" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsAnnualLeave" Width="150" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsAttachedRequired" Width="150" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsOnlyWorkDay" Width="150" Type="CheckBox"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXNumberEdit runat="server" ID="editMaxLeaveDays" DataField="MaxLeaveDays"></px:PXNumberEdit>
                    <px:PXNumberEdit runat="server" ID="editMaxLeaveHour" DataField="MaxLeaveHour"></px:PXNumberEdit>
                    <px:PXCheckBox runat="server" ID="editIsAnnualLeave" DataField="IsAnnualLeave"></px:PXCheckBox>
                    <px:PXCheckBox runat="server" ID="editIsAttachedRequired" DataField="IsAttachedRequired"></px:PXCheckBox>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode InitNewRow="True" AllowUpload="True"></Mode>
    </px:PXGrid>
</asp:Content>
