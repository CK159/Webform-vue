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
								@click="detailLoad(item.ProductId)"
								<%--Highlight active row--%>
								:class="{'info': itemActive(item.ProductId)}">
	
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
							<div class="form-group">
								<label>Product Type</label>
								<async-dropdown 
									v-model="detail.ProductTypeId"
									friendly-name="Product Type"
									api-url="/api/select/productTypeSelect"
									api-key="ProductTypeId">
								</async-dropdown>
							</div>
						</div>

						<div class="col-md-6">
							<table class="table table-hover table-bordered table-striped table-condensed">
								<thead>
									<tr>
										<th>Catalog</th>
										<th>Store</th>
									</tr>
								</thead>
								<tbody>
									<tr v-for="item in detail.Catalogs" :key="item.CatalogId">
										<td>
											<div class="checkbox no-margin" :class="{'text-muted': !item.Active}">
												<label>
													<input type="checkbox" v-model="item.Assigned"/>
													{{item.Active ? "" : "(inactive)"}}
													{{item.CatalogName}}
												</label>
											</div>
										</td>
										<td>{{item.StoreName}}</td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
					
					<h3>Product Image Resources ({{detail.Resources ? detail.Resources.length : 0}})</h3>
					<div class="form-group">
						Add Resources <input type="file" multiple @change="fileAdded"/>
					</div>
					
					<div class="row">
						<div class="col-lg-3 col-md-4 col-sm-6"
						     v-for="item in detail.Resources"
						     :key="item.ProductTypeId">
							<img class="img-responsive" :src="smartSrc(item.File)" />
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