var vueApp = new Vue({
	el: "#vueContainer",
	data: function () {
		return {
			preview: [],
			detail: {},
			//Default values used when user creates new record TODO: easier way? Lazy-load from server?
			newDetail: {
				"PreviewDetailId": -1,
				"Name": "",
				"Description": "",
				"Active": false,
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