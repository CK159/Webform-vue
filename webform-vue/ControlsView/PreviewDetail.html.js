Vue.component("preview-detail", {
	template: "#preview-detail-template",
	sync: [
		"preview", //Array of items to show in preview section
		"detail" //Object to display in details section
	],
	props: {
		search: Object, //Search and filter fields to pass to preview load api
		newDetail: Object, //What should be assigned to the detailData when 'new' button is clicked
		pk: String, //The primary key in the detail object (for loading and saving details)
		apiEndpoints: Object, //{baseURL: "", previewLoad: "", detailLoad: "", detailSave: "", detailDelete: ""}

		//Total page buttons shown. Up to 4 will be for first / last page + jump foward / back buttons
		pagesShown: {default: 14, type: Number},
		//Adjust how far forward / back the current page will appear in the page button list
		selectionOffset: {default: 1, type: Number}
	},
	data: function () {
		return {
			previewState: "unloaded", //unloaded, loading, loaded
			detailState: "unloaded", //unloaded, loading, loaded, new
			messageType: "none", //none, success, error
			messageText: "", //Displayed in message box when messageType != "none"
			currentPage: 0, //First page is page 0
			pages: 1,
			recordCount: 0,
			pageSize: 10,
			pageSizes: [10, 20, 50, 100, 250, null]
		}
	},
	computed: {
		fullPreviewData: function () {
			return Object.assign({}, this.search, {
				currentPage: this.currentPage,
				pageSize: this.pageSize
			});
		},
		shownPages: function () {
			var start = this.currentPage - Math.ceil(this.pagesShown / 2) + this.selectionOffset;
			var end = this.currentPage + Math.floor(this.pagesShown / 2) + this.selectionOffset;

			//Check if upper page range too high and shift left
			var overage = end - this.pages;
			if (overage > 0) {
				start -= overage;
				end -= overage;
			}

			//Check if lower page range too low and shift right
			if (start < 0) {
				end -= start;
				start = 0;
			}

			//Check if page range larger than total pages and shrink range
			end = Math.min(end, this.pages);

			var pages = [];
			for (var i = start; i < end; i++) {
				pages.push({
					page: i,
					text: i + 1,
					selected: i === this.currentPage,
					enabled: true
				});
			}

			if (pages.length > 2) {
				//First page shortcut (always shown as first element)
				pages[0] = {
					page: 0,
					text: 1,
					selected: this.currentPage === 0,
					enabled: true
				};
				//Last page shortcut (always shown as last element)
				pages[pages.length - 1] = {
					page: this.pages - 1,
					text: this.pages,
					selected: this.currentPage === this.pages - 1,
					enabled: true
				};
			}

			if (pages.length > 5) {
				//Jump 1 set back shortcut (shown as 2nd element if not at start)
				if (start > 0) {
					pages[1] = {
						page: Math.max(this.currentPage - this.pagesShown + 4, 0),
						text: "<<",
						selected: false,
						enabled: true
					};
				}

				//Jump 1 set forward shortcut (shown as 2nd to last element if not at end)
				if (end < this.pages) {
					pages[pages.length - 2] = {
						page: Math.min(this.currentPage + this.pagesShown - 4, this.pages - 1),
						text: ">>",
						selected: false,
						enabled: true
					};
				}
			}

			return pages;
		}
	},
	methods: {
		//To be accessed by parent component to check if a preview row should be highlighted based on loaded detail record
		itemActive: function (id1, id2) {
			return id1 === id2 && this.detailState === 'loaded';
		},
		//Can be accessed by parent component to load/reload search results.
		refreshSearch: function () {
			this.currentPage = 0;
			this.previewLoad();
		},
		previewLoad: function (keepMessage) {
			if (keepMessage === false) {
				this.clearMessage();
			}

			var vm = this;
			this.previewState = "loading";

			this.api({
				baseURL: this.apiEndpoints.baseURL,
				action: this.apiEndpoints.previewLoad,
				formData: this.fullPreviewData,
				done: function (data) {
					vm.preview = data.result;
					vm.currentPage = data.currentPage;
					vm.pages = data.pages;
					vm.recordCount = data.recordCount;
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
			if (this.detailState !== "loaded" && this.detailState !== "new") {
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
			if (this.detailState !== "loaded") {
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
		},
		pageChange: function (page) {
			this.currentPage = page;
			this.previewLoad();
		}
	},
	watch: {
		pageSize: function (newVal) {
			//The page size selector was altered. Refresh.
			this.currentPage = 0;
			this.previewLoad();
		}
	},
	created: function () {
		//Populate detail with default data to prevent undefined data references
		this.detail = Object.assign({}, this.newDetail);
		this.previewLoad();
	}
});