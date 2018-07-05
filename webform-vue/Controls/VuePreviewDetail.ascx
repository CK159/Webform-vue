<%@ Control %>

<link rel="stylesheet" href="/Content/site-vue.css"/>
<br />
<br />

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Vue.js App</div>
		</div>
				
		<div class="panel-body">
			<preview-detail
				v-bind:preview.sync="preview"
				v-bind:detail.sync="detail"
				v-bind:new-detail="newDetail"
				v-bind:api-actions="apiActions"
				ref="pdRef">
				
				<div slot="preview">
					<table class="table table-hover table-bordered table-striped table-condensed">
						<thead>
							<tr>
								<th>Date</th>
								<th>ID</th>
								<th>Name</th>
								<th>Category</th>
								<th>Code</th>
								<th>Value</th>
								<th>Status</th>
								<th>Active</th>
							</tr>
						</thead>
						<tbody>
							<tr 
								class="pointer"
								v-for="item in preview"
								:key="item.id"
								v-on:click="$refs['pdRef'].detailLoad(item.id)"
								<%--Highlight active row--%>
								v-bind:class="{'info': itemActive(item.id)}"> <%--TODO: Refs issue--%>
								
								<td>{{item.date | formatDate}}</td> <%--Use custom formatter to display date as standard mm/dd/yyyy--%>
								<td>{{item.id}}</td>
								<td>{{item.name}}</td>
								<td>{{item.category}}</td>
								<td>{{item.code}}</td>
								<td>{{item.value}}</td>
								<td>{{item.status}}</td>
								<td>
									<span class="text-success" v-if="item.active">&#10004;</span>
									<span class="text-danger" v-if="!item.active">&#10008;</span>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div slot="detail">
					<div class="row">
						<div class="col-md-6">
							<div class="form-group">
								<label>Name</label>
								<input class="form-control" v-model="detail.name" />
								<label>Description</label>
								<input class="form-control" v-model="detail.description" />
								<label>Status</label>
								<input class="form-control" v-model="detail.status" /> <%--TODO: searchable dropdown--%>
							</div>
						</div>
						
						<div class="col-md-6">
							<div class="form-group">
								<label>Active</label>
								<div class="checkbox">
									<label>
										<input type="checkbox" v-model="detail.active" />
										Item Active
									</label>
								</div>
								<label>Date Created</label>
								<input class="form-control" v-bind="detail.date" disabled /> <%--TODO: Needs | formatDate--%>
							</div>
						</div>
					</div>
				</div>
			</preview-detail>
		</div>
	</div>
</div>

<script src="/Scripts/vue.js"></script>
<script src="/Scripts/utilities.js"></script>

<%@ Register TagPrefix="uc" TagName="PreviewDetail" Src="~/ControlsView/PreviewDetail.ascx" %>
<uc:PreviewDetail runat="server"></uc:PreviewDetail>

<script src="/Controls/VuePreviewDetail.ascx.js"></script>