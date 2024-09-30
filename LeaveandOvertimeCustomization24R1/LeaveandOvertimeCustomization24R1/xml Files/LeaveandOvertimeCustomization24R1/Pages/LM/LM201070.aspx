<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201070.aspx.cs"
    Inherits="Page_LM201070" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Document" TypeName="LeaveAndOvertimeCustomization.EmployeeAnnualLeaveMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="True" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" Caption="Employee Annual Maint" DataMember="Document" FilesIndicator="True"
        NoteIndicator="True" LinkIndicator="True" NotifyIndicator="True" DefaultControlID="editEmployeeID" TabIndex="100">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
            <px:PXSelector runat="server" ID="editEmployeeID" DataField="EmployeeID"></px:PXSelector>
            <px:PXSelector runat="server" ID="editLeaveType" DataField="LeaveType"></px:PXSelector>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
            <px:PXTextEdit runat="server" ID="edAvailCompensatedHrs" DataField="AvailCompensatedHrs"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="edUsedCompensatedHrs" DataField="UsedCompensatedHrs"></px:PXTextEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Height="400px" Style="z-index: 100;" Width="100%" DataMember="Transaction" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Annual" BindingContext="form" RepaintOnDemand="false">
                <Template>
                    <px:PXGrid ID="grid" runat="server" Style="z-index: 100;" Width="100%" Caption="Employee Annual Details" SkinID="Details" Height="300px">
                        <Levels>
                            <px:PXGridLevel DataMember="Transaction">
                                <RowTemplate>
                                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="M" />
                                    <px:PXCheckBox runat="server" DataField="AllowCarryForward" ID="editAllowCarryForward"></px:PXCheckBox>
                                </RowTemplate>
                                <Columns>
                                    <px:PXGridColumn DataField="LineNbr" AllowNull="False" />
                                    <px:PXGridColumn DataField="StartDate" Width="200px" />
                                    <px:PXGridColumn DataField="EndDate" Width="100px" />
                                    <px:PXGridColumn DataField="LeaveHours" Width="100px" CommitChanges="True" />
                                    <px:PXGridColumn DataField="CarryForwardHours" Width="100px" CommitChanges="True"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LeaveDays" Width="100px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="AllowCarryForward" Width="100px" Type="CheckBox" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
                        <CallbackCommands>
                            <Save PostData="Page" />
                        </CallbackCommands>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Compensated" BindingContext="form" RepaintOnDemand="false">
                <Template>
                    <px:PXGrid ID="grid" runat="server" Style="z-index: 100;" Width="100%" Caption="Employee Annual Details" SkinID="Details" Height="300px">
                        <Levels>
                            <px:PXGridLevel DataMember="CompensatedTrans">
                                <Columns>
                                    <px:PXGridColumn DataField="RefNbr" AllowNull="False" />
                                    <px:PXGridColumn DataField="AvailableYear" Width="200px" />
                                    <px:PXGridColumn DataField="TransferHours" Width="100px" />
                                    <px:PXGridColumn DataField="AllowCarryForward" Width="100px" Type="CheckBox" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
                        <CallbackCommands>
                            <Save PostData="Page" />
                        </CallbackCommands>
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
    </px:PXTab>

</asp:Content>
