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
			previewState: "unloaded", //unloaded, loading, loaded
			detailState: "unloaded", //unloaded, loading, loaded, new
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
				formData: {[this.pk]: id},
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
			
			var saveLoad = vm.detail[vm.pk] <= 0 ? " created." : " saved.";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.detailSave,
				jsonData: vm.detail,
				done: function (data) {
					vm.showMessage("success", "Item " + data[vm.pk] + saveLoad);
					vm.detail = data; //Get any updates applied by server to record (new ID, created dates, field formatting, etc.)
					vm.previewLoad(); //Refresh preview (if name or other properties changes)
				},
				always: function () {
					vm.detailState = "loaded"; //TODO: don't set to loaded if request fails
				}
			});
		},
		detailCancel: function () {
			this.clearMessage();
			this.detailState = "unloaded";
		},
		detailNew: function () {
			this.clearMessage();
			this.detailState = "new";
			this.detail = Object.assign({}, this.newDetail); 
		},
		detailDelete: function () {
			this.clearMessage();
			if (this.detailState !== "loaded"){
				return; //Only allow deleting existing records. If unloaded, do nothing.
			}

			var vm = this;
			this.detailState = "loading";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.detailDelete,
				formData: {[vm.pk]: vm.detail[vm.pk]},
				dataType: "text", //No response content is returned, cant be set to json
				done: function (data) {
					vm.showMessage("success", "Item " + vm.detail[vm.pk] + " deleted.");
					vm.previewLoad(); //Refresh preview to remove deleted element
				},
				always: function () {
					vm.detailState = "unloaded"; //TODO: Don't set to unloaded on failure
				}
			});
		},
		//Hides the success / error message indicator
		clearMessage: function () {
			this.messageType = "none";
		},
		showMessage: function (type, msg) {
			this.messageType = type;
			this.messageText = msg;
		}
	},
	created: function () {
		this.previewLoad();
	}
});