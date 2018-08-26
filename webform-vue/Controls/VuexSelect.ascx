<%@ Control %>

<link rel="stylesheet" href="/Content/site-vue.css"/>
<br/>
<br/>

<div id="vueContainer">
	<div class="panel panel-primary">
		<div class="panel-heading">
			<div class="panel-title">Smart Vuex-backed select dropdowns</div>
		</div>

		<div class="panel-body">
			<h2>
				<button type="button" class="btn btn-danger btn-sm pull-right">- Code</button>
				Code
				<button type="button" class="btn btn-primary btn-sm">+ Code</button>
			</h2>
			
			<div class="form-group">
				<select class="form-control">
					<option>code a</option>
					<option>code b</option>
					<option>code c</option>
				</select>
			</div>
			
			<h2>
				Code Attributes
				<button type="button" class="btn btn-primary btn-sm">+ Code Attribute</button>
			</h2>
			<div>
				<div class="row">
					<div class="col-sm-6">
						<h3>Attr 1</h3>
						<div class="form-group">
							<select class="form-control">
								<option>attr1</option>
								<option>attr2</option>
								<option>attr3</option>
							</select>
						</div>
					</div>
					<div class="col-sm-6">
						<h3>
							Attr 1 Values
							<button type="button" class="btn btn-primary btn-sm">+ Code Attribute Value</button>
						</h3>
						
						<div class="form-group">
							<select class="form-control">
								<option>val i</option>
								<option>val ii</option>
								<option>val iii</option>
								<option>val iv</option>
								<option>val v</option>
							</select>
						</div>
						<div class="form-group">
							<select class="form-control">
								<option>val i</option>
								<option>val ii</option>
								<option>val iii</option>
								<option>val iv</option>
								<option>val v</option>
							</select>
						</div>
						<div class="form-group">
							<select class="form-control">
								<option>val i</option>
								<option>val ii</option>
								<option>val iii</option>
								<option>val iv</option>
								<option>val v</option>
							</select>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>

<%--Dependencies--%>
<script src="/Scripts/vue.js"></script>
<script src="/Scripts/vuex.js"></script>
<script src="/Scripts/utilities.js"></script>

<%--Page-specific resources--%>
<script src="/Controls/VuexSelect.ascx.js"></script>