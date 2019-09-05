<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ReportViewerMaster.Master" AutoEventWireup="true" CodeBehind="LegacyReportViewer.aspx.cs" Inherits="MyPepsi.Reports.CrystalViewer.LegacyReportViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label Text="Beginning Date" runat="server"/><asp:TextBox ID="BeginingDate" TextMode="Date" runat="server" />
    <asp:Label Text="End Date" runat="server"/><asp:TextBox ID="EndDate" TextMode="Date" runat="server" />
    <asp:Label Text="Warehouse" runat="server"/><asp:DropDownList ID="WarehouseList" runat="server" />
    <asp:Label Text="Customer" runat="server" Visible="false"/><asp:DropDownList ID="CustomerList" runat="server" Visible="false"/>
    <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
    <CR:CrystalReportViewer ID="crViewer" runat="server" AutoDataBind="true" />
</asp:Content>
