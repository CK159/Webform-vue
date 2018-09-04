<%@ Control %>

<link rel="stylesheet" href="/Content/site-vue.css"/>
<link rel="stylesheet" href="/Content/plugins/v-tooltip.css"/>
<br/>
<br/>

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Async select dropdowns</div>
		</div>

		<div class="panel-body">
			<h2>
				<button type="button" class="btn btn-danger btn-xs pull-right" @click="modal.showCodeDelete = true">
					- Code
				</button>
				Code
				
				<v-popover class="inline-block" @apply-show="openCreateCode">
					<button type="button" class="btn btn-primary btn-xs">+ Code</button>

					<template slot="popover">
						<div class="form-group">
							<label>New Code Name</label>
							<input class="form-control" v-model="newCodeName" ref="newCodeName" />
						</div>

						<div class="text-right">
							<button type="button" class="btn btn-default" v-close-popover>Close</button>
							<button type="button" class="btn btn-success" v-close-popover @click="newCodeClick">
								Create Code
							</button>
						</div>
					</template>
				</v-popover>
			</h2>
			
			<div class="form-group" v-if="!isNewCode">
				<async-dropdown 
					v-model="codeId"
					friendly-name="Code"
					api-url="/api/select/codeSelect"
					api-key="codeId">
					<%--apiData=""--%>
				</async-dropdown>
			</div>
			<div class="form-group" v-if="isNewCode">
				<label>New Code: <span class="text-info">{{codeName}}</span></label>
			</div>
			
			<h2>
				Code Attributes
				<button type="button" class="btn btn-primary btn-xs" @click="modal.showCodeAttributeAdd = true">
					+ Code Attribute
				</button>
			</h2>
			<div class="row">
				<div 
					class="col-sm-6"
					v-for="codeAttr in codeAttributes"
					:key="codeAttr.CodeAttributeId">
					
					<div class="panel panel-default">
						<div class="panel-heading">
							<div class="panel-title">
								<button type="button" class="btn btn-danger btn-xs pull-right" @click="removeCodeAttrClick(codeAttr.CodeAttributeId)">
									- Code Attribute
								</button>
								<h3 class="no-margin">
									{{codeAttr.AttributeName}}
									<small class="text-muted">Id: {{codeAttr.CodeAttributeId}}</small>
								</h3>
							</div>
						</div>
		
						<div class="panel-body">
							<p>
								Code Attribute Values ({{safeCodeAttrVal(codeAttr.CodeAttributeId).length}})
								<button type="button" class="btn btn-primary btn-xs" 
								        @click="addCodeAttrValClick(codeAttr.CodeAttributeId, codeAttr.AttributeId)">
									+ Code Attribute Value
								</button>
							</p>
							
							<div
								v-for="cav in safeCodeAttrVal(codeAttr.CodeAttributeId)"
								:key="cav.CodeAttributeValueId">
						
								<div class="form-group">
									<button type="button" class="btn btn-danger btn-xs pull-right"
									        @click="removeCodeAttrValClick(codeAttr.CodeAttributeId, cav.CodeAttributeValueId)">
										X
									</button>
									<span class="text-muted">
										CodeAttributeValueId: {{cav.CodeAttributeValueId}}
										AttributeValueId: {{cav.AttributeValueId}}
									</span>
									<async-dropdown 
										v-model="cav.AttributeValueId"
										friendly-name="Attribute Value"
										api-url="/api/select/attributeValueSelect"
										api-key="AttributeValueId"
										:api-data="{AttributeId: cav.AttributeId}">
									</async-dropdown>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div> 
		</div>
	</div>
	
	<modal v-if="modal.showCodeDelete" v-on:close="modal.showCodeDelete = false">
		<h3 slot="header">Confirm Delete Code</h3>
		<div slot="body">
			Are you sure you want to delete the current code?
		</div>
		<span slot="footer">
			<button type="button" class="btn btn-danger" @click="removeCodeClick">Delete Code</button>
		</span>
	</modal>
	
	<modal v-if="modal.showCodeAttributeAdd" v-on:close="modal.showCodeAttributeAdd = false">
		<h3 slot="header">Add Code Attribute</h3>
		<div slot="body">
			<div class="form-group">
				<async-dropdown 
					v-model="modal.addAttributeId"
					friendly-name="Attribute"
					api-url="/api/select/attributeSelect"
					api-key="AttributeId">
				</async-dropdown>
			</div>
		</div>
		<span slot="footer">
			<button type="button" class="btn btn-primary" @click="addAttrClick(modal.addAttributeId)">Add Attribute</button>
		</span>
	</modal>
</div>

<%--Dependencies--%>
<script src="/Content/vue/vue.js"></script>
<script src="/Content/utilities.js"></script>
<script src="/Content/plugins/v-tooltip.min.js"></script>
<!--#include file="~/ControlsView/AsyncDropdown.html"-->
<!--#include file="~/ControlsView/modal.html"-->

<%--Page-specific resources--%>
<script src="/Controls/VueAsyncDropdown.ascx.js"></script>