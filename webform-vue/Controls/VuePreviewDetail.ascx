<%@ Control %>

<link rel="stylesheet" href="/Content/site-vue.css"/>
<link rel="stylesheet" href="/Content/plugins/v-tooltip.css"/>
<br/>
<br/>

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Vue.js App</div>
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
								<label>Name</label>
								<clear-btn v-bind:value.sync="search.name"></clear-btn>
								<input class="form-control" v-model="search.name"/>
							</div>
							<div class="form-group">
								<label>Category ID</label>
								<clear-btn v-bind:value.sync="search.categoryID" v-bind:default="null"></clear-btn>
								<async-dropdown 
									v-model="search.categoryID"
									friendly-name="Category"
									default-text="All Categories"
									api-url="/api/select/categorySelect"
									api-key="CategoryID">
								</async-dropdown>
							</div>
						</div>

						<div class="col-md-4">
							<div class="form-group">
								<label>Start Date</label>
								<clear-btn v-bind:value.sync="search.startDate" v-bind:default="null"></clear-btn>
								<my-datepicker v-model="search.startDate"></my-datepicker>
							</div>
							
							<div class="form-group">
								<label>End Date</label>
								<clear-btn v-bind:value.sync="search.endDate" v-bind:default="null"></clear-btn>
								<my-datepicker v-model="search.endDate"></my-datepicker>
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
								<th>Date</th>
								<th>ID</th>
								<th>Name</th>
								<th>Categories</th>
								<th>Code</th>
								<th>Active</th>
							</tr>
						</thead>
						<tbody>
							<tr
								class="pointer"
								v-for="item in preview"
								:key="item.PreviewDetailId"
								v-on:click="detailLoad(item.PreviewDetailId)"
								<%--Highlight active row--%>
								v-bind:class="{'info': itemActive(item.PreviewDetailId)}">
	
								<td>{{item.Date | formatDate}}</td> <%--Use custom formatter to display date as standard mm/dd/yyyy--%>
								<td>{{item.PreviewDetailId}}</td>
								<td>{{item.Name}}</td>
								<td>{{item.Categories.join(", ")}}</td>
								<td>{{item.Codes.join(", ")}}</td>
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
								<input class="form-control" v-model="detail.Name"/>
							</div>
							<div class="form-group">
								<label>Description</label>
								<input class="form-control" v-model="detail.Description"/>
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
							<div class="form-group">
								<label>Categories</label>
								<select class="form-control" v-model="detail.CategoryIds" multiple>
									<option v-for="item in categories" :value="item.value">{{item.name}}</option>
								</select>
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
<!--#include file="~/ControlsView/MyDatepicker.html"-->
<!--#include file="~/ControlsView/DualCheckboxes.html"-->
<!--#include file="~/ControlsView/ColoredCheck.html"-->
<!--#include file="~/ControlsView/AsyncDropdown.html"-->

<%--Page-specific resources--%>
<script src="/Controls/VuePreviewDetail.ascx.js"></script>