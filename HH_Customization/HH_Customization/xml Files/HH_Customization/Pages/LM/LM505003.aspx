<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM505003.aspx.cs" Inherits="Page_LM505003" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="HH_Customization.Graph.LUMHRApprovalProcess"
        PrimaryView="Filter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="130px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXDropDown runat="server" ID="CstPXDropDown4" DataField="ProcessType" CommitChanges="True" ></px:PXDropDown>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit2" DataField="CutoffDate" CommitChanges="True" ></px:PXDateTimeEdit>
			<px:PXSelector runat="server" ID="CstPXSelector1" DataField="BranchID" CommitChanges="True" ></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="EmployeeID" CommitChanges="True" ></px:PXSelector></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="ProcessView">
			    <Columns>
				<px:PXGridColumn DataField="Selected" Width="60" Type="CheckBox" AllowCheckAll="True" />
				<px:PXGridColumn DataField="CutoffDate" Width="90" />
				<px:PXGridColumn DataField="BranchID" Width="70" />
				<px:PXGridColumn DataField="EmployeeID" Width="140" />
				<px:PXGridColumn DataField="EmployeeID_EPEmployee_acctName" Width="220" />
				<px:PXGridColumn DataField="Date" Width="90" />
				<px:PXGridColumn DataField="Type" Width="140" />
				<px:PXGridColumn DataField="Hour" Width="100" />
				<px:PXGridColumn DataField="Amount" Width="100" /></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>