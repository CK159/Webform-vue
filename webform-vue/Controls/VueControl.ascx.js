var vueApp = new Vue({
	el: "#vueContainer",
	data: {
		//Left panel examples
		simpleMessage: "Hello Vue.js!",
		simpleChecked: true,
		simpleCheckboxes: [],
		
		//Right panel examples
		multiInputSelect: 1,
		multiInputData: [
			{id:0, type: 1, value: "1"},
			{id:1, type: 3, value: "b"},
			{id:2, type: 4, value: "3"},
			{id:3, type: 5, value: "2"}
		],
		multiInputNames: [
			"N/A",
			"Plain Input",
			"Textarea",
			"Select",
			"Buttons",
			"Mildly Inconvenient Text Box"
		],
		
		//Remaining Examples
		recordPreview: [/*{id: 1, name: "first", active: true, date: "2018-06-23"}*/],
		recordPreviewState: "unloaded", //unloaded, loading, loaded
		recordState: "unloaded", //unloaded, loading, loaded, new
		recordDetail: {
			id: "",
			name: "",
			description: "",
			active: false,
			date: "",
			prices: [/*{id: 0, type: 0, amount: 0, name: ""}*/]
		},
		recordPriceTypes: [
			"Price",
			"Shipping",
			"Fee",
			"Misc.",
			"Other",
			"N/A",
		],
		
		//Neverending Examples...
		artificialDelay: 250,
		endpoint: ["asmx"] //asmx, mvc
	},
	computed: {
		//Left panel examples
		simpleLengthLeft: function () {
			return 30 - this.simpleMessage.length;
		},
		simple2sComplement: function () {
			var sum = this.simpleCheckboxes.reduce(function (a, b) {
				return parseInt(a, 10) + parseInt(b, 10);
			}, 0);
			return window.toTwosComplement(sum, 5);
		}
	},
	methods: {
		//Right panel examples
		multiInputAdd: function () {
			var nextID = this.multiInputData.reduce(function (max, p) {
				return p.id > max ? p.id : max;
			}, 0) + 1;
			this.multiInputData.push({id: nextID, type: this.multiInputSelect, value: "New " + nextID});
		},
		multiInputRandomize: function () {
			this.multiInputData.forEach(function (element) {
				element.type = getRandomInt(1, 5);
			});
		},
		multiInputRemove: function (index) {
			this.multiInputData.splice(index, 1);
		},

		//Remaining Examples
		//Sumplified API interaction function with useful default values
		ezAjax: function (argOpts) {
			var opt = $.extend({
				action: "",
				data: {},
				dataType: "json",
				done: null,
				fail: function (jqXHR, textStatus){
					console.log("Error: " + textStatus, jqXHR);
					alert( "Error: " + textStatus);
				},
				always: null
			}, argOpts);
			
			var url = "/application/ApiThingy.asmx/" + opt.action;
			
			setTimeout(function(){
				$.ajax({
					url: url,
					data: opt.data,
					method: "POST",
					dataType: opt.dataType
				})
					.done(function(data) {
						if (typeof opt.done === "function") { opt.done(data); }
					})
					.fail(function(jqXHR, textStatus) {
						if (typeof opt.fail === "function") { opt.fail(jqXHR, textStatus); }
					})
					.always(function() {
						if (typeof opt.always === "function") { opt.always(); }
					});
			}, this.artificialDelay);
		},
		//Manipulation functions for record preview and detail
		recordPreviewLoad: function () {
			var vm = this;
			this.recordPreviewState = "loading";
			
			this.ezAjax({
				action: "GetRecordPreview",
				data: {},
				done: function (data) {
					vm.recordPreview = data;
				},
				always: function () {
					vm.recordPreviewState = "loaded";
				}
			});
		},
		recordLoad: function (id) {
			var vm = this;
			this.recordState = "loading";
			
			this.ezAjax({
				action: "GetRecordDetail",
				data: {id: id},
				done: function (data) {
					vm.recordDetail = data;
				},
				always: function () {
					vm.recordState = "loaded";
				}
			});
		},
		recordSave: function () {
			if (this.recordState != "loaded" && this.recordState != "new"){
				return; //Only allow saving loaded or new records.
			}
			
			var vm = this;
			this.recordState = "loading";

			this.ezAjax({
				action: "SaveRecordDetail",
				data: {json: JSON.stringify(vm.recordDetail)},
				done: function (data) {
					vm.recordDetail.id = data.id; //Update id of this record (for new records)
					vm.recordPreviewLoad(); //Refresh preview (if name or other properties changes)
				},
				always: function () {
					vm.recordState = "loaded";
				}
			});
		},
		recordCancel: function () {
			this.recordState = "unloaded";
		},
		recordNew: function () {
			this.recordState = "new";
			this.recordDetail = {
				id: -1,
				name: "",
				description: "",
				active: false,
				date: "",
				prices: []
			};
		},
		recordDelete: function () {
			if (this.recordState != "loaded"){
				return; //Only allow deleting existing records. If unloaded, do nothing.
			}
			
			var vm = this;
			this.recordState = "loading";

			this.ezAjax({
				action: "DeleteRecordDetail",
				data: {id: vm.recordDetail.id},
				dataType: "text", //No response content is returned, cant be set to json
				done: function (data) {
					vm.recordPreviewLoad(); //Refresh preview to removel deleted element
				},
				always: function () {
					vm.recordState = "unloaded";
				}
			});
		},
		//Manipulation functions for record detail prices
		priceRemove: function (index) {
			console.log(index);
			this.recordDetail.prices.splice(index, 1);
		},
		priceAdd: function () {
			//Find the next available ID to use
			var nextID = this.recordDetail.prices.reduce(function (max, p) {
				return p.id > max ? p.id : max;
			}, 0) + 1;
			
			//Put new entry at the top
			this.recordDetail.prices.unshift({
				id: nextID,
				type: 0,
				price: 0,
				name: "New Price"
			});
		}
	},
	created: function () {
		this.recordPreviewLoad();
	},
	components: {
		vuejsDatepicker
	}
});