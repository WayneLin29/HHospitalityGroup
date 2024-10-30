<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505000.aspx.cs" Inherits="Pages_LM505000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMCloudBedTransactionProcess"
        PrimaryView="TransacionFilter">
        <CallbackCommands>
            <%--<px:PXDSCallbackCommand Name="importData" Visible="true"></px:PXDSCallbackCommand>--%>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="TransacionFilter" ID="TAB" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="Transaction">
                <Template>
                    <px:PXFormView ID="transactionForm" runat="server" DataSourceID="ds" DataMember="TransacionFilter" Width="100%" Height="110px" AllowAutoHide="false">
                        <Template>
                            <px:PXDateTimeEdit runat="server" ID="edFromDate" DataField="FromDate" Width="180px"></px:PXDateTimeEdit>
                            <px:PXDateTimeEdit runat="server" ID="edToDate" DataField="ToDate" Width="180px"></px:PXDateTimeEdit>
                            <px:PXDropDown runat="server" ID="edProcessType" DataField="ProcessType" Width="200px"></px:PXDropDown>
                            <px:PXCheckBox runat="server" ID="IsImported" DataField="IsImported" CommitChanges="True"></px:PXCheckBox>
                            <px:PXLayoutRule runat="server" ColumnWidth="M" ControlSize="M" StartColumn="true"></px:PXLayoutRule>
                            <px:PXSelector runat="server" ID="edCloudBedPropertyID" DataField="CloudBedPropertyID" CommitChanges="true"></px:PXSelector>
                        </Template>
                    </px:PXFormView>
                    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="TransactionGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
                        <Levels>
                            <px:PXGridLevel DataMember="Transaction">
                                <Columns>
                                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="IsImported" Width="120" Type="CheckBox"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="BatchNbr" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LineNbr" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="PropertyID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReservationID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="SubReservationID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="HouseAccountID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="HouseAccountName" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="GuestID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="PropertyName" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionDateTime" Width="180" DisplayFormat="g"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionDateTimeUTC" Width="180" DisplayFormat="g"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionLastModifiedDateTime" Width="180" DisplayFormat="g"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionLastModifiedDateTimeUTC" Width="180" DisplayFormat="g"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="GuestCheckIn" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="GuestCheckOut" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RoomTypeID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RoomTypeName" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RoomName" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="GuestName" Width="120" Type="CheckBox"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Description" Width="200"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Category" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionCode" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionNotes" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Quantity" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Amount" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Currency" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="UserName" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionType" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionCategory" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ItemCategoryName" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="TransactionID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ParentTransactionID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="CardType" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="IsDeleted" Width="120" Type="CheckBox"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ErrorMessage" Width="200"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
                        <ActionBar>
                        </ActionBar>
                        <Mode AllowDelete="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Reservation">
                <Template>
                    <px:PXFormView ID="ReservationForm" runat="server" DataSourceID="ds" DataMember="ReservationFilter" Width="100%" Height="80px" AllowAutoHide="false">
                        <Template>
                            <px:PXLayoutRule runat="server" Merge="True" ControlSize="XM" LabelsWidth="S" />
                            <px:PXDateTimeEdit runat="server" ID="edReservationFromDate_Date" DataField="ReservationFromDate_Date" Width="120px"></px:PXDateTimeEdit>
                            <px:PXDateTimeEdit runat="server" ID="edReservationFromDate_Time" DataField="ReservationFromDate_Time" TimeMode="True" SuppressLabel="True"></px:PXDateTimeEdit>
                            <px:PXLayoutRule runat="server" Merge="True" ControlSize="XM" LabelsWidth="S" />
                            <px:PXDateTimeEdit runat="server" ID="edReservationToDate_Date" DataField="ReservationToDate_Date" Width="120px"></px:PXDateTimeEdit>
                            <px:PXDateTimeEdit runat="server" ID="edReservationToDate_Time" DataField="ReservationToDate_Time" TimeMode="True" SuppressLabel="True"></px:PXDateTimeEdit>
                        </Template>
                    </px:PXFormView>
                    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="ReservationsGrid" runat="server" DataSourceID="ds" Width="100%" Height="100%" SkinID="Details" AllowAutoHide="false">
                        <Levels>
                            <px:PXGridLevel DataMember="Reservations">
                                <Columns>
                                    <px:PXGridColumn DataField="PropertyID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ReservationID" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="DateCreated" Width="180" DisplayFormat="g"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="DateModified" Width="180" DisplayFormat="g"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Source" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ThirdPartyIdentifier" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Status" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="StartDate" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="EndDate" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Balance" Width="120"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
                        <ActionBar>
                        </ActionBar>
                        <Mode AllowDelete="True" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXTab>
</asp:Content>
