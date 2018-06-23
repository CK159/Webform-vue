<%@ Control Language="C#" CodeBehind="VueControl.aspx.cs" Inherits="WebformVue.VueControl" AutoEventWireup="true" %>

<h2>Vue.js ASCX custom control</h2>
<asp:Button id="Button1" runat="server" onclick="Button1_Click" Text="Cause Postback"></asp:Button>
<asp:Label id="Label1" runat="server" Visible="false"></asp:Label>

<div>&nbsp;</div>

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Vue.js App</div>
		</div>
		
		<div class="panel-body">
			<div class="row">
				<%--Left panel--%>
				<div class="col-md-6">
					<h2>Examples</h2><hr />
					
					<label>Checkbox Emulator</label>
					<%--Basic custom component with a title and checked property that we can bind.--%>
					<%--The .sync allows the custom component to send updates for that property back--%>
					<%--Without .sync, its a one-way binding and the component can't update data its provided--%>
					<my-checkbox
						v-bind:title="simpleMessage"
						v-bind:checked.sync="simpleChecked">
					</my-checkbox>
					<br />
					
					<label>Live Text Update</label>
					<%--Changes to text box auto-updates simpleMessage and everything that binds to it--%>
					<input class="form-control" v-model="simpleMessage" maxlength="30" />
					<p class="help-block">
						<%--Live display of characters left and turns red when less than 10 characters left--%>
						<span v-bind:class="{'text-danger': simpleLengthLeft < 10}">
							{{simpleLengthLeft}} characters remaining.
						</span>
					</p>
					
				</div>
				
				<%--Right panel--%>
				<div class="col-md-6">
					<h2>More Examples</h2><hr />
					
				</div>
			</div>
			
			<%--Middle section--%>
			<h2>Remaining Examples</h2><hr />
			
			<h2>Neverending Examples....</h2><hr />
			
		</div>
	</div>
</div>

<script type="text/x-template" id="checkbox-template">
	<div class="checkbox-wrapper" @click="check">
		<span :class="{ checkbox: true, checked: checked }"></span>
		<span v-if="checked">&#10004;</span>
		<span v-if="!checked">&#10008;</span>
		<span class="title">{{title}}</span>
	</div>
</script>

<script src="/Scripts/vue.js"></script>
<script src="/Controls/VueControl.ascx.js"></script>