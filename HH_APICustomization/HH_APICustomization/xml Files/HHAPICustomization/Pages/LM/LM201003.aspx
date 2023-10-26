<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201003.aspx.cs" Inherits="Pages_LM201003" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMHRPayrollPreferenceMaint" PrimaryView="MasterFilter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
    <%--    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="100px" AllowAutoHide="false">
        <Template>
            <px:PXSelector runat="server" ID="edBranchFilter" DataField="BranchID" Width="180" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edDateFrom" DataField="DateFrom" Width="180px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edDateTo" DataField="DateTo" Width="180px" CommitChanges="true"></px:PXDateTimeEdit>
        </Template>
    </px:PXFormView>--%>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="MasterFilter" ID="TAB" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="CONTRIBUTION">
                <Template>
                    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="payrollHourGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
                        <Levels>
                            <px:PXGridLevel DataMember="Contribution">
                                <Columns>
                                    <px:PXGridColumn DataField="Type" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="EffectiveDate" Width="300"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RangeFrom" Width="150"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RangeTo" Width="150"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Method" Type="DropDownList" MatrixMode="True" Width="200" CommitChanges="True"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Employer" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Employee" Width="120"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
                        <ActionBar>
                        </ActionBar>
                        <Mode AllowDelete="True" AllowUpload="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="PAYROLL ACCOUNT MAPPING">
                <Template>
                    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="payrollAdjustmentGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
                        <Levels>
                            <px:PXGridLevel DataMember="PayrollAccountMapping">
                                <Columns>
                                    <px:PXGridColumn DataField="PayrollType" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Branch" Width="180"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="DebitAccount" Width="150"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="DebitSub" Width="150"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreditAcount" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreditSub" Width="200"></px:PXGridColumn>
                                </Columns>
                                <RowTemplate>
                                    <px:PXSelector runat="server" ID="edBranch" DataField="Branch" CommitChanges="true"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edDebitAccount" DataField="DebitAccount" CommitChanges="true"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edDebitSub" DataField="DebitSub"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edCreditAcount" DataField="CreditAcount" CommitChanges="true"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edCreditSub" DataField="CreditSub"></px:PXSelector>
                                </RowTemplate>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
                        <ActionBar>
                        </ActionBar>
                        <Mode AllowDelete="True" AllowUpload="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXTab>
</asp:Content>
