<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505006.aspx.cs" Inherits="Pages_LM505006" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMCloudBedRemitTransactionProcess"
        PrimaryView="Document">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="refresh" Visible="true"></px:PXDSCallbackCommand>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="server">
    <px:PXFormView ID="form4" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Document" CaptionVisible="False">
        <Template>
            <px:PXLayoutRule runat="server" ControlSize="SM" LabelsWidth="S" StartColumn="True" />
            <px:PXSelector runat="server" ID="edRefNbr" DataField="RefNbr" Width="150px" CommitChanges="true"></px:PXSelector>
            <px:PXDateTimeEdit runat="server" ID="edDate" DataField="Date" Width="150px" TextMode="DateTime"></px:PXDateTimeEdit>
            <px:PXDropDown runat="server" ID="edShift" DataField="Shift" Width="150px"></px:PXDropDown>
            <px:PXSelector runat="server" ID="edBranch" DataField="Branch" Width="150px"></px:PXSelector>
            <px:PXLayoutRule runat="server" ControlSize="SM" LabelsWidth="S" StartColumn="True" />
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" Enabled="False" />
            <px:PXTextEdit runat="server" ID="edCreatedByID" DataField="CreatedByID"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="edBatchNbr" DataField="BatchNbr" Enabled="False"></px:PXTextEdit>
            <px:PXSelector runat="server" ID="edOwnerID" DataField="OwnerID" Enabled="false"></px:PXSelector>
            <px:PXLayoutRule runat="server" ControlSize="SM" LabelsWidth="S" StartColumn="True" />
            <px:PXSelector runat="server" ID="edVoidedBy" DataField="VoidedBy" Enabled="False"></px:PXSelector>
            <px:PXTextEdit runat="server" ID="edVoidReason" DataField="VoidReason" Enabled="False"></px:PXTextEdit>
            <px:PXTextEdit runat="server" ID="edVoidBatchNbr" DataField="VoidBatchNbr" Enabled="False"></px:PXTextEdit>
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:PXTab DataMember="PaymentTransactions" ID="PXTab1" runat="server" DataSourceID="ds" Height="800px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="PAYMENT CHECK">
                <Template>
                    <px:PXGrid ID="gridPaymentCheck" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 300px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True" AllowPaging="true">
                        <Levels>
                            <px:PXGridLevel DataMember="PaymentTransactions">
                                <Columns>
                                    <px:PXGridColumn DataField="Description" Width="250px" CommitChanges="true"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RecordedAmt" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RemitAmt" Width="150px" CommitChanges="True"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="OpenAmt" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="OPRemark" Width="250px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ADRemark" Width="250px"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoCallBack ActiveBehavior="True" Command="Refresh" Target="gridPaymentDetails" Enabled="True" />
                        <Mode AllowAddNew="False" InitNewRow="False" AllowDelete="False" />
                    </px:PXGrid>
                    <px:PXGrid ID="gridPaymentDetails" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 380px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True">
                        <ActionBar Position="TopAndBottom">
                            <CustomItems>
                                <px:PXToolBarButton Text="TOGGLE OUT">
                                    <AutoCallBack Command="PaymentToggleOut" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="TOGGLE IN">
                                    <AutoCallBack Command="PaymentToggleIn" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="AUDIT TOGGLE OUT">
                                    <AutoCallBack Command="AuditPaymentToggleOut" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="AUDIT TOGGLE IN">
                                    <AutoCallBack Command="AuditPaymentToggleIn" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Levels>
                            <px:PXGridLevel DataMember="PaymentDetails">
                                <Columns>
                                    <px:PXGridColumn DataField="Selected" Type="CheckBox" AllowCheckAll="true" CommitChanges="true" AutoCallBack="true"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ToRemit" Type="CheckBox"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionID" Width="150px" AllowUpdate="False"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReservationID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RemitRefNbr" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="HouseAccountName" Width="200px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LUMCloudBedReservations__GuestName" Width="180px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LUMCloudBedReservations__Source" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Amount" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="UserName" Width="180px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionNotes" Width="240px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="AccountID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="SubAccountID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ToggleByID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ToggleDateTime" Width="150px"></px:PXGridColumn>
                                </Columns>
                                <RowTemplate>
                                    <px:PXTextEdit runat="server" ID="edPmtTransactionID" DataField="TransactionID" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edPmtReservationID" DataField="ReservationID" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edPmtHouseAccountName" DataField="HouseAccountName" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edPmtAmount" DataField="Amount" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edPmtUserName" DataField="UserName" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edPmtTransactionNotes" DataField="TransactionNotes" Enabled="false"></px:PXTextEdit>
                                    <px:PXSelector runat="server" ID="edPmtAccountID" DataField="AccountID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edPmtSubAcoountID" DataField="SubAccountID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edPmtToggleByID" DataField="ToggleByID" Enabled="false"></px:PXSelector>
                                    <px:PXDateTimeEdit runat="server" ID="edPmtToggleDateTime" DataField="ToggleDateTime" Enabled="false"></px:PXDateTimeEdit>
                                </RowTemplate>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode AllowAddNew="False" AllowDelete="False" InitNewRow="False" />
                        <AutoSize Enabled="true" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="RESERVATION CHECK">
                <Template>
                    <px:PXGrid ID="gridReservationCheck" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 300px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True" AllowPaging="true">
                        <ActionBar Position="TopAndBottom">
                            <CustomItems>
                                <px:PXToolBarButton Text="OUT OF SCOPE">
                                    <AutoCallBack Command="OutOfScope" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="IN SCOPE">
                                    <AutoCallBack Command="InScope" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Levels>
                            <px:PXGridLevel DataMember="ReservationTransactions">
                                <Columns>
                                    <px:PXGridColumn DataField="Selected" Type="CheckBox" AllowCheckAll="true" CommitChanges="true" AutoCallBack="true"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="IsOutOfScope" Width="130px" Type="CheckBox"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReservationID" Width="250px" CommitChanges="true"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CheckMessage" Width="250px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="GuestName" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CheckIn" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CheckOut" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="OPRemark" Width="250px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ADRemark" Width="250px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Balance" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Total" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="PendingCount" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RoomRevenue" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Taxes" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Fees" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Others" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Payment" Width="150px" TextAlign="Right"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoCallBack Target="gridReservationDetails" ActiveBehavior="True" Command="Refresh" Enabled="True" />
                        <Mode AllowAddNew="False" InitNewRow="False" AllowDelete="False" />
                    </px:PXGrid>
                    <px:PXGrid ID="gridReservationDetails" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 380px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True">
                        <ActionBar Position="TopAndBottom">
                            <CustomItems>
                                <px:PXToolBarButton Text="TOGGLE OUT">
                                    <AutoCallBack Command="ReservationToggleOut" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="TOGGLE IN">
                                    <AutoCallBack Command="ReservationToggleIn" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="AUDIT TOGGLE OUT">
                                    <AutoCallBack Command="AuditReservationToggleOut" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="AUDITTOGGLE IN">
                                    <AutoCallBack Command="AuditReservationToggleIn" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Levels>
                            <px:PXGridLevel DataMember="ReservationDetails">
                                <Columns>
                                    <px:PXGridColumn DataField="Selected" Type="CheckBox" AllowCheckAll="true" CommitChanges="true" AutoCallBack="true"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ToRemit" Type="CheckBox"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionID" Width="150px" AllowUpdate="False"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReservationID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RemitRefNbr" Width="200px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Description" Width="180px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="DebitAmount" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CreditAmount" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionNotes" Width="240px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="UserName" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ToggleByID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ToggleDateTime" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="AccountID" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="SubAccountID" Width="150px"></px:PXGridColumn>
                                </Columns>
                                <RowTemplate>
                                    <px:PXTextEdit runat="server" ID="edResTransactionID" DataField="TransactionID" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edResReservationID" DataField="ReservationID" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edResRemitRefNbr" DataField="RemitRefNbr" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edResDescription" DataField="Description" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edResDebitAmount" DataField="DebitAmount" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edResCreditAmount" DataField="CreditAmount" Enabled="false"></px:PXTextEdit>
                                    <px:PXTextEdit runat="server" ID="edResUserName" DataField="UserName" Enabled="false"></px:PXTextEdit>
                                    <px:PXSelector runat="server" ID="edResAccountID" DataField="AccountID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edResSubAcoountID" DataField="SubAccountID"></px:PXSelector>
                                    <px:PXSelector runat="server" ID="edResToggleByID" DataField="ToggleByID" Enabled="false"></px:PXSelector>
                                    <px:PXDateTimeEdit runat="server" ID="edResToggleDateTime" DataField="ToggleDateTime" Enabled="false"></px:PXDateTimeEdit>
                                </RowTemplate>
                            </px:PXGridLevel>
                        </Levels>
                        <Mode AllowAddNew="False" AllowDelete="False" InitNewRow="False" />
                        <AutoSize Enabled="true" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="BLOCK ROOM MONITOR">
                <Template>
                    <px:PXGrid ID="grodBlockRoom" runat="server" DataSourceID="ds" RepaintColumns="True" AutoRepaint="True" MatrixMode="True" Style="z-index: 100; left: 0px; top: 0px; height: 300px;" Width="100%" SkinID="Details" BorderWidth="0px" SyncPosition="True" AllowPaging="true">
                        <Levels>
                            <px:PXGridLevel DataMember="RemitBlock">
                                <Columns>
                                    <px:PXGridColumn DataField="BlockID" Width="130px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RoomName" Width="250px" CommitChanges="true"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LUMCloudBedRoomBlock__StartDate" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LUMCloudBedRoomBlock__EndDate" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LUMCloudBedRoomBlock__CreatedDateTime" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LUMCloudBedRoomBlock__LastModifiedDateTime" Width="150px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CheckMessage" Width="250px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="OPRemark" Width="250px"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ADRemark" Width="250px"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoCallBack Target="gridReservationDetails" ActiveBehavior="True" Command="Refresh" Enabled="True" />
                        <Mode AllowAddNew="False" InitNewRow="False" AllowDelete="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="APPROVALS" BindingContext="form" RepaintOnDemand="false">
                <Template>
                    <px:PXGrid ID="gridApproval" runat="server" DataSourceID="ds" Width="100%" SkinID="DetailsInTab" NoteIndicator="True" Style="left: 0px; top: 0px;">
                        <AutoSize Enabled="True" />
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                        <Levels>
                            <px:PXGridLevel DataMember="Approval">
                                <Columns>
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctCD" />
                                    <px:PXGridColumn DataField="ApproverEmployee__AcctName" />
                                    <px:PXGridColumn DataField="WorkgroupID" />
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctCD" />
                                    <px:PXGridColumn DataField="ApprovedByEmployee__AcctName" />
                                    <px:PXGridColumn DataField="ApproveDate" />
                                    <px:PXGridColumn DataField="Status" AllowNull="False" AllowUpdate="False" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="Reason" AllowUpdate="False" />
                                    <px:PXGridColumn DataField="AssignmentMapID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="RuleID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="StepID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CreatedDateTime" Visible="false" SyncVisible="false" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
    </px:PXTab>
    <px:PXSmartPanel ID="pnlVoidRemit" runat="server" CaptionVisible="True" Caption="Void Remittance"
        Style="position: static" LoadOnDemand="True" Key="Document" AutoCallBack-Target="frmVoidRemit"
        AutoCallBack-Command="Refresh" DesignView="Content">
        <px:PXFormView ID="frmVoidRemit" runat="server" SkinID="Transparent" DataMember="Document" DataSourceID="ds" EmailingGraph="">
            <Template>
                <px:PXLayoutRule runat="server" ControlSize="M" LabelsWidth="M" StartColumn="True" />
                <px:PXLabel ID="lblWarning" runat="server" Encode="True">
					Please specify void reason
                </px:PXLabel>
                <px:PXTextEdit ID="edVoidReason" runat="server" AutoRefresh="True" DataField="VoidReason" DataSourceID="ds" />

                <px:PXPanel ID="PXPanel1" runat="server" SkinID="Buttons">
                    <px:PXButton ID="btnMyCommandOK" runat="server" DialogResult="OK" Text="OK">
                        <AutoCallBack Command="Save" Target="frmVoidRemit" />
                    </px:PXButton>
                    <px:PXButton ID="btnMyCommandCancel" runat="server" DialogResult="Cancel" Text="Cancel" />
                </px:PXPanel>
            </Template>
        </px:PXFormView>
    </px:PXSmartPanel>
</asp:Content>

