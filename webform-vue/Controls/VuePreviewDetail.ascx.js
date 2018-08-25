var vueApp = new Vue({
	el: "#vueContainer",
	data: function () {
		return {
			search: {
				name: null, //string
				categoryID: null, //int
				active: true, //null, true, false
				startDate: null,
				endDate: null
			},
			isActive: true,
			isInactive: false,
			preview: [],
			detail: {},
			//Default values used when user creates new record TODO: easier way? Lazy-load from server?
			newDetail: {
				"PreviewDetailId": -1,
				"Name": "",
				"Description": "",
				"Active": true,
				"Date": "0001-01-01T00:00:00",
				"CategoryIds": [],
				"CodeIds": []
			},
			pk: "PreviewDetailId",
			apiEndpoints: {
				baseURL: "/ApiThingyController.cs/PreviewDetail/",
				previewLoad: "Preview",
				detailLoad: "Load",
				detailSave: "Save",
				detailDelete: "Delete",
			},
			categories: []
		}
	},
	methods: {
		itemActive: function (previewid) {
			if (!this.$refs.hasOwnProperty("pdRef")) {
				return false;
			}

			return this.$refs["pdRef"].itemActive(previewid, this.detail.PreviewDetailId);
		},
		detailLoad: function (id) {
			this.$refs['pdRef'].detailLoad(id);
		},
		refreshSearch: function () {
			this.$refs["pdRef"].refreshSearch();
		},
		resetSearch: function () {
			//https://stackoverflow.com/a/43643407
			Object.assign(this.$data.search, this.$options.data.call(this).search);
			this.refreshSearch();
		},
	},
	watch: {
		isActive: function(newVal) {
			this.search.active = newVal === this.isInactive ? null : newVal;
		},
		isInactive: function(newVal) {
			this.search.active = newVal === this.isActive ? null : !newVal;
		},
		"search.active": function(newVal) {
			if (newVal !== null) {
				//active is true or false, so both checkboxes have a defined state to be in
				this.isActive = newVal === true;
				this.isInactive = newVal === false;
			}
			else if (newVal === null && this.isActive !== this.isInactive) {
				//Only one checkbox is checked but both should be in the same state - default to unchecked
				this.isActive = false;
				this.isInactive = false;
			}
		}
	},
	created: function () {
		var vm = this;

		this.api({
			action: "/ApiThingyController.cs/Category/GetSelect",
			done: function (data) {
				vm.categories = data;
			}
		});
	}
});