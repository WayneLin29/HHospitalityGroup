<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM407000.aspx.cs" Inherits="Page_LM407000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <px:PXDataSource ID="ds" Width="100%" Visible="True" runat="server" TypeName="LeaveAndOvertimeCustomization.Graph.EmployeeHolidaySchedule" PrimaryView="Document" PageLoadBehavior="PopulateSavedValues">
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="phF" runat="Server">
    <script src="../../LumCalendar/js/main.js" type="text/javascript"></script>
    <link href='../../LumCalendar/css/main.css' rel='stylesheet' />

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                timeZone: 'local',
                events: <%= holidayJson %>,
                eventTimeFormat: { // like '14:30:00'
                    hour: '2-digit',
                    minute: '2-digit',
                    meridiem: false,
                    hour12: false
                }
            });
            calendar.render();
        });
    </script>
    <div id='calendar'></div>

    <style>
        .fc-event-title {
            white-space: normal;
        }
    </style>

</asp:Content>

