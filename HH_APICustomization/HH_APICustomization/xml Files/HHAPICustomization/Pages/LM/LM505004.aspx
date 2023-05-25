﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505004.aspx.cs" Inherits="Pages_LM505004" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMReconcilidProcess"
        PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="transactionForm" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="180px" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="S" ControlSize="S" />
            <px:PXSelector runat="server" ID="edBranchID" DataField="BranchID" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edFilterAccountID" DataField="AccountID" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="edFilterSubID" DataField="SubID" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edFilterDateFrom" DataField="DateFrom" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edFilterDateTo" DataField="DateTo" Width="150px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXCheckBox runat="server" ID="edFilterShowReconciledTrans" DataField="ShowReconciledTrans" CommitChanges="true"></px:PXCheckBox>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="S" />
            <px:PXTextEdit runat="server" ID="edFilterPostingSelectedBalance" DataField="PostingSelectedBalance" Width="200px"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="edFilterReconciledSelectedBalance" DataField="ReconciledSelectedBalance" Width="200px"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="edFilterDifferenceAmt" DataField="DifferenceAmt" Width="200px" CommitChanges="true"></px:PXTextEdit>
        </Template>
    </px:PXFormView>
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="DebitGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="DebitTransactions">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="BatchNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LineNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TranDesc"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TranDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="DebitAmt"></px:PXGridColumn>
                    <px:PXGridColumn DataField="RefNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ThirdPartyIdentifier"></px:PXGridColumn>
                    <px:PXGridColumn DataField="StartDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="EndDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Source"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ReferenceID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrReconciled" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrReconciledDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrReconciledBatch"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Users__UserName"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="false" AllowAddNew="false" />
    </px:PXGrid>
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="CreditGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="CreditTransactions">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="BatchNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LineNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TranDesc"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TranDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreditAmt"></px:PXGridColumn>
                    <px:PXGridColumn DataField="RefNbr"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LUMCloudBedReservations__ThirdPartyIdentifier"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LUMCloudBedReservations__StartDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LUMCloudBedReservations__EndDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LUMCloudBedReservations__Source"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ReferenceID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrReconciled" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrReconciledDate"></px:PXGridColumn>
                    <px:PXGridColumn DataField="UsrReconciledBatch"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Users__UserName"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="false" AllowAddNew="false" />
    </px:PXGrid>
</asp:Content>
