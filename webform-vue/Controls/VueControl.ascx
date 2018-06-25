<%@ Control Language="C#" CodeBehind="VueControl.aspx.cs" Inherits="WebformVue.VueControl" AutoEventWireup="true" %>

<h2>Web Forms ASCX custom control</h2>
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
				<%--Upper Left section--%>
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
					
					<label>
						5-bit
						<a href="https://en.wikipedia.org/wiki/Two%27s_complement" target="_blank">2's Complement</a>
						Binary to Int. Result: {{simple2sComplement}}
					</label>
					<div class="form-group form-inline">
						<div class="checkbox"><input type="checkbox" v-model="simpleCheckboxes" value="16"></div>
						<div class="checkbox"><input type="checkbox" v-model="simpleCheckboxes" value="8"></div>
						<div class="checkbox"><input type="checkbox" v-model="simpleCheckboxes" value="4"></div>
						<div class="checkbox"><input type="checkbox" v-model="simpleCheckboxes" value="2"></div>
						<div class="checkbox"><input type="checkbox" v-model="simpleCheckboxes" value="1"></div>
					</div>
				</div>
				
				<%--Upper Right section--%>
				<div class="col-md-6">
					<h2>More Examples</h2><hr />
					
					<div class="form-group">
						<label>Input Type</label>
						<select class="form-control" v-model="multiInputSelect">
							<option value="1">input type=text</option>
							<option value="2">textarea</option>
							<option value="3">select</option>
							<option value="4">Buttons</option>
							<option value="5">Mildly Inconvenient Text Box</option>
						</select>
					</div>
					<p>
						<button type="button" class="btn btn-primary" v-on:click="multiInputAdd">Add Item</button>
						<button type="button" class="btn btn-danger" v-on:click="multiInputRandomize">Randomize Input Types</button>
					</p>
					
					<label>List of fields</label>
					<%--List binding (v-for) - creates markup/vue components for every element in multiInputData array--%>
					<div class="form-group" 
						v-for="(item, index) in multiInputData"
						<%--You should (and sometimes must) set the :key to something that is unique to every array element--%>
						<%--in order to help Vue.js keep track of items durring changes, sorts, re-renders--%>
						:key="item.id">
						
						<div class="pull-right text-muted">
							ID: {{item.id}} Input Type: {{item.type}}
							<%--This button will call our remove method.--%>
							<%--That will delete this item from the multiInputData array.--%>
							<%--Vue.js will update the UI accordingly.--%>
							<button type="button" class="btn btn-xs btn-default" v-on:click="multiInputRemove(index)">X</button>
						</div>
						<span class="text-muted">{{multiInputNames[item.type]}}</span>
						<my-input
							v-bind:type="item.type"
							v-bind:value.sync="item.value">
						</my-input>
					</div>
				</div>
			</div>
			
			<%--Middle section--%>
			<h2>Remaining Examples</h2><hr />
			
			<%--Preview Table--%>
			<div class="table-responsive" style="position: relative;">
				<%--TODO: Nicer loading bar--%>
				<div class="jumbotron" v-if="recordPreviewState == 'loading'" style="position: absolute; left: 0; right: 0;">
					<h1 class="text-center">IT'S LOADING!!!!!</h1>
				</div>
				<table class="table table-hover table-bordered table-striped table-condensed">
					<thead>
						<tr>
							<th>ID</th>
							<th>Name</th>
							<th>Active</th>
							<th>Date Created</th>
						</tr>
					</thead>
					<tbody>
						<tr 
							v-for="item in recordPreview"
							:key="item.id"
							v-on:click="recordLoad(item.id)"
							v-bind:class="{'info': item.id == recordDetail.id}"> <%--Highlight active row--%>
							
							<td>{{item.id}}</td>
							<td>{{item.name}}</td>
							<td>
								<span class="text-success" v-if="item.active">&#10004;</span>
								<span class="text-danger" v-if="!item.active">&#10008;</span>
							</td>
							<td>{{item.date}}</td>
						</tr>
					</tbody>
				</table>
				
				<%--<h1 v-if="recordPreviewState == 'loading'" style="position: absolute; background-color: white;">IT'S LOADING!!!!!</h1>--%>
			</div>
				
			<%--Action buttons--%>
			<p class="text-right">
				<button type="button" class="btn btn-info" v-on:click="recordNew">New</button>
				<button type="button" class="btn btn-danger" v-on:click="recordDelete">Delete</button>
			</p>
			<div class="row">
				<div class="col-lg-10 col-md-9 col-sm-8">
					<button type="button" class="btn btn-success btn-block" v-on:click="recordSave">Save</button>
				</div>
				<div class="col-lg-2 col-md-3 col-sm-4">
					<button type="button" class="btn btn-danger btn-block" v-on:click="recordCancel">Cancel</button>
				</div>
			</div>
			
			<p class="text-muted">State: {{recordState}}</p> <%--Debugging / visualization--%>
			
			<%--Details Section--%>
			<div class="row" v-if="recordState != 'unloaded'" style="position: relative;">
				<div class="col-md-6">
					<p class="text-muted">ID: {{recordDetail.id}}</p>
					<div class="form-group">
						<label>Name</label>
						<input class="form-control" v-model="recordDetail.name" />
					</div>
					<div class="form-group">
						<label>Description</label>
						<textarea class="form-control" v-model="recordDetail.description"></textarea>
					</div>
					<div class="form-group">
						<label>Date</label>
						<input class="form-control" v-model="recordDetail.date" /> <%--TODO: Datepicker options?--%>
					</div>
				</div>
				
				<div class="col-md-6">
					<div class="form-group">
						<div class="checkbox">
							<label>
								<input type="checkbox" v-model="recordDetail.active"> Active
							</label>
						</div>
					</div>
					
					<span class="text-muted">TODO:</span> <label>Array of sub-items</label>
				</div>
				
				<%--TODO: Nicer loading bar--%>
				<div class="jumbotron" v-if="recordState == 'loading'" style="position: absolute; left: 0; right: 0;">
					<h1 class="text-center">IT'S LOADING!!!!!</h1>
				</div>
			</div>
			
			<%--Bottom section--%>
			<h2>Neverending Examples...</h2><hr />
			<div class="row">
				<div class="col-lg-4 col-md-6">
					<div class="form-group">
						<my-theme-changer></my-theme-changer>
					</div>
				</div>
				<div class="col-lg-4 col-md-6">
					<label>AJAX Artificial Delay</label>
					<div class="form-group input-group">
						<input class="form-control" v-model="artificialDelay" />
						<span class="input-group-addon">ms.</span>
					</div>
				</div>
				<div class="col-lg-4 col-md-6">
					<label>AJAX Endpoint</label>
					<div class="form-group">
						<label class="checkbox-inline"><input type="checkbox" value="asmx" v-model="endpoint" disabled>ApiThingy.asmx</label>
						<label class="checkbox-inline"><input type="checkbox" value="mvc" v-model="endpoint" disabled>ApiThingyController</label>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>

<script type="text/x-template" id="my-checkbox-template">
	<div class="my-checkbox-wrapper" v-on:click="check">
		<span :class="{ checkbox: true, checked: checked }"></span>
		<span class="text-success" v-if="checked">&#10004;</span>
		<span class="text-danger" v-if="!checked">&#10008;</span>
		<span class="title">{{title}}</span>
	</div>
</script>

<script type="text/x-template" id="my-input-template">
	<div class="my-input-wrapper"> <%--Component outer--%>
		<div v-if="type == 1"> <%--Specific input types--%>
			<input class="form-control" v-model="intVal" />
		</div>
		<div v-if="type == 2">
			<textarea class="form-control" v-model="intVal">b</textarea>
		</div>
		<div v-if="type == 3">
			<select class="form-control" v-model="intValQ">
				<option>a</option>
				<option>b</option>
				<option>c</option>
				<option>1</option>
				<option>2</option>
				<option>3</option>
				<option>?</option>
			</select>
		</div>
		<div v-if="type == 4">
			<%--This really should be its own component, rendered with v-for to remove duplicaiton--%>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == 'a', 'btn-default': intValQ != 'a'}" v-on:click="intValQ = 'a'">a</button>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == 'b', 'btn-default': intValQ != 'b'}" v-on:click="intValQ = 'b'">b</button>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == 'c', 'btn-default': intValQ != 'c'}" v-on:click="intValQ = 'c'">c</button>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == '1', 'btn-default': intValQ != '1'}" v-on:click="intValQ = '1'">1</button>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == '2', 'btn-default': intValQ != '2'}" v-on:click="intValQ = '2'">2</button>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == '3', 'btn-default': intValQ != '3'}" v-on:click="intValQ = '3'">3</button>
			<button type="button" class="btn" v-bind:class="{'btn-success': intValQ == '?', 'btn-default': intValQ != '?'}" v-on:click="intValQ = '?'">?</button>
		</div>
		<div v-if="type == 5">
			<label>Text input with extra steps!</label>
			<input class="form-control" v-model="intVal" v-on:click="showModal = true" />
		</div>
		<modal v-if="showModal" v-on:close="showModal = false">
			<div slot="body">
				<label>Enter some text...</label>
				<input class="form-control" v-model="intVal" />
			</div>
			<h3 slot="header">Edit Text Box Text...In A Pop-up</h3>
		</modal>
	</div>
</script>

<!-- template for the modal component -->
<!-- https://vuejs.org/v2/examples/modal.html -->
<script type="text/x-template" id="modal-template">
	<transition name="modal">
		<div class="modal-mask">
			<div class="modal-wrapper">
				<div class="modal-container">

					<div class="modal-header">
						<slot name="header">Alert</slot>
					</div>

					<div class="modal-body">
						<slot name="body"></slot>
					</div>

					<div class="modal-footer">
						<button type="button" class="btn btn-default" @click="$emit('close')">
							Close
						</button>
						<slot name="footer"></slot>
					</div>
				</div>
			</div>
		</div>
	</transition>
</script>

<script src="/Scripts/misc.js"></script>
<script src="/Scripts/vue.js"></script>
<script src="/Scripts/themeChanger.js"></script>
<script src="/Controls/VueControl.ascx.js"></script>
<link rel="stylesheet" href="/Content/VueModal.css" />