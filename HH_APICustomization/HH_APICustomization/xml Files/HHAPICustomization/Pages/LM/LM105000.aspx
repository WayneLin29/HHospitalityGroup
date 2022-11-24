<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM105000.aspx.cs" Inherits="Pages_LM105000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMCloudBedPreferenceMaint"
        PrimaryView="APIPreference">
        <CallbackCommands>
            <%--<px:PXDSCallbackCommand Name="importData" Visible="true"></px:PXDSCallbackCommand>--%>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="APIPreference" ID="APITab" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="CLOUD BED API">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
                    <px:PXLayoutRule ControlSize="L" runat="server" ID="CstPXLayoutRule4" StartGroup="True" GroupCaption="API Settings"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="edClientID" DataField="ClientID" TextMode="Password"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="edClientSecret" DataField="ClientSecret" TextMode="Password"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="edOauthUrl" DataField="OauthUrl" Width="400px"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="edOauthCODE" DataField="OauthCODE" Width="400px"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="edAccessToken" DataField="AccessToken" Width="500px" TextMode="Password"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="edRefreshToken" DataField="RefreshToken" Width="500px" TextMode="Password"></px:PXTextEdit>
                    <px:PXDateTimeEdit runat="server" ID="edRefreshTokenExpiresTime" DataField="RefreshTokenExpiresTime"></px:PXDateTimeEdit>
                    <px:PXTextEdit runat="server" ID="edWebHookUrl" DataField="WebHookUrl" Width="400px"></px:PXTextEdit>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Property">
                <Template>
                    <px:PXGrid ID="gridCloudBed" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 372px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True">
                        <Levels>
                            <px:PXGridLevel DataMember="CloudBedSetup">
                                <Columns>
                                    <px:PXGridColumn DataField="CloudBedPropertyID" CommitChanges="true" />
                                    <px:PXGridColumn DataField="BranchID" />
                                    <px:PXGridColumn DataField="ClearingAcct" />
                                    <px:PXGridColumn DataField="ClearingSub" />
                                    <px:PXGridColumn DataField="DebitAcct" />
                                    <px:PXGridColumn DataField="DebitSub" />
                                    <px:PXGridColumn DataField="CreditAcct" />
                                    <px:PXGridColumn DataField="CreditSub" />
                                    <px:PXGridColumn DataField="Active" Type="CheckBox"></px:PXGridColumn>
                                </Columns>
                                <RowTemplate>
                                    <px:PXSelector runat="server" ID="edBranchID" DataField="BranchID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edClearingAcct" DataField="ClearingAcct"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edClearingSub" DataField="ClearingSub"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edDebitAcct" DataField="DebitAcct"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edDebitSub" DataField="DebitSub"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edCreditAcct" DataField="CreditAcct"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edCreditSub" DataField="CreditSub"></px:PXSelector>
                                </RowTemplate>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode AllowUpload="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Account Mapping">
                <Template>
                     <px:PXFormView ID="AccountMappingForm" runat="server" DataSourceID="ds" DataMember="AcctFilter" Width="100%" Height="80px" AllowAutoHide="false">
                        <Template>
                            <px:PXSelector runat="server" ID="edFCloudBedPropertyID" DataField="CloudBedPropertyID" CommitChanges="true" Width="150px"></px:PXSelector>
                        </Template>
                    </px:PXFormView>
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
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXTab>
</asp:Content>
