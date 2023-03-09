<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201000.aspx.cs" Inherits="Page_LM201000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_Customization.Graph.LUMTourTypeClassMaint"
        PrimaryView="ToryTypeClass"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="ToryTypeClass" Width="100%" Height="110px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule1" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="TypeClassCD" ></px:PXSelector>
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask4" DataField="BranchID" ></px:PXSegmentMask>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule6" ColumnSpan="2" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit5" DataField="Description" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule2" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit7" DataField="BaseRate" ></px:PXNumberEdit>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit8" DataField="TargetRate" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule9" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector10" DataField="CuryID" ></px:PXSelector></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="CostStructures">
			    <Columns>
				<px:PXGridColumn DataField="Level" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" DataField="InventoryID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ExtCost" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="AccountID" Width="120" ></px:PXGridColumn>
				<px:PXGridColumn DataField="SubID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="VendorID" Width="140" /></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>