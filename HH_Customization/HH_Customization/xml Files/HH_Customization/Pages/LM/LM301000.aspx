<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM301000.aspx.cs" Inherits="Page_LM301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_Customization.Graph.LUMTourGroupEntry"
        PrimaryView="Group"
        >
		<CallbackCommands>
			<px:PXDSCallbackCommand Visible="False" Name="UpdateSO" ></px:PXDSCallbackCommand></CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView SyncPosition="True" ID="form" runat="server" DataSourceID="ds" DataMember="Group" Width="100%" Height="100px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule ControlSize="S" runat="server" ID="CstPXLayoutRule1" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector30" DataField="TourGroupNbr" />
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit7" DataField="DateFrom" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule14" ColumnSpan="4" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit28" DataField="Description" ></px:PXTextEdit>
			<px:PXLayoutRule ControlSize="S" runat="server" ID="CstPXLayoutRule2" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector AllowEdit="True" runat="server" ID="CstPXSelector29" DataField="TourTypeClassID" CommitChanges="True" AutoRefresh="True" ></px:PXSelector>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit9" DataField="DateTo" ></px:PXDateTimeEdit>
			<px:PXLayoutRule ControlSize="S" runat="server" ID="CstPXLayoutRule3" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSegmentMask CommitChanges="True" runat="server" ID="CstPXSegmentMask11" DataField="BranchID" ></px:PXSegmentMask>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule4" StartColumn="True" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit13" DataField="TourGuide" ></px:PXTextEdit>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit12" DataField="Helper" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule5" StartRow="True" ></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule15" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit20" DataField="RevenueTWD" ></px:PXNumberEdit>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit19" DataField="RevenuePHP" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule16" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit21" DataField="CostPHP" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule17" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit23" DataField="GrossProfitTWD" ></px:PXNumberEdit>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit22" DataField="GrossProfitPHP" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule18" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit24" DataField="GrossProfitPer" ></px:PXNumberEdit></Template>
	
		<AutoSize Enabled="True" Container="Window" MinHeight="100" ></AutoSize>
		</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem BindingContext="form" Text="Guests">
				<Template>
					<px:PXGrid Width="100%" SkinID="Details" AllowPaging="True" DataSourceID="ds" SyncPosition="True" runat="server" ID="CstPXGrid25">
						<Levels>
							<px:PXGridLevel DataMember="Guests" >
								<Columns>
									<px:PXGridColumn DataField="SubGroupID" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="NameCH" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="NameEN" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="BirthDay" Width="90" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Age" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="BaseRate" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="AdjAmt" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="Total" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Remark" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="CuryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SOOrderNbr" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SOOrderType" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SOLineNbr" Width="70" ></px:PXGridColumn></Columns>
								<RowTemplate>
									<px:PXSelector runat="server" ID="CstPXSelector31" DataField="SOOrderNbr" AllowEdit="True" /></RowTemplate></px:PXGridLevel></Levels>
						<AutoSize MinHeight="100" Enabled="True" Container="Window" ></AutoSize>
						<Mode InitNewRow="True" InplaceInsert="True" ></Mode>
						
						<ActionBar>
							<CustomItems>
								<px:PXToolBarButton>
									<AutoCallBack Command="UpdateSO" Target="ds" ></AutoCallBack></px:PXToolBarButton></CustomItems></ActionBar></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem BindingContext="form" Text="Items">
				<Template>
					<px:PXGrid SyncPosition="True" Width="100%" SkinID="Details" AllowPaging="True" runat="server" ID="CstPXGrid26" DataSourceID="ds">
						<Levels>
							<px:PXGridLevel DataMember="Items" >
								<Columns>
									<px:PXGridColumn DataField="InventoryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Date" Width="90" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ExtCost" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="CuryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="AccountID" Width="120" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SubID" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="VendorID" Width="140" />
									<px:PXGridColumn DataField="APRefNbr" Width="140" />
									<px:PXGridColumn DataField="APLineNbr" Width="70" /></Columns></px:PXGridLevel></Levels>
						<AutoSize MinHeight="100" Container="Parent" Enabled="True" ></AutoSize>
						<AutoSize Container="Parent" ></AutoSize>
						<Mode InitNewRow="True" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="100" ></AutoSize>
	</px:PXTab>
</asp:Content>