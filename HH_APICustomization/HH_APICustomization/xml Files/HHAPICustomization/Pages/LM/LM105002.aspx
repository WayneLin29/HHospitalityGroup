<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM105002.aspx.cs" Inherits="Pages_LM105000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMCloudbedAccountMappingMaint"
        PrimaryView="AccountMapping">
        <CallbackCommands>
            <%--<px:PXDSCallbackCommand Name="importData" Visible="true"></px:PXDSCallbackCommand>--%>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="AccountMappingForm" runat="server" DataSourceID="ds" DataMember="AcctFilter" Width="100%" Height="80px" AllowAutoHide="false">
        <Template>
            <px:PXSelector runat="server" ID="edFCloudBedPropertyID" DataField="CloudBedPropertyID" CommitChanges="true" Width="150px"></px:PXSelector>
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:PXGrid ID="gridCloudBedAccountMapping" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 372px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True">
        <Levels>
            <px:PXGridLevel DataMember="AccountMapping">
                <Columns>
                    <px:PXGridColumn DataField="SequenceID"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CloudBedPropertyID" CommitChanges="true" />
                    <px:PXGridColumn DataField="Type" />
                    <px:PXGridColumn DataField="TransCategory" />
                    <px:PXGridColumn DataField="HouseAccount" />
                    <px:PXGridColumn DataField="TransactionCode" />
                    <px:PXGridColumn DataField="Description" />
                    <px:PXGridColumn DataField="Source" />
                    <px:PXGridColumn DataField="AccountID" />
                    <px:PXGridColumn DataField="SubAccountID" />
                </Columns>
                <RowTemplate>
                    <px:PXSelector runat="server" ID="edCloudBedPropertyIDMap" DataField="CloudBedPropertyID"></px:PXSelector>
                    <px:PXSelector runat="server" ID="edAccountIDMap" DataField="AccountID"></px:PXSelector>
                    <px:PXSelector runat="server" ID="edSubAccountIDMap" DataField="SubAccountID"></px:PXSelector>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <Mode AllowUpload="True" />
    </px:PXGrid>
</asp:Content>
