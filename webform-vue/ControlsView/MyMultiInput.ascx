<%@ Control %>

<div id="my-multi-input-template" class="vue-template">
	<div class="my-multi-input-wrapper"> <%--Component outer--%>
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
</div>
<script src="/ControlsView/MyMultiInput.ascx.js"></script>
