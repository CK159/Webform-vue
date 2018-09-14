<%@ Control %>

<link rel="stylesheet" href="/Content/site-vue.css"/>
<link rel="stylesheet" href="/Content/plugins/v-tooltip.css"/>
<br/>
<br/>

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Manage Products - Entity Framework</div>
		</div>

		<div class="panel-body">
			<preview-detail
				v-bind:search="search"
				v-bind:preview.sync="preview"
				v-bind:detail.sync="detail"
				v-bind:new-detail="newDetail"
				v-bind:pk="pk"
				v-bind:api-url="apiUrl"
				ref="pdRef">

				<div slot="search">
					<div class="row">
						<div class="col-md-4">
							<div class="form-group">
								<label>Product Name</label>
								<clear-btn v-bind:value.sync="search.productName"></clear-btn>
								<input class="form-control" v-model="search.productName"/>
							</div>
							<div class="form-group">
								<label>Description</label>
								<clear-btn v-bind:value.sync="search.productDesc"></clear-btn>
								<input class="form-control" v-model="search.productName"/>
							</div>
						</div>

						<div class="col-md-4">
							<div class="form-group">
								<label>In Catalog</label>
								<clear-btn v-bind:value.sync="search.catalogId" v-bind:default="null"></clear-btn>
								<async-dropdown 
									v-model="search.catalogId"
									friendly-name="Catalog"
									default-text="Any"
									api-url="/api/select/catalogSelect"
									api-key="CatalogId">
								</async-dropdown>
							</div>
						</div>

						<div class="col-md-4">
							<label>Active</label>
							<clear-btn v-bind:value.sync="search.active" v-bind:default="true"></clear-btn>
							<dual-checkboxes v-bind:value.sync="search.active"></dual-checkboxes>
							
							<button type="button" class="btn btn-danger btn-xs pull-right" @click="resetSearch">Clear</button>
							<label>&nbsp;</label>
							<div class="form-group">
								<button type="button" class="btn btn-primary btn-block" @click="refreshSearch">Search</button>
							</div>
						</div>
					</div>
				</div>

				<div slot="preview">
					<table class="table table-hover table-bordered table-striped table-condensed pd-preview-table">
						<thead>
							<tr>
								<th>ID</th>
								<th>Name</th>
								<th>Catalogs</th>
								<th>Active</th>
							</tr>
						</thead>
						<tbody>
							<tr
								class="pointer"
								v-for="item in preview"
								:key="item.ProductId"
								v-on:click="detailLoad(item.ProductId)"
								<%--Highlight active row--%>
								v-bind:class="{'info': itemActive(item.ProductId)}">
	
								<td>{{item.ProductId}}</td> <%--Use custom formatter to display date as standard mm/dd/yyyy--%>
								<td>{{item.ProductName}}</td>
								<td>{{item.Name}}</td>
								<td>{{item.Catalogs.join(", ")}}</td>
								<td><colored-check :value="item.Active"></colored-check></td>
							</tr>
						</tbody>
					</table>
				</div>

				<div slot="detail">
					<div class="row">
						<div class="col-md-6">
							<div class="form-group">
								<label>Name</label>
								<input class="form-control" v-model="detail.ProductName"/>
							</div>
							<div class="form-group">
								<label>Description</label>
								<input class="form-control" v-model="detail.ProductDesc"/>
							</div>
							<div class="form-group">
								<label>Active</label>
								<div class="checkbox">
									<label>
										<input type="checkbox" v-model="detail.Active"/>
										Item Active
									</label>
								</div>
							</div>
						</div>

						<div class="col-md-6">
							<%--<div class="form-group">
								<label>Categories</label>
								<select class="form-control" v-model="detail.CategoryIds" multiple>
									<option v-for="item in categories" :value="item.value">{{item.name}}</option>
								</select>
							</div>--%>
							<div class="form-group">
								<label>Date Created</label>
								<input class="form-control" v-model="detail.DateCreated" disabled/>
							</div>
						</div>
					</div>
				</div>
			</preview-detail>
		</div>
	</div>
</div>

<%--Dependencies--%>
<script src="/Content/vue/vue.js"></script>
<script src="/Content/utilities.js"></script>
<script src="/Content/plugins/vuejs-datepicker.js"></script>
<script src="/Content/plugins/v-tooltip.min.js"></script>
<!--#include file="~/ControlsView/PreviewDetail.html"-->
<!--#include file="~/ControlsView/ClearBtn.html"-->
<!--#include file="~/ControlsView/DualCheckboxes.html"-->
<!--#include file="~/ControlsView/ColoredCheck.html"-->
<!--#include file="~/ControlsView/AsyncDropdown.html"-->

<%--Page-specific resources--%>
<script src="/Controls/ef/Product.ascx.js"></script>