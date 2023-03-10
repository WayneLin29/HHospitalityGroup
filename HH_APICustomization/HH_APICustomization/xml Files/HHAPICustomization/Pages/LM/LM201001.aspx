<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201001.aspx.cs" Inherits="Pages_LM201001" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMPayrollMaint"
        PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="100px" AllowAutoHide="false">
        <Template>
            <px:PXSelector runat="server" ID="edBranchFilter" DataField="BranchID" Width="180" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edDateFrom" DataField="DateFrom" Width="180px" CommitChanges="true"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edDateTo" DataField="DateTo" Width="180px" CommitChanges="true"></px:PXDateTimeEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="payrollHour" ID="TAB" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="Working Hour">
                <Template>
                    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="payrollHourGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
                        <Levels>
                            <px:PXGridLevel DataMember="payrollHour">
                                <Columns>
                                    <px:PXGridColumn DataField="BranchID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="WorkingDate" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="EmployeeID" Width="150" DisplayMode="Hint"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="EarningType" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Hour" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Remark" Width="300"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreatedByID" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreatedDateTime" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="120"></px:PXGridColumn>
                                </Columns>
                                <RowTemplate>
                                    <px:PXDateTimeEdit runat="server" ID="edWorkingDate" DataField="WorkingDate"></px:PXDateTimeEdit>
                                    <px:PXSelector runat="server" ID="edEmployeeID" DataField="EmployeeID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edEarningType" DataField="EarningType"></px:PXSelector>
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
            <px:PXTabItem Text="Adjustment">
                <Template>
                    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="payrollAdjustmentGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
                        <Levels>
                            <px:PXGridLevel DataMember="payrollAdjustment">
                                <Columns>
                                    <px:PXGridColumn DataField="Branch" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="AdjustmentDate" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="EmployeeID" Width="150" DisplayMode="Hint"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="AdjustmentType" Width="150"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Amount" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Remark" Width="180"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreatedByID" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreatedDateTime" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LastModifiedByID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="120"></px:PXGridColumn>
                                </Columns>
                                <RowTemplate>
                                    <px:PXDateTimeEdit runat="server" ID="edAdjustmentDate" DataField="AdjustmentDate"></px:PXDateTimeEdit>
                                    <px:PXSelector runat="server" ID="edEmployeeIDAdjustment" DataField="EmployeeID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edAdjustmentType" DataField="AdjustmentType"></px:PXSelector>
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
