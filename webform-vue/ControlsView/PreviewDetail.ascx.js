Vue.component("preview-detail", {
	template: "#preview-detail-template",
	sync: [
		"preview", //Array of items to show in preview section
		"detail" //Object to display in details section
	],
	props: {
		newDetail: Object, //What should be assigned to the detailData when 'new' button is clicked
		pk: String, //The primary key in the detail object (for loading and saving details)
		apiEndpoints: Object //{baseURL: "", previewLoad: "", detailLoad: "", detailSave: "", detailDelete: ""}
	},
	data: function () {
		return {
			previewState: "loaded", //unloaded, loading, loaded
			detailState: "loaded", //unloaded, loading, loaded, new
			messageType: "none", //none, success, error
			messageText: "" //Displayed in message box when messageType != "none"
		}
	},
	methods: {
		itemActive: function (id1, id2) {
			return id1 === id2 && this.detailState === 'loaded';
		},
		//Manipulation functions for record preview and detail
		previewLoad: function (keepMessage) {
			if (keepMessage === false) {
				this.clearMessage();
			}
			
			var vm = this;
			this.previewState = "loading";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.previewLoad,
				data: {},
				done: function (data) {
					vm.preview = data;
				},
				always: function () {
					vm.previewState = "loaded";
				}
			});
		},
		detailLoad: function (id) {
			this.clearMessage();
			var vm = this;
			this.detailState = "loading";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.detailLoad,
				data: {[this.pk]: id},
				done: function (data) {
					vm.detail = data;
				},
				always: function () {
					vm.detailState = "loaded";
				}
			});
		},
		detailSave: function () {
			this.clearMessage();
			if (this.detailState !== "loaded" && this.detailState !== "new"){
				return; //Only allow saving loaded or new records.
			}

			var vm = this;
			this.detailState = "loading";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.detailSave,
				data: {json: JSON.stringify(vm.recordDetail)},
				done: function (data) {
					vm.detail[vm.pk] = data[vm.pk]; //Update id of this record (for new records)
					vm.previewLoad(false); //Refresh preview (if name or other properties changes)
				},
				always: function () {
					vm.detailState = "loaded"; //TODO: don't set to loaded if request fails
				}
			});
		},
		detailCancel: function () {
			this.clearMessage();
			this.recordState = "unloaded";
		},
		detailNew: function () {
			this.clearMessage();
			this.recordState = "new";
			this.detail = this.newDetail; //TODO: need to make a copy here?
		},
		detailDelete: function () {
			this.clearMessage();
			if (this.recordState !== "loaded"){
				return; //Only allow deleting existing records. If unloaded, do nothing.
			}

			var vm = this;
			this.recordState = "loading";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.detailDelete,
				data: {[vm.pk]: vm.detail[vm.pk]},
				dataType: "text", //No response content is returned, cant be set to json
				done: function (data) {
					vm.previewLoad(false); //Refresh preview to remove deleted element
				},
				always: function () {
					vm.recordState = "unloaded";
				}
			});
		},
		//Hides the success / error message indicator
		clearMessage: function () {
			this.messageType = "none";
		}
	},
	created: function () {
		this.previewLoad();
	}
});