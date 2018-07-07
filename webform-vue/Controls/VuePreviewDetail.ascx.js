var vueApp = new Vue({
	el: "#vueContainer",
	data: function () {
		return {
			preview: [
				{date: "2018-06-30", id: 1009, name: "Static data", category: "Not Sure", code: "123", value: "6.89", active: true}
			],
			detail: {},
			//Default values used when user creates new record
			newDetail: {
				name: "",
				description: "",
				active: false,
				type: "alpha",
				detailConfig: []
			},
			apiActions: {
				previewLoad: "ApiThingyController.cs/PreviewDetail/Preview",
				detailLoad: "ApiThingyController.cs/PreviewDetail/Load",
				detailSave: "ApiThingyController.cs/PreviewDetail/Save",
				detailDelete: "ApiThingyController.cs/PreviewDetail/Delete",
			}
		}
	},
	methods: {
		itemActive: function (previewid) {
			console.log(this.$refs);
			if (!this.$refs.hasOwnProperty("pdRef")){
				return false;
			} 
			return this.$refs["pdRef"].itemActive(previewid, this.detail.id);
		}
	}
});