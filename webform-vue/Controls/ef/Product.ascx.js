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
				"ProductTypeId": null,
				"Active": true,
				"Resources": [],
				"Catalogs": []
			},
			pk: "ProductId",
			apiUrl: "/api/Product/"
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
		fileAdded: function (e) {
			var vm = this;
			var chain = Promise.resolve();
			var newResources = [];

			for (var i = 0; i < e.target.files.length; i++) {
				var file = e.target.files[i];

				(function (f) {
					chain = chain.then(function () {
						return file2Base64(f);
					}).then(function (b64) {
						newResources.push(vm.createNewProductResource(f, b64));
					});
				})(file);
			}

			chain = chain.then(function () {
				vm.detail.Resources = newResources.concat(vm.detail.Resources);
				fixSortOrder(vm.detail.Resources);
				
				//Reset file input
				e.target.value = null;
			});

			chain.catch(function (e) {
				console.log("fileAdded failure", e);
			});
		},
		createNewProductResource: function (file, b64) {
			return {
				ProductResourceId: nextNewId(),
				ProductId: this.detail.ProductId,
				File: {
					FileId: nextNewId(),
					FileName: file.name,
					MimeType: file.type,
					Content: b64,
					DateCreated: new Date().toISOString()
				},
				SortOrder: 0,
				Active: true,
				DateCreated: new Date().toISOString()
			};
		},
		smartSrc: function (fileDto){
			if (fileDto.Content) {
				return "data:" + fileDto.MimeType + ";base64," + fileDto.Content;
			}
			return "/api/File/" + fileDto.FileId;
		}
	}
});