<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505007.aspx.cs" Inherits="Pages_LM505007" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMHRPayrollPostingProcess"
        PrimaryView="Document">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Document" Width="100%" Height="100px" AllowAutoHide="false">
        <Template>
            <px:PXSelector runat="server" ID="edDocRefNbr" DataField="DocRefNbr" Width="180px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edBranchID" DataField="BranchID" Width="180px" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edProcessTime" DataField="ProcessTime" Width="180px"></px:PXDateTimeEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="TransactionsGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="Transactions">
                <Columns>
                    <px:PXGridColumn DataField="OriginBranchID" Width="150px" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OriginBatchNbr" Width="150px" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OriginLineNbr" Width="100px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TransactionDate" Width="150px" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Description" Width="200px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PostingBranchID" Width="200px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="AccountID" Width="180px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SubID" Width="150px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TranDesc" Width="150px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="DebitAmount" Width="150px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreditAmount" Width="150px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="RefNbr" Width="150px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrTaxZone" Width="120px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrTaxCategory" Width="120px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LineNbr" Width="80px"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXSegmentMask ID="edOriginBranchID" runat="server" DataField="OriginBranchID" OnValueChange="Commit" AutoRefresh="true" />
                    <px:PXDateTimeEdit runat="server" ID="edTransactionDate" DataField="TransactionDate"></px:PXDateTimeEdit>
                    <px:PXSegmentMask ID="edPostingBranchID" runat="server" DataField="PostingBranchID" OnValueChange="Commit" AutoRefresh="true" />
                    <px:PXSegmentMask ID="edAccountID" runat="server" DataField="AccountID" AutoRefresh="True" OnValueChange="Commit" />
                    <px:PXSegmentMask ID="edSubID" runat="server" DataField="SubID" AutoRefresh="True" OnValueChange="Commit" />
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="True" AllowUpload="True" />
    </px:PXGrid>
</asp:Content>
