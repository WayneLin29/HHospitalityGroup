<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505001.aspx.cs" Inherits="Page_LM505001" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMTouchBistroImportProcess"
        PrimaryView="Filter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXFormView runat="server" ID="CstFormView1" DataMember="Filter" DataSourceID="ds" Width="100%" >
		<Template>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule2" StartColumn="True" />
			<px:PXCheckBox runat="server" ID="CstPXCheckBox3" DataField="IsImported" CommitChanges="True" /></Template></px:PXFormView>
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="Receiveds">
			    <Columns>
				<px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="60" Type="CheckBox" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Fileid" Width="70" />
				<px:PXGridColumn DataField="FileName" Width="280" />
				<px:PXGridColumn DataField="MailFrom" Width="280" />
				<px:PXGridColumn DataField="EmailSubject" Width="280" />
				<px:PXGridColumn DataField="CreatedAt" Width="90" />
				<px:PXGridColumn DataField="IsImported" Width="60" Type="CheckBox" /></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid></asp:Content>