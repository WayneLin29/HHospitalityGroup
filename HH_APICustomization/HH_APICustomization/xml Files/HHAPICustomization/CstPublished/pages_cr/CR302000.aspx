<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="CR302000.aspx.cs" Inherits="Page_CR302000"
    Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource EnableAttributes="true" UDFTypeField="ClassID" ID="ds" runat="server" Visible="True" Width="100%" PrimaryView="Contact" TypeName="PX.Objects.CR.ContactMaint">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Cancel" PopupVisible="true" />
            <px:PXDSCallbackCommand Name="Delete" PopupVisible="true" ClosePopup="True" />
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" PopupVisible="true" />
            <px:PXDSCallbackCommand Name="First" StartNewGroup="True" />
            <px:PXDSCallbackCommand Name="Action" StartNewGroup="true" CommitChanges="true" />

            <px:PXDSCallbackCommand Name="AddOpportunity" CommitChanges="True" Visible="False" />
            <px:PXDSCallbackCommand Name="Opportunities_ViewDetails" Visible="False" DependOnGrid="gridOpportunities" />
            <px:PXDSCallbackCommand Name="AddCase" CommitChanges="True" Visible="False" />
            <px:PXDSCallbackCommand Name="Cases_ViewDetails" Visible="False" DependOnGrid="gridCases" />
            <px:PXDSCallbackCommand Name="NewTask" Visible="False" CommitChanges="True" PopupCommand="Cancel" PopupCommandTarget="ds" />
            <px:PXDSCallbackCommand Name="NewEvent" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="NewActivity" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="NewMailActivity" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="ViewActivity" Visible="False" CommitChanges="True" DependOnGrid="gridActivities" />
            <px:PXDSCallbackCommand Name="OpenActivityOwner" Visible="False" CommitChanges="True" DependOnGrid="gridActivities" />
            <px:PXDSCallbackCommand Name="ViewOnMap" CommitChanges="true" Visible="false" />
            <px:PXDSCallbackCommand Name="ValidateAddress" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="AddressLookupSelectAction" CommitChanges="true" Visible="false" />
            <px:PXDSCallbackCommand Name="AddressLookup" SelectControlsIDs="form" RepaintControls="None" RepaintControlsIDs="ds,formA" CommitChanges="true" Visible="false" />
            <px:PXDSCallbackCommand Name="Members_CRCampaign_ViewDetails" Visible="False" CommitChanges="True" DependOnGrid="grdCampaignHistory" />
            <px:PXDSCallbackCommand Name="Subscriptions_CRMarketingList_ViewDetails" Visible="False" CommitChanges="True" DependOnGrid="grdMarketingLists" />
            <px:PXDSCallbackCommand Name="ResetPassword" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="ResetPasswordOK" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Relations_TargetDetails" Visible="False" CommitChanges="True" DependOnGrid="grdRelations" />
            <px:PXDSCallbackCommand Name="Relations_EntityDetails" Visible="False" CommitChanges="True" DependOnGrid="grdRelations" />
            <px:PXDSCallbackCommand Name="Relations_ContactDetails" Visible="False" CommitChanges="True" DependOnGrid="grdRelations" />
            <px:PXDSCallbackCommand Name="ActivateLogin" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="EnableLogin" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="DisableLogin" Visible="False" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="UnlockLogin" Visible="False" CommitChanges="True" />

            <px:PXDSCallbackCommand Name="DuplicateMerge" Visible="False" CommitChanges="True" DependOnGrid="PXGridDuplicates" />
            <px:PXDSCallbackCommand Name="DuplicateAttach" Visible="False" CommitChanges="True" DependOnGrid="PXGridDuplicates" />
            <px:PXDSCallbackCommand Name="ViewDuplicate" Visible="false" DependOnGrid="PXGridDuplicates" />
            <px:PXDSCallbackCommand Name="ViewDuplicateRefContact" Visible="false" DependOnGrid="PXGridDuplicates" />
            <px:PXDSCallbackCommand Name="ViewLinkingDuplicateRefContact" Visible="false" />
            <px:PXDSCallbackCommand Name="ViewMergingDuplicateBAccount" Visible="false" />
            <px:PXDSCallbackCommand Name="ViewLinkingDuplicateBAccount" Visible="false" />

            <px:PXDSCallbackCommand Name="syncSalesforce" Visible="false" />
            <px:PXDSCallbackCommand Name="syncHubSpot" Visible="false" />
            <px:PXDSCallbackCommand Name="pullFromHubSpot" Visible="false" />
            <px:PXDSCallbackCommand Name="pushToHubSpot" Visible="false" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" Width="100%" Caption="Contact Summary" DataMember="Contact" DataSourceID="ds" NoteIndicator="True" FilesIndicator="True"
        LinkIndicator="True" BPEventsIndicator="True" DefaultControlID="edContactID">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
            <px:PXSelector ID="edContactID" runat="server" DataField="ContactID" NullText="<NEW>" DisplayMode="Text" TextMode="Search" FilterByAllFields="True" AutoRefresh="True" />
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" />

            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="XM" />
            <px:PXSegmentMask ID="edBAccountID" runat="server" AllowEdit="True" CommitChanges="True" DataField="BAccountID" FilterByAllFields="True" TextMode="Search" DataSourceID="ds" AutoRefresh="True" />
            <px:PXSelector ID="OwnerID" runat="server" DataField="OwnerID" TextMode="Search" DisplayMode="Text" FilterByAllFields="True" AutoRefresh="true" CommitChanges="True" />
            <px:PXDropDown ID="edDuplicateStatus" runat="server" DataField="DuplicateStatus" CommitChanges="True" />
            <px:PXCheckBox ID="edDuplicateFound" runat="server" DataField="DuplicateFound" Visible="False" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="500px" DataMember="ContactCurrent" Width="100%">
        <Items>
            <px:PXTabItem Text="Details" RepaintOnDemand="False">
                <Template>
                    <px:PXLayoutRule runat="server" ControlSize="XM" LabelsWidth="SM" StartColumn="True" />

                    <px:PXLayoutRule ID="PXLayoutRule12" runat="server" GroupCaption="Contact" StartGroup="True" />
                    <px:PXFormView ID="PXFormView1" runat="server" DataMember="ContactCurrent" DataSourceID="ds" SkinID="Transparent">
                        <Template>
                            <px:PXTextEdit ID="edFirstName" runat="server" DataField="FirstName" />
                            <px:PXTextEdit ID="edLastName" runat="server" DataField="LastName" />
                            <px:PXTextEdit ID="edFullName" runat="server" DataField="FullName" />
                            <px:PXTextEdit ID="edSalutation" runat="server" DataField="Salutation" SuppressLabel="False" />
                            <px:PXMailEdit ID="EMail" runat="server" CommandName="NewMailActivity" CommandSourceID="ds" DataField="EMail" CommitChanges="True" />
                            <px:PXLayoutRule ID="PXLayoutRule2" runat="server" Merge="True" />
                            <px:PXDropDown ID="Phone1Type" runat="server" DataField="Phone1Type" Size="S" SuppressLabel="True" CommitChanges="True" TabIndex="-1" />
                            <px:PXLabel ID="lblPhone1" runat="server" Text=" " SuppressLabel="true" />
                            <px:PXMaskEdit ID="Phone1" runat="server" DataField="Phone1" SuppressLabel="True" LabelWidth="34px" />
                            <px:PXLayoutRule ID="PXLayoutRule14" runat="server" Merge="True" />
                            <px:PXDropDown ID="Phone2Type" runat="server" DataField="Phone2Type" Size="S" SuppressLabel="True" CommitChanges="True" TabIndex="-1" />
                            <px:PXLabel ID="lblPhone2" runat="server" Text=" " SuppressLabel="true" />
                            <px:PXMaskEdit ID="Phone2" runat="server" DataField="Phone2" SuppressLabel="True" LabelWidth="34px" />
                            <px:PXLayoutRule ID="PXLayoutRule15" runat="server" Merge="True" />
                            <px:PXDropDown ID="Phone3Type" runat="server" DataField="Phone3Type" Size="S" SuppressLabel="True" CommitChanges="True" TabIndex="-1" />
                            <px:PXLabel ID="lblPhone3" runat="server" Text=" " SuppressLabel="true" />
                            <px:PXMaskEdit ID="Phone3" runat="server" DataField="Phone3" SuppressLabel="True" LabelWidth="34px" />
                            <px:PXLayoutRule ID="PXLayoutRule4" runat="server" Merge="True" />
                            <px:PXDropDown ID="FaxType" runat="server" DataField="FaxType" Size="S" SuppressLabel="True" CommitChanges="True" TabIndex="-1" />
                            <px:PXLabel ID="lblFax" runat="server" Text=" " SuppressLabel="true" />
                            <px:PXMaskEdit ID="Fax" runat="server" DataField="Fax" SuppressLabel="True" LabelWidth="34px" />
                            <px:PXLayoutRule runat="server" />
                            <px:PXLinkEdit ID="WebSite" runat="server" DataField="WebSite" CommitChanges="True" />
                        </Template>
                        <ContentLayout ControlSize="XM" LabelsWidth="SM" OuterSpacing="None" />
                        <ContentStyle BackColor="Transparent" BorderStyle="None" />
                    </px:PXFormView>

                    <px:PXLayoutRule runat="server" GroupCaption="Address" StartGroup="True" StartColumn="True" />
                    <px:PXFormView ID="formA" runat="server" DataMember="AddressCurrent" DataSourceID="ds" SkinID="Transparent">
                        <Template>
                            <px:PXLayoutRule runat="server" />
                            <px:PXFormView ID="panelfordatamember" runat="server" DataMember="ContactCurrent2" DataSourceID="ds" RenderStyle="Simple">
                                <Template>
                                    <px:PXLayoutRule runat="server" LabelsWidth="SM" />
                                    <px:PXCheckBox ID="chkOverrideAddress" runat="server" DataField="OverrideAddress" CommitChanges="True" TabIndex="-1" />
                                </Template>
                                <ContentStyle BackColor="Transparent" BorderStyle="None" />
                            </px:PXFormView>
                            <px:PXLayoutRule runat="server" Merge="false" />
                            <px:PXButton ID="btnAddressLookup" runat="server" CommandName="AddressLookup" CommandSourceID="ds" Size="xs" TabIndex="-1" />
                            <px:PXButton ID="btnViewOnMap" runat="server" CommandName="ViewOnMap" CommandSourceID="ds" Size="xs" Text="View On Map" Height="20px" TabIndex="-1" />
                            <px:PXTextEdit ID="edAddressLine1" runat="server" DataField="AddressLine1" CommitChanges="true" />
                            <px:PXTextEdit ID="edAddressLine2" runat="server" DataField="AddressLine2" CommitChanges="true" />
                            <px:PXTextEdit ID="edCity" runat="server" DataField="City" CommitChanges="true" />
                            <px:PXSelector ID="edState" runat="server" AutoRefresh="True" DataField="State" CommitChanges="True" FilterByAllFields="True" TextMode="Search" DataSourceID="ds" />
                            <px:PXMaskEdit ID="edPostalCode" runat="server" DataField="PostalCode" Size="S" CommitChanges="true" />
                            <px:PXSelector ID="edCountryID" runat="server" AllowEdit="True" DataField="CountryID" FilterByAllFields="True" TextMode="Search" CommitChanges="True" DataSourceID="ds" edit="1" />
                            <px:PXCheckBox ID="chkIsValidated" runat="server" DataField="IsValidated" TabIndex="-1" Enabled="False" />
                        </Template>
                        <ContentLayout ControlSize="XM" LabelsWidth="SM" OuterSpacing="None" />
                        <ContentStyle BackColor="Transparent" BorderStyle="None" />
                    </px:PXFormView>

                    <px:PXLayoutRule ID="PXLayoutRule16" runat="server" GroupCaption="Personal Data Privacy" StartGroup="True" ControlSize="XM" LabelsWidth="SM" />
                    <px:PXCheckBox ID="PXCheckBox1" runat="server" DataField="ConsentAgreement" AlignLeft="True" CommitChanges="True" TabIndex="-1" />
                    <px:PXDateTimeEdit ID="edConsentDate" runat="server" DataField="ConsentDate" CommitChanges="True" />
                    <px:PXDateTimeEdit ID="edConsentExpirationDate" runat="server" DataField="ConsentExpirationDate" CommitChanges="True" />
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Review">
                <Template>
                    <px:PXFormView ID="PXReviewForm" runat="server" DataMember="ReviewCurrent" DataSourceID="ds">
                        <Template>
                            <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
                            <px:PXDateTimeEdit runat="server" ID="edReviewDate" DataField="ReviewDate"></px:PXDateTimeEdit>
                            <px:PXSelector runat="server" ID="edReservationID" DataField="ReservationID" FilterByAllFields="true" Width="180px" CommitChanges="True"></px:PXSelector>
                            <px:PXSelector runat="server" ID="edRoomID" DataField="RoomID" Width="300px" CommitChanges="true"></px:PXSelector>
                            <px:PXNumberEdit runat="server" ID="edScore" DataField="Score" Width="80px"></px:PXNumberEdit>
                            <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
                            <px:PXTextEdit runat="server" ID="edPropertyID" DataField="LUMCloudBedReservations__PropertyID" Width="100px"></px:PXTextEdit>
                            <px:PXTextEdit runat="server" ID="edCloudBedBranch" DataField="LUMCloudBedPreference__BranchID" Width="120px"></px:PXTextEdit>
                            <px:PXTextEdit runat="server" ID="edSource" DataField="LUMCloudBedReservations__Source" Width="100px"></px:PXTextEdit>
                            <px:PXDateTimeEdit runat="server" ID="edCheckin" DataField="LUMCloudBedRoomAssignment__Checkin"></px:PXDateTimeEdit>
                            <px:PXDateTimeEdit runat="server" ID="edCheckout" DataField="LUMCloudBedRoomAssignment__Checkout"></px:PXDateTimeEdit>
                            <px:PXLayoutRule runat="server" StartRow="true"></px:PXLayoutRule>
                            <px:PXRichTextEdit ID="edReviewText" runat="server" DataField="ReviewText"
                                Style="width: 100%;height: 120px" AllowAttached="true" AllowSearch="true"
                                AllowMacros="true" AllowLoadTemplate="false" AllowSourceMode="true">
                                <AutoSize Enabled="True" MinHeight="500" />
                            </px:PXRichTextEdit>
                        </Template>
                    </px:PXFormView>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Activities" LoadOnDemand="True" RepaintOnDemand="False">
                <Template>
                    <pxa:PXGridWithPreview ID="gridActivities" runat="server" DataSourceID="ds" Width="100%" AllowSearch="True" DataMember="Activities" AllowPaging="true" NoteField="NoteText"
                        FilesField="NoteFiles" BorderWidth="0px" GridSkinID="Inquire" SplitterStyle="z-index: 100; border-top: solid 1px Gray;  border-bottom: solid 1px Gray"
                        PreviewPanelStyle="z-index: 100; background-color: Window" PreviewPanelSkinID="Preview" BlankFilterHeader="All Activities" MatrixMode="true" PrimaryViewControlID="form">
                        <ActionBar DefaultAction="cmdViewActivity" CustomItemsGroup="0" PagerVisible="False">
                            <CustomItems>
                                <px:PXToolBarButton Key="cmdAddTask">
                                    <AutoCallBack Command="NewTask" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Key="cmdAddEvent">
                                    <AutoCallBack Command="NewEvent" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Key="cmdAddEmail">
                                    <AutoCallBack Command="NewMailActivity" Target="ds" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Key="cmdAddActivity">
                                    <AutoCallBack Command="NewActivity" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Levels>
                            <px:PXGridLevel DataMember="Activities">
                                <Columns>
                                    <px:PXGridColumn DataField="IsCompleteIcon" Width="21px" AllowShowHide="False" AllowResize="False" ForceExport="True" />
                                    <px:PXGridColumn DataField="PriorityIcon" Width="21px" AllowShowHide="False" AllowResize="False" ForceExport="True" />
                                    <px:PXGridColumn DataField="CRReminder__ReminderIcon" Width="21px" AllowShowHide="False" AllowResize="False" ForceExport="True" />
                                    <px:PXGridColumn DataField="ClassIcon" Width="31px" AllowShowHide="False" AllowResize="False" ForceExport="True" />
                                    <px:PXGridColumn DataField="ClassInfo" />
                                    <px:PXGridColumn DataField="RefNoteID" Visible="false" AllowShowHide="False" />
                                    <px:PXGridColumn DataField="ContactID" LinkCommand="OpenActivityContact" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="Subject" LinkCommand="ViewActivity" />
                                    <px:PXGridColumn DataField="UIStatus" />
                                    <px:PXGridColumn DataField="Released" TextAlign="Center" Type="CheckBox" />
                                    <px:PXGridColumn DataField="StartDate" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="CreatedDateTime" DisplayFormat="g" Visible="False" />
                                    <px:PXGridColumn DataField="TimeSpent" />
                                    <px:PXGridColumn DataField="CreatedByID" Visible="false" AllowShowHide="False" />
                                    <px:PXGridColumn DataField="CreatedByID_Creator_Username" Visible="false" SyncVisible="False" SyncVisibility="False">
                                        <NavigateParams>
                                            <px:PXControlParam Name="PKID" ControlID="gridActivities" PropertyName="DataValues[&quot;CreatedByID&quot;]" />
                                        </NavigateParams>
                                    </px:PXGridColumn>
                                    <px:PXGridColumn DataField="WorkgroupID" />
                                    <px:PXGridColumn DataField="OwnerID" LinkCommand="OpenActivityOwner" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="ProjectID" AllowShowHide="true" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="ProjectTaskID" AllowShowHide="true" Visible="false" SyncVisible="false" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <PreviewPanelTemplate>
                            <px:PXHtmlView ID="edBody" runat="server" DataField="body" TextMode="MultiLine" MaxLength="50" Width="100%" Height="100px" SkinID="Label">
                                <AutoSize Container="Parent" Enabled="true" />
                            </px:PXHtmlView>
                        </PreviewPanelTemplate>
                        <AutoSize Enabled="true" />
                        <GridMode AllowAddNew="False" AllowDelete="False" AllowFormEdit="False" AllowUpdate="False" AllowUpload="False" />
                    </pxa:PXGridWithPreview>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="CRM Info" LoadOnDemand="True">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="True" GroupCaption="CRM" StartGroup="True" ControlSize="XM" LabelsWidth="SM" />
                    <px:PXSelector ID="edClassID" runat="server" AllowEdit="True" DataField="ClassID" FilterByAllFields="True" TextMode="Search" CommitChanges="True" />
                    <px:PXSelector CommitChanges="True" ID="edWorkgroupID" runat="server" DataField="WorkgroupID" TextMode="Search" DisplayMode="Text" FilterByAllFields="True" />
                    <px:PXSegmentMask ID="edParentBAccountID" runat="server" AllowEdit="True" DataField="ParentBAccountID" FilterByAllFields="True" TextMode="Search" DataSourceID="ds" AutoRefresh="True" />
                    <px:PXTextEdit ID="edExtRefNbr" runat="server" DataField="ExtRefNbr" />
                    <px:PXDropDown ID="edSource" runat="server" DataField="Source" />
                    <px:PXCheckBox ID="edSynchronize" runat="server" DataField="Synchronize" TabIndex="-1" />

                    <px:PXLayoutRule ID="PXLayoutRule11" runat="server" GroupCaption="Activities" StartGroup="True" />
                    <px:PXDateTimeEdit ID="edLastIncomingDate" runat="server" DataField="ContactActivityStatistics.LastIncomingActivityDate" Size="SM" />
                    <px:PXDateTimeEdit ID="edLastOutgoingDate" runat="server" DataField="ContactActivityStatistics.LastOutgoingActivityDate" Size="SM" />

                    <px:PXLayoutRule ID="PXLayoutRule3" runat="server" GroupCaption="Contact Preferences" StartGroup="True" />
                    <px:PXDropDown ID="Method" runat="server" DataField="Method" />
                    <px:PXLayoutRule ID="PXLayoutRule5" runat="server" Merge="True" />
                    <px:PXCheckBox ID="edNoCall" runat="server" DataField="NoCall" Size="S" SuppressLabel="True" Width="112" TabIndex="-1" />
                    <px:PXCheckBox ID="edNoMarketingMaterials" runat="server" DataField="NoMarketing" Size="S" TabIndex="-1" />
                    <px:PXLayoutRule ID="PXLayoutRule7" runat="server" />
                    <px:PXLayoutRule ID="PXLayoutRule8" runat="server" Merge="True" />
                    <px:PXCheckBox ID="edNoEMail" runat="server" DataField="NoEMail" Size="S" SuppressLabel="True" Width="112" TabIndex="-1" />
                    <px:PXCheckBox ID="edNoMassMail" runat="server" DataField="NoMassMail" Size="S" Width="112" TabIndex="-1" />
                    <px:PXLayoutRule ID="PXLayoutRule9" runat="server" />
                    <px:PXSelector ID="edLanguageID" runat="server" AllowEdit="True" DataField="LanguageID" TextMode="Search" DataSourceID="ds" DisplayMode="Hint" />

                    <px:PXLayoutRule runat="server" StartGroup="True" GroupCaption="Photo" LabelsWidth="SM" ControlSize="XM" StartColumn="True" />
                    <px:PXImageUploader ID="edSignature" runat="server" DataField="Img" DataMember="ContactCurrent" Height="300px" Width="398px" AllowUpload="true" SuppressLabel="true" />

                    <px:PXLayoutRule ID="PXLayoutRule6" runat="server" GroupCaption="Person" StartGroup="True" />
                    <px:PXDateTimeEdit ID="edBirthday" runat="server" DataField="DateOfBirth" />
                    <px:PXDropDown ID="ddGender" runat="server" DataField="Gender" />
                    <px:PXDropDown ID="ddMartialStatus" runat="server" DataField="MaritalStatus" />
                    <px:PXTextEdit ID="teSpouse" runat="server" DataField="Spouse" />
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Duplicates" BindingContext="form" VisibleExp="DataControls[&quot;edDuplicateFound&quot;].Value == true" LoadOnDemand="True">
                <Template>
                    <px:PXSplitContainer runat="server" ID="sp1" SplitterPosition="300" SkinID="Horizontal" Height="500px">
                        <AutoSize Enabled="true" />
                        <Template1>
                            <px:PXGrid ID="PXGridDuplicatesForMerging" runat="server" DataSourceID="ds" SkinID="Inquire" Width="100%" Height="200px" MatrixMode="True" SyncPosition="True"
                                Caption="Records for Merging" CaptionVisible="true"
                                RestrictFields="True" AutoGenerateColumns="AppendDynamic" OnRowDataBound="Duplicates_RowDataBound">
                                <ActionBar>
                                    <CustomItems>
                                        <px:PXToolBarButton Text="Merge" Key="cmdMerge" DependOnGrid="PXGridDuplicatesForMerging" StateColumn="CanBeMerged">
                                            <AutoCallBack Command="DuplicateMerge" Target="ds" />
                                            <PopupCommand Command="Cancel" Target="ds" />
                                        </px:PXToolBarButton>
                                    </CustomItems>
                                </ActionBar>
                                <Levels>
                                    <px:PXGridLevel DataMember="DuplicatesForMerging">
                                        <Columns>
                                            <px:PXGridColumn DataField="DuplicateContact__DisplayName" TextAlign="Left" AllowShowHide="False" LinkCommand="ViewMergingDuplicate" />
                                            <px:PXGridColumn DataField="DuplicateContact__FullName" />
                                            <px:PXGridColumn DataField="DuplicateContact__BAccountID" TextAlign="Left" LinkCommand="ViewMergingDuplicateBAccount" />
                                            <px:PXGridColumn DataField="Phone1" />
                                            <px:PXGridColumn DataField="DuplicateContact__EMail" />
                                            <px:PXGridColumn DataField="DuplicateContact__OwnerID" />

                                            <px:PXGridColumn DataField="DuplicateContactID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__WebSite" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__Phone2" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__Phone3" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__AddressLine1" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__AddressLine2" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__State" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__City" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__CountryID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__WorkgroupID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="CRActivityStatistics__LastIncomingActivityDate" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="CRActivityStatistics__LastOutgoingActivityDate" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__FirstName" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__LastName" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__ContactID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="BAccountR__Type" TextAlign="Left" Width="160px" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__CreatedDateTime" TextAlign="Left" Width="130px" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__LastModifiedDateTime" TextAlign="Left" Width="130px" Visible="false" SyncVisible="false" />

                                            <%-- hidden at all --%>
                                            <px:PXGridColumn DataField="CanBeMerged" TextAlign="Center" Type="CheckBox" />
                                        </Columns>
                                        <Layout FormViewHeight="" />
                                    </px:PXGridLevel>
                                </Levels>
                                <AutoSize Enabled="True" MinHeight="200" />
                                <ActionBar>
                                    <Actions>
                                        <Search Enabled="False" />
                                    </Actions>
                                </ActionBar>
                                <Mode AllowAddNew="False" AllowColMoving="False" AllowDelete="False" />
                            </px:PXGrid>
                        </Template1>
                        <Template2>
                            <px:PXGrid ID="PXGridDuplicatesForLinking" runat="server" DataSourceID="ds" SkinID="Inquire" Width="100%" Height="200px" MatrixMode="True" SyncPosition="True"
                                Caption="Records for Association" CaptionVisible="true"
                                RestrictFields="True" AutoGenerateColumns="AppendDynamic" OnRowDataBound="Duplicates_RowDataBound">
                                <ActionBar>
                                    <CustomItems>
                                        <px:PXToolBarButton Text="Attach to Account" Key="cmdAttachToAccount">
                                            <AutoCallBack Command="DuplicateAttach" Target="ds" />
                                            <PopupCommand Command="Cancel" Target="ds" />
                                        </px:PXToolBarButton>
                                    </CustomItems>
                                </ActionBar>
                                <Levels>
                                    <px:PXGridLevel DataMember="DuplicatesForLinking">
                                        <Columns>
                                            <px:PXGridColumn DataField="DuplicateContact__ContactType" TextAlign="Left" AllowShowHide="False" />
                                            <px:PXGridColumn DataField="DuplicateContact__DisplayName" TextAlign="Left" AllowShowHide="False" LinkCommand="ViewLinkingDuplicate" />
                                            <px:PXGridColumn DataField="DuplicateContact__BAccountID" TextAlign="Left" LinkCommand="ViewLinkingDuplicateBAccount" />
                                            <px:PXGridColumn DataField="DuplicateContact__FullName" />
                                            <px:PXGridColumn DataField="Phone1" />
                                            <px:PXGridColumn DataField="DuplicateContact__Email" />
                                            <px:PXGridColumn DataField="DuplicateContact__OwnerID" />

                                            <px:PXGridColumn DataField="DuplicateContactID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="BAccountR__AcctName" TextAlign="Left" Width="160px" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__WebSite" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__Phone2" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__AddressLine1" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__AddressLine2" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__State" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__City" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="Address__CountryID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__WorkgroupID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="BAccountR__Type" TextAlign="Left" Width="160px" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__FirstName" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__LastName" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="CRActivityStatistics__LastIncomingActivityDate" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="CRActivityStatistics__LastOutgoingActivityDate" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__ContactID" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__CreatedDateTime" TextAlign="Left" Width="130px" Visible="false" SyncVisible="false" />
                                            <px:PXGridColumn DataField="DuplicateContact__LastModifiedDateTime" TextAlign="Left" Width="130px" Visible="false" SyncVisible="false" />

                                        </Columns>
                                        <Layout FormViewHeight="" />
                                    </px:PXGridLevel>
                                </Levels>
                                <AutoSize Enabled="True" MinHeight="200" />
                                <ActionBar>
                                    <Actions>
                                        <Search Enabled="False" />
                                    </Actions>
                                </ActionBar>
                                <Mode AllowAddNew="False" AllowColMoving="False" AllowDelete="False" />
                            </px:PXGrid>
                        </Template2>
                    </px:PXSplitContainer>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Attributes">
                <Template>
                    <px:PXGrid ID="PXGridAnswers" runat="server" DataSourceID="ds" SkinID="Inquire" Width="100%"
                        Height="200px" MatrixMode="True">
                        <Levels>
                            <px:PXGridLevel DataMember="Answers" DataKeyNames="EntityType,EntityID,AttributeID">
                                <Columns>
                                    <px:PXGridColumn DataField="AttributeID" TextAlign="Left" AllowShowHide="False" TextField="AttributeID_description" />
                                    <px:PXGridColumn DataField="isRequired" TextAlign="Center" Type="CheckBox" />
                                    <px:PXGridColumn DataField="Value" AllowShowHide="False" AllowSort="False" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="200" />
                        <ActionBar>
                            <Actions>
                                <Search Enabled="False" />
                            </Actions>
                        </ActionBar>
                        <Mode AllowAddNew="False" AllowColMoving="False" AllowDelete="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>


            <px:PXTabItem Text="Leads" LoadOnDemand="True">
                <Template>
                    <px:PXGrid ID="grdLeads" runat="server" Height="400px" Width="100%" Style="z-index: 100" AllowPaging="True" ActionsPosition="Top" DataSourceID="ds" SkinID="Details" SyncPosition="True"
                        AllowSearch="True" AllowFilter="True" FastFilterFields="DisplayName,Salutation,Address__City,EMail,Phone1,Source,CampaignID,Status,OwnerID,WorkgroupID">
                        <Levels>
                            <px:PXGridLevel DataMember="Leads">
                                <Columns>
                                    <px:PXGridColumn DataField="MemberName" LinkCommand="Leads_ViewDetails" AllowShowHide="False" />
                                    <px:PXGridColumn DataField="Salutation" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CreatedDateTime" />
                                    <px:PXGridColumn DataField="Address__City" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="Address__State" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="Address__CountryID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="EMail" />
                                    <px:PXGridColumn DataField="Phone1" />
                                    <px:PXGridColumn DataField="Phone2" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="Phone3" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="Source" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CampaignID" />
                                    <px:PXGridColumn DataField="Status" />
                                    <px:PXGridColumn DataField="OwnerID" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="WorkgroupID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CRActivityStatistics__LastIncomingActivityDate" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CRActivityStatistics__LastOutgoingActivityDate" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="LastModifiedDateTime" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="LastModifiedByID" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="ContactID" Visible="false" SyncVisible="false" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar DefaultAction="cmdViewDoc">
                            <Actions>
                                <Delete Enabled="false" />
                                <FilterBar ToolBarVisible="Top" Order="0" GroupIndex="3" />
                                <AddNew ToolBarVisible="False" />
                            </Actions>
                            <CustomItems>
                                <px:PXToolBarButton Key="cmdAddLead" ImageKey="AddNew" Tooltip="Add New Lead" DisplayStyle="Image">
                                    <AutoCallBack Command="RecordCreation@CreateLead" Target="ds" />
                                    <PopupCommand Command="Refresh" Target="grdLeads" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                        <AutoSize Enabled="True" MinHeight="100" MinWidth="100" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Opportunities" LoadOnDemand="true">
                <Template>
                    <px:PXGrid ID="gridOpportunities" runat="server" DataSourceID="ds" Height="423px" Width="100%" AllowSearch="True" ActionsPosition="Top" SkinID="Inquire">
                        <AutoSize Enabled="True" MinHeight="100" MinWidth="100" />
                        <Levels>
                            <px:PXGridLevel DataMember="Opportunities">
                                <Columns>
                                    <px:PXGridColumn DataField="OpportunityID" LinkCommand="Opportunities_ViewDetails" />
                                    <px:PXGridColumn DataField="Subject" />
                                    <px:PXGridColumn DataField="StageID" />
                                    <px:PXGridColumn DataField="CROpportunityProbability__Probability" TextAlign="Right" />
                                    <px:PXGridColumn DataField="Status" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="CuryProductsAmount" TextAlign="Right" />
                                    <px:PXGridColumn DataField="CuryID" />
                                    <px:PXGridColumn DataField="CloseDate" />
                                    <px:PXGridColumn DataField="WorkgroupID" />
                                    <px:PXGridColumn DataField="OwnerID" DisplayMode="Text" />
                                    <px:PXGridColumn DataField="ClassID" LinkCommand="Opportunities_CLassID_ViewDetails" AllowShowHide="true" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CROpportunityClass__Description" AllowShowHide="true" Visible="false" SyncVisible="false" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                        <ActionBar ActionsText="False" DefaultAction="cmdOpportunityDetails" PagerVisible="False">
                            <CustomItems>
                                <px:PXToolBarButton Key="cmdAddOpportunity" ImageKey="AddNew" Tooltip="Add New Opportunity" DisplayStyle="Image">
                                    <AutoCallBack Command="RecordCreation@AddOpportunity" Target="ds" />
                                    <PopupCommand Command="Refresh" Target="gridOpportunities" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Text="Opportunity Details" Key="cmdOpportunityDetails">
                                    <AutoCallBack Command="Opportunities_ViewDetails" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Cases" LoadOnDemand="True">
                <Template>
                    <px:PXGrid ID="gridCases" runat="server" DataSourceID="ds" Height="423px" Width="100%" AllowSearch="True"
                        SkinID="Inquire" AllowPaging="true" AdjustPageSize="Auto" BorderWidth="0px">
                        <ActionBar DefaultAction="cmdViewCaseDetails" PagerVisible="False">
                            <CustomItems>
                                <px:PXToolBarButton Key="cmdAddCase" ImageKey="AddNew" Tooltip="Add New Case" DisplayStyle="Image">
                                    <ActionBar GroupIndex="0" Order="2" />
                                    <AutoCallBack Command="RecordCreation@AddCase" Target="ds" />
                                    <PopupCommand Command="Refresh" Target="gridCases" />
                                </px:PXToolBarButton>
                                <px:PXToolBarButton Key="cmdViewCaseDetails" Visible="false">
                                    <ActionBar GroupIndex="0" />
                                    <AutoCallBack Command="Cases_ViewDetails" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Levels>
                            <px:PXGridLevel DataMember="Cases">
                                <Columns>
                                    <px:PXGridColumn DataField="CaseCD" LinkCommand="Cases_ViewDetails" />
                                    <px:PXGridColumn DataField="Subject" />
                                    <px:PXGridColumn DataField="CaseClassID" />
                                    <px:PXGridColumn DataField="Severity" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="Status" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="Resolution" RenderEditorText="True" />
                                    <px:PXGridColumn DataField="CreatedDateTime" />
                                    <px:PXGridColumn DataField="InitResponse" DisplayFormat="###:##:##" />
                                    <px:PXGridColumn DataField="TimeEstimated" DisplayFormat="###:##:##" />
                                    <px:PXGridColumn DataField="ResolutionDate" />
                                    <px:PXGridColumn DataField="WorkgroupID" />
                                    <px:PXGridColumn DataField="OwnerID" DisplayMode="Text" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="100" MinWidth="100" />
                        <Mode AllowAddNew="False" AllowDelete="False" AllowUpdate="False" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Campaigns" LoadOnDemand="True">
                <Template>
                    <px:PXGrid ID="grdCampaignHistory" runat="server" Height="400px" Width="100%" Style="z-index: 100"
                        AllowPaging="True" ActionsPosition="Top" AllowSearch="true" DataSourceID="ds" SyncPosition="true"
                        FastFilterFields="CRCampaign__CampaignID,CRCampaign__CampaignName,CRCampaignMembers__MarketingListID,CRCampaign__PromoCodeID"
                        SkinID="Details">
                        <Levels>
                            <px:PXGridLevel DataMember="Members">
                                <Columns>
                                    <px:PXGridColumn DataField="CampaignID" AutoCallBack="true" LinkCommand="Members_CRCampaign_ViewDetails" />
                                    <px:PXGridColumn DataField="CRCampaign__CampaignName" />
                                    <px:PXGridColumn DataField="CRMarketingList__MailListCode" LinkCommand="MarketingListID_ViewDetails" DisplayMode="Hint" />
                                    <px:PXGridColumn DataField="CRCampaign__Status" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CRCampaign__StartDate" />
                                    <px:PXGridColumn DataField="CRCampaign__EndDate" />
                                    <px:PXGridColumn DataField="CRCampaign__PromoCodeID" />
                                    <px:PXGridColumn DataField="CRCampaign__OwnerID" LinkCommand="OwnerID_ViewDetails" />
                                    <px:PXGridColumn DataField="Contact__ContactType" />
                                    <px:PXGridColumn DataField="Contact__FullName" />
                                    <px:PXGridColumn DataField="Contact__Phone1" />
                                    <px:PXGridColumn DataField="CRCampaignMembers__CreatedDateTime" />
                                    <px:PXGridColumn DataField="CRCampaign__CreatedByID" LinkCommand="CreatedByID_ViewDetails" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CRCampaign__CreatedDateTime" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CRCampaign__LastModifiedByID" LinkCommand="LastModifiedByID_ViewDetails" Visible="false" SyncVisible="false" />
                                    <px:PXGridColumn DataField="CRCampaign__LastModifiedDateTime" Visible="false" SyncVisible="false" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="100" MinWidth="100" />
                        <ActionBar DefaultAction="cmdViewDoc">
                            <Actions>
                                <FilterBar ToolBarVisible="Top" Order="0" GroupIndex="3" />
                                <Search ToolBarVisible="Top" />
                            </Actions>
                        </ActionBar>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Notifications" LoadOnDemand="True">
                <Template>
                    <px:PXGrid runat="server" ID="gridNC" SkinID="Details" DataSourceID="ds" BorderWidth="0px" Width="100%" AdjustPageSize="Auto">
                        <Mode AllowAddNew="false"></Mode>
                        <Levels>
                            <px:PXGridLevel DataMember="NWatchers">
                                <Columns>
                                    <px:PXGridColumn AllowUpdate="False" DataField="NotificationSetup__Module" />
                                    <px:PXGridColumn AllowUpdate="False" DataField="NotificationSetup__SourceCD" />
                                    <px:PXGridColumn DataField="NotificationSetup__NotificationCD" />
                                    <px:PXGridColumn AllowUpdate="False" DataField="ClassID" />
                                    <px:PXGridColumn AllowUpdate="False" DataField="EntityDescription" />
                                    <px:PXGridColumn DataField="ReportID" DisplayFormat="CC.CC.CC.CC" />
                                    <px:PXGridColumn DataField="NotificationID" />
                                    <px:PXGridColumn AllowNull="False" DataField="Format" RenderEditorText="True" />
                                    <px:PXGridColumn AllowNull="False" DataField="Hidden" TextAlign="Center" Type="CheckBox" />
                                    <px:PXGridColumn DataField="Active" TextAlign="Center" Type="CheckBox" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="true" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem LoadOnDemand="True" Text="User Info" BindingContext="form">
                <Template>
                    <px:PXFormView ID="frmLogin" runat="server" DataMember="User" SkinID="Transparent" MarkRequired="Dynamic">
                        <Template>
                            <px:PXLayoutRule runat="server" ControlSize="XM" LabelsWidth="SM" StartColumn="True" />
                            <px:PXDropDown ID="edState" runat="server" DataField="State" Enabled="False" />
                            <px:PXSelector ID="edLoginType" runat="server" DataField="LoginTypeID" CommitChanges="True" AllowEdit="True" AutoRefresh="True" />
                            <px:PXMaskEdit ID="edUsername" runat="server" DataField="Username" CommitChanges="True" />
                            <px:PXTextEdit ID="edPassword" runat="server" DataField="Password" TextMode="Password" />
                            <px:PXCheckBox ID="edGenerate" runat="server" DataField="GeneratePassword" CommitChanges="True" />
                            <px:PXButton ID="btnResetPassword" runat="server" Text="Reset Password" CommandName="ResetPassword" CommandSourceID="ds" Width="150" Height="20" />
                            <px:PXLayoutRule ID="PXLayoutRule2" runat="server" ControlSize="SM" StartColumn="True" SuppressLabel="True" />
                            <px:PXButton ID="btnActivateLogin" runat="server" CommandName="ActivateLogin" CommandSourceID="ds" Width="150" Height="20" />
                            <px:PXButton ID="btnEnableLogin" runat="server" CommandName="EnableLogin" CommandSourceID="ds" Width="150" Height="20" />
                            <px:PXButton ID="btnDisableLogin" runat="server" CommandName="DisableLogin" CommandSourceID="ds" Width="150" Height="20" />
                            <px:PXButton ID="btnUnlockLogin" runat="server" CommandName="UnlockLogin" CommandSourceID="ds" Width="150" Height="20" />
                        </Template>
                    </px:PXFormView>
                    <px:PXGrid ID="gridRoles" runat="server" DataSourceID="ds" Width="100%" ActionsPosition="Top" SkinID="DetailsInTab" Caption=" ">
                        <ActionBar>
                            <Actions>
                                <Save Enabled="False" />
                                <AddNew Enabled="False" />
                                <Delete Enabled="False" />
                            </Actions>
                        </ActionBar>
                        <Levels>
                            <px:PXGridLevel DataMember="Roles">
                                <Columns>
                                    <px:PXGridColumn AllowMove="False" AllowSort="False" DataField="Selected" TextAlign="Center" Type="CheckBox" AutoCallBack="True" />
                                    <px:PXGridColumn DataField="Rolename" AllowUpdate="False" />
                                    <px:PXGridColumn AllowUpdate="False" DataField="Rolename_Roles_descr" />
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True" MinHeight="250" MinWidth="300" />
                    </px:PXGrid>
                    <px:PXSmartPanel ID="pnlResetPassword" runat="server" Caption="Change password" LoadOnDemand="True" Key="User" CommandName="ResetPasswordOK" CommandSourceID="ds"
                        AcceptButtonID="btnOk" CancelButtonID="btnCancel" AutoCallBack-Command="Refresh" AutoCallBack-Target="frmResetParams" AutoCallBack-Enabled="true" AutoReload="True">
                        <px:PXFormView ID="frmResetParams" runat="server" DataSourceID="ds" Width="100%" DataMember="User" Caption="Reset Password" SkinID="Transparent">
                            <Template>
                                <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartColumn="True" ControlSize="M" LabelsWidth="SM" />
                                <px:PXTextEdit ID="edNewPassword" runat="server" DataField="NewPassword" TextMode="Password" Required="True" />
                                <px:PXTextEdit ID="edConfirmPassword" runat="server" DataField="ConfirmPassword" TextMode="Password" Required="True" />
                            </Template>
                        </px:PXFormView>
                        <px:PXPanel ID="PXPanel1" runat="server" SkinID="Buttons">
                            <px:PXButton ID="btnOk" runat="server" DialogResult="OK" Text="OK" />
                            <px:PXButton ID="btnCancel" runat="server" DialogResult="Cancel" Text="Cancel" />
                        </px:PXPanel>
                    </px:PXSmartPanel>
                </Template>
            </px:PXTabItem>

            <px:PXTabItem Text="Sync Status">
                <Template>
                    <px:PXGrid ID="syncGrid" runat="server" DataSourceID="ds" Height="150px" Width="100%" ActionsPosition="Top" SkinID="Inquire" SyncPosition="true">
                        <Levels>
                            <px:PXGridLevel DataMember="SyncRecs" DataKeyNames="SyncRecordID">
                                <Columns>
                                    <px:PXGridColumn DataField="SYProvider__Name" />
                                    <px:PXGridColumn DataField="RemoteID" CommitChanges="True" LinkCommand="GoToSalesforce" />
                                    <px:PXGridColumn DataField="Status" />
                                    <px:PXGridColumn DataField="Operation" />
                                    <px:PXGridColumn DataField="LastErrorMessage" />
                                    <px:PXGridColumn DataField="LastAttemptTS" DisplayFormat="g" />
                                    <px:PXGridColumn DataField="AttemptCount" />
                                    <px:PXGridColumn DataField="SFEntitySetup__ImportScenario" />
                                    <px:PXGridColumn DataField="SFEntitySetup__ExportScenario" />
                                </Columns>
                                <Layout FormViewHeight="" />
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <CustomItems>
                                <px:PXToolBarButton Key="SyncSalesforce">
                                    <AutoCallBack Command="SyncSalesforce" Target="ds" />
                                </px:PXToolBarButton>
                            </CustomItems>
                        </ActionBar>
                        <Mode InitNewRow="true" />
                        <AutoSize Enabled="True" MinHeight="150" />
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>

        </Items>

        <AutoSize Container="Window" Enabled="True" MinHeight="250" MinWidth="300" />

    </px:PXTab>

</asp:Content>
