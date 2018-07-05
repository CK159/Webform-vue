<%@ Control %>

<div id="preview-detail-template" class="vue-template">
	<div class="preview-detail-container"> <%--Single root element--%>
		<%--Preview display or table--%>
		<div style="position: relative;">
			<slot name="preview">Preview Area</slot>
			
			<div class="preview-container">
				<div class="jumbotron" v-if="previewState == 'loading'" style="position: absolute; left: 0; right: 0;">
					<h1 class="text-center">IT'S LOADING!!!!!</h1>
				</div>
			</div>
		</div>
		
		<%--Action buttons--%>
		<div class="detail-actions">
			<p>
				<button type="button" class="btn btn-danger pull-right" v-on:click="detailDelete">Delete</button>
				<button type="button" class="btn btn-info" v-on:click="detailNew">New</button>
			</p>
			<div class="row">
				<div class="col-lg-10 col-md-9 col-sm-8">
					<button type="button" class="btn btn-success btn-block" v-on:click="detailSave">Save</button>
				</div>
				<div class="col-lg-2 col-md-3 col-sm-4">
					<button type="button" class="btn btn-danger btn-block" v-on:click="detailCancel">Cancel</button>
				</div>
			</div>
		</div>
		
		<div 
			class="detail-message alert"
			v-if="messageType != 'none'"
			v-bind:class="{'alert-success': messageType == 'success', 'alert-danger': messageType == 'error'}">
			
			{{messageText}}
		</div>
		
		<%--Debugging / visualization--%>
		<p class="text-muted">Preview State: {{previewState}} Detail State: {{detailState}}</p>
		
		<%--Details display--%>
		<div class="detail-container" style="position: relative;">
			<div v-if="detailState != 'unloaded'">
				<slot name="detail">Detail Area</slot>
			</div>
			
			<div class="jumbotron" v-if="detailState == 'loading'" style="position: absolute; left: 0; right: 0;">
				<h1 class="text-center">IT'S LOADING!!!!!</h1>
			</div>
		</div>
	</div>
</div>
<script src="/ControlsView/PreviewDetail.ascx.js"></script>