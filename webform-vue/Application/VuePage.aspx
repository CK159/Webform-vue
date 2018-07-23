<%@ Page Language="C#" CodeBehind="VuePage.aspx.cs" Inherits="WebformVue.VuePage" AutoEventWireup="true" %>
<%@ Register TagPrefix="cc" TagName="vueControl" Src="../Controls/VueControl.ascx" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>Vue.js In Webforms</title>
	<link rel="stylesheet" href="/Content/bootstrap.css">
	<link rel="stylesheet" href="/Content/bootstrap-theme.css" data-id="bootstrap-theme-css">
	
	<script type="text/javascript" src="/Scripts/jquery-3.3.1.js"></script>
	<script type="text/javascript" src="/Scripts/bootstrap.js"></script>
</head>
<body>
<form id="form1" runat="server">
	<div class="container">
		<h1>Web Forms ASPX</h1>
		<asp:Label id="Label1" runat="server" Text="Label"></asp:Label>
		<br/>
		<asp:Button id="Button1" runat="server" onclick="Button1_Click" Text="Button"></asp:Button>
		<%--<cc:vueControl runat="server"></cc:vueControl>--%>
		<asp:PlaceHolder runat="server" ID="ControlContainer"></asp:PlaceHolder>
		<asp:label ID="WarningLabel" runat="server" Visible="False" CssClass="text-danger">No / Unknown user control specified!</asp:label>
	</div>
</form>
</body>
</html>