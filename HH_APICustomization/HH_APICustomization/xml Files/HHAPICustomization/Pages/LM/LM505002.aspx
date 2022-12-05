<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505002.aspx.cs" Inherits="Page_LM505002" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Grpah.LUMTouchBistroTransactionProcess"
        PrimaryView="Filter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="45px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule ControlSize="SM" runat="server" ID="CstPXLayoutRule6" StartColumn="True" ></px:PXLayoutRule>
			<px:PXDropDown runat="server" ID="CstPXDropDown8" DataField="DataType" CommitChanges="True" ></px:PXDropDown>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule7" StartColumn="True" ></px:PXLayoutRule>
			<px:PXCheckBox CommitChanges="True" runat="server" ID="CstPXCheckBox9" DataField="IsImported" ></px:PXCheckBox></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid AdjustPageSize="Auto" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="Transactions">
			    <Columns>
				<px:PXGridColumn AllowCheckAll="True" Type="CheckBox" TextAlign="Center" DataField="Selected" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DataType" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn Type="CheckBox" DataField="IsImported" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn DataField="FileID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Date" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="RestaurantID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="AccountID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="SubID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="BatchNbr" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LineNbr" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="AccountName" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Payments" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Deposits" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ChargedToAccount" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Subtotal" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Tips" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Total" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="MenuItem" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="SalesCategory" Width="180" ></px:PXGridColumn>
				<px:PXGridColumn DataField="MenuGroup" Width="180" ></px:PXGridColumn>
				<px:PXGridColumn DataField="MenuItemVoidQty" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Voids" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="MenuItemQty" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="GrossSales" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Discounts" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="NetSales" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Tax1" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Tax2" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Tax3" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Server" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DateTimestamp" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Reason" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Amount" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Register" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Comment" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ErrorMessage" Width="280" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>