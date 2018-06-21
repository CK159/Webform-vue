<%@ Control Language="C#" CodeBehind="VueControl.aspx.cs" Inherits="WebformVue.VueControl" AutoEventWireup="true" %>

<script src="/Scripts/vue.js"></script>

<h2>Vue.js ASCX custom control</h2>
<asp:Button id="Button1" runat="server" onclick="Button1_Click" Text="Cause Postback"></asp:Button>
<asp:Label id="Label1" runat="server" Visible="false"></asp:Label>
