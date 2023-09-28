<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505005.aspx.cs" Inherits="Pages_LM505005" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:pxdatasource id="ds" runat="server" visible="True" width="100%"
        typename="HH_APICustomization.Graph.LUMORMaintenanceProcess"
        primaryview="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:pxdatasource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:pxformview id="transactionForm" runat="server" datasourceid="ds" datamember="Filter" width="100%" height="250px" allowautohide="false">
        <Template>
            <px:PXLayoutRule runat="server" StartGroup="true" GroupCaption="AP FILTER"></px:PXLayoutRule>
            <px:PXLayoutRule runat="server" StartColumn="true" LabelsWidth="M" ControlSize="S" />
            <px:PXSelector runat="server" ID="edAPRefNbrFrom" DataField="APRefNbrFrom" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edAPRefNbrTo" DataField="APRefNbrTo" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edAPStartDate" DataField="APStartDate" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edAPEndDate" DataField="APEndDate" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXSelector runat="server" ID="edAPBranch" DataField="APBranch" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edAPHBranch" DataField="APHBranch" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edAPVendor" DataField="APVendor" Width="150px" CommitChanges="true"></px:PXSelector>

            <px:PXLayoutRule runat="server" StartGroup="true" GroupCaption="OR FILTER"></px:PXLayoutRule>
            <px:PXLayoutRule runat="server" StartColumn="true" LabelsWidth="M" ControlSize="S" />
            <px:PXTextEdit runat="server" ID="edORNumberFrom" DataField="ORNumberFrom" Width="150px" CommitChanges="true"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="edORNumberTo" DataField="ORNumberTo" Width="150px" CommitChanges="true"></px:PXTextEdit>
            <px:PXDateTimeEdit runat="server" ID="edORDateFrom" DataField="ORDateFrom" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edORDateTo" DataField="ORDateTo" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXSelector runat="server" ID="edORStatus" DataField="ORStatus" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edORVendor" DataField="ORVendor" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edORBranch" DataField="ORBranch" Width="150px" CommitChanges="true"></px:PXSelector>

            <px:PXLayoutRule runat="server" StartGroup="true" GroupCaption="UPDATE"></px:PXLayoutRule>
            <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
            <px:PXSelector runat="server" ID="edUpdORBranch" DataField="UpdORBranch" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edUpdORDate" DataField="UpdORDate" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXSelector runat="server" ID="edUpdORVendor" DataField="UpdORVendor" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXTextEdit runat="server" ID="edUpdORNumber" DataField="UpdORNumber" Width="150px" CommitChanges="true"></px:PXTextEdit>
            <px:PXSelector runat="server" ID="edUpdORStatus" DataField="UpdORStatus" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edUpdORTaxZone" DataField="UpdORTaxZone" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXCheckBox runat="server" ID="edUpdCleanUp" DataField="UpdCleanUp" CommitChanges="true"></px:PXCheckBox>
            <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
            <px:PXSelector runat="server" ID="edUpdProjectID" DataField="UpdProjectID" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edUpdTaskID" DataField="UpdTaskID" Width="150px" CommitChanges="true" AutoRefresh="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edUpdAccountID" DataField="UpdAccountID" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edUpdSubID" DataField="UpdSubID" Width="150px" CommitChanges="true" AutoRefresh="true"></px:PXSelector>
        </Template>
    </px:pxformview>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:pxgrid allowpaging="True" adjustpagesize="Auto" syncposition="True" id="TransactionsGrid" runat="server" datasourceid="ds" width="100%" height="100%" skinid="Details" allowautohide="false">
        <Levels>
            <px:PXGridLevel DataMember="Transactions">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__TranType"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__RefNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APInvoice__DocDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APInvoice__Status"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APInvoice__VendorID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APInvoice__RefNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APInvoice__BranchID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__BranchID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APInvoice__DocDesc"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__LineNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__TranDesc"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__ProjectID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__TaskID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__AccountID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__SubID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__CuryLineAmt"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__UsrORBranch"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__UsrORVendor"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__UsrORDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__UsrOrNumber"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__UsrORStatus"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APTran__UsrORTaxZone"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="false" AllowAddNew="false" />
    </px:pxgrid>
</asp:Content>
