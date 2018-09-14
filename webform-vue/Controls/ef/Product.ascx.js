var vueApp = new Vue({
	el: "#vueContainer",
	data: function () {
		return {
			search: {
				productName: null,
				productDesc: null,
				catalogId: null,
				active: true
			},
			preview: [],
			detail: {},
			//Default values used when user creates new record TODO: easier way? Lazy-load from server?
			newDetail: {
				"ProductId": -1,
				"ProductName": "",
				"ProductDesc": "",
				"ProductRichDesc": "",
				"Type": null, //TODO: figure this out
				"Date": "0001-01-01T00:00:00",
				"Active": true,
				"ProductResources": [] //TODO: figure this out
			},
			pk: "ProductId",
			apiEndpoints: {
				baseURL: "/api/Product/",
				previewLoad: "Preview",
				detailLoad: "Load",
				detailSave: "Save",
				detailDelete: "Delete",
			}
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
	}
});