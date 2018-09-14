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
			apiUrl: "/ApiThingyController.cs/PreviewDetail/",
			categories: []
		}
	},
	methods: {
		itemActive: function (previewId) {
			if (!this.$refs.hasOwnProperty("pdRef")) {
				return false;
			}

			return this.$refs["pdRef"].itemActive(previewId, this.detail[this.pk]);
		},
		detailLoad: function (id) {
			this.$refs['pdRef'].detailLoad(id);
		},
		refreshSearch: function () {
			this.$refs["pdRef"].refreshSearch();
		},
		resetSearch: function () {
			this.componentDataReset("search");
			this.refreshSearch();	
		},
	},
	created: function () {
		var vm = this;

		this.api({
			action: "/api/select/categorySelect",
			done: function (data) {
				vm.categories = data;
			}
		});
	}
});