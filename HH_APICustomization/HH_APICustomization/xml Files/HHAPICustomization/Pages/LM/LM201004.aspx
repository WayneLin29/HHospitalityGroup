<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201004.aspx.cs" Inherits="Pages_LM201004" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMAllowedCombinationMaint"
        PrimaryView="AllowData">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="AllowDataGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="AllowData">
                <Columns>
                    <px:PXGridColumn DataField="BranchID" Width="120" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LedgerID" Width="120" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="AccountID" Width="150" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SubID" Width="200"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Remark" Width="300"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXSelector runat="server" ID="edBranchID" DataField="BranchID"></px:PXSelector>
                    <px:PXSelector runat="server" ID="edLedgerID" DataField="LedgerID"></px:PXSelector>
                    <px:PXSelector runat="server" ID="edAccountID" DataField="AccountID"></px:PXSelector>
                    <px:PXSelector runat="server" ID="edSubID" DataField="SubID"></px:PXSelector>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="True" AllowUpload="True" />
    </px:PXGrid>
</asp:Content>
