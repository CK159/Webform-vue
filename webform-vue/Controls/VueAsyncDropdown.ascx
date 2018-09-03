<%@ Control %>

<link rel="stylesheet" href="/Content/site-vue.css"/>
<br/>
<br/>

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Async select dropdowns</div>
		</div>

		<div class="panel-body">
			<h2>
				<button type="button" class="btn btn-danger btn-xs pull-right">- Code</button>
				Code
				<button type="button" class="btn btn-primary btn-xs">+ Code</button>
			</h2>
			
			<div class="form-group">
				<async-dropdown 
					v-model="codeId"
					friendly-name="Code"
					api-url="/api/select/codeSelect"
					api-key="codeId">
					<%--apiData=""--%>
				</async-dropdown>
			</div>
			
			<h2>
				Code Attributes
				<button type="button" class="btn btn-primary btn-xs">+ Code Attribute</button>
			</h2>
			<div class="row">
				<div 
					class="col-sm-6"
					v-for="codeAttr in codeAttributes"
					:key="codeAttr.CodeAttributeId">
					
					<div class="panel panel-default">
						<div class="panel-heading">
							<div class="panel-title">
								<button type="button" class="btn btn-danger btn-xs pull-right">- Code Attribute</button>
								<h3 class="no-margin">
									{{codeAttr.AttributeName}}
									<small class="text-muted">Id: {{codeAttr.CodeAttributeId}}</small>
								</h3>
							</div>
						</div>
		
						<div class="panel-body">
							<p>
								Code Attribute Values ({{safeCodeAttrVal(codeAttr.CodeAttributeId).length}})
								<button type="button" class="btn btn-primary btn-xs">+ Code Attribute Value</button>
							</p>
							
							<div
								v-for="cav in safeCodeAttrVal(codeAttr.CodeAttributeId)"
								:key="cav.CodeAttributeValueId">
						
								<div class="form-group">
									<button type="button" class="btn btn-danger btn-xs pull-right">X</button>
									<span class="text-muted">
										CodeAttributeValueId: {{cav.CodeAttributeValueId}}
										AttributeValueId: {{cav.AttributeValueId}}
									</span>
									<async-dropdown 
										v-model="cav.AttributeValueId"
										friendly-name="Attribute Value"
										api-url="/api/select/attributeValueSelect"
										api-key="AttributeValueId"
										:apiData="{AttributeId: cav.AttributeId}">
									</async-dropdown>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div> 
		</div>
	</div>
</div>

<%--Dependencies--%>
<script src="/Content/vue/vue.js"></script>
<script src="/Content/utilities.js"></script>
<!--#include file="~/ControlsView/AsyncDropdown.html"-->

<%--Page-specific resources--%>
<script src="/Controls/VueAsyncDropdown.ascx.js"></script>