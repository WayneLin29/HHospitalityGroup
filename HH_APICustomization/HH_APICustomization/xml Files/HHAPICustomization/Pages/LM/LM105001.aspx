<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM105001.aspx.cs" Inherits="Page_LM105001" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_APICustomization.Graph.LUMTouchBistroPreferenceMaint"
        PrimaryView="MasterFilter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXTab DataMember="MasterFilter" SyncPosition="" ID="tab" runat="server" DataSourceID="ds" Style="z-index: 100" 
		Width="100%" Height="100%" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Restaurant">
			
				<Template>
					<px:PXGrid TabIndex="300" SyncPosition="True" DataSourceID="ds" Height="100%" AdjustPageSize="Auto" AllowPaging="" runat="server" ID="CstPXGrid1" SkinID="Details" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="Preferences" >
								<Columns>
									<px:PXGridColumn DataField="RestaurantCD" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Branch" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="AccountID" Width="120" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SubAcctID" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn Type="CheckBox" DataField="Active" Width="60" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<Mode AutoInsert="False" ></Mode>
						<Mode InplaceInsert="False" ></Mode></px:PXGrid></Template></px:PXTabItem>
			<px:PXTabItem Text="Account Mapping">
			
				<Template>
								<px:PXFormView DataSourceID="ds" runat="server" ID="CstFormView3" DataMember="Filter" >
									<Template>
										<px:PXSelector runat="server" ID="CstPXSelector4" DataField="RestaurantID" CommitChanges="True" ></px:PXSelector></Template></px:PXFormView>
					<px:PXGrid TabIndex="400" SyncPosition="True" DataSourceID="ds" AdjustPageSize="Auto" AllowPaging="" Height="100%" runat="server" ID="CstPXGrid2" SkinID="Details" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="AccountMappings" >
								<Columns>
									<px:PXGridColumn DataField="Type" Width="180" ></px:PXGridColumn>
									<px:PXGridColumn DataField="RestaurantID" Width="140" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SalesCategory" Width="180" ></px:PXGridColumn>
									<px:PXGridColumn DataField="MenuGroup" Width="180" ></px:PXGridColumn>
									<px:PXGridColumn DataField="MenuItem" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="PayAccount" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Reason" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="AccountID" Width="120" ></px:PXGridColumn>
									<px:PXGridColumn DataField="SubAcctID" Width="140" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<ActionBar>
							<Actions>
								<Upload Enabled="" ></Upload></Actions></ActionBar>
						<Mode AllowUpload="True" ></Mode></px:PXGrid></Template></px:PXTabItem>
		</Items>
	
		<AutoSize Container="Window" Enabled="True" ></AutoSize>
		<AutoSize Container="Window" ></AutoSize></px:PXTab>
</asp:Content>