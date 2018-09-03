Vue.component("async-dropdown", {
	template: "#async-dropdown-template",
	props: {
		value: Number, 
		apiUrl: String,
		apiKey: String, //API parameter name to look up selected value directly
		apiData: { //Other data to submit to API, excluding apiKey
			default: function () {
				return {};
			},
			type: Object
		},
		friendlyName: {
			default: "Item",
			type: String
		},
		showDefault: {
			default: true,
			type: Boolean
		},
		defaultValue: {
			default: null,
			type: Number
		},
		defaultText: {
			default: null,
			type: String
		},
		inputClass: {
			default: function () {
				return ["form-control"];
			},
			type: [Array,  Object]
		}
	},
	data: function () {
		return {
			status: "unloaded", //unloaded, pending, loading, loaded, error
			rawData: {}
		}
	},
	computed: {
		innerValue:	{
			get: function () {
				return this.value;
			},
			set: function (newVal) {
				this.$emit('input', this.toNullInt(newVal));
			}
		},
		options: function () {
			//Handle awaiting data
			if (this.status === "loading") {
				return [{value: this.value, name: "Loading..."}];
			}
			
			//Handle any strange error conditions
			if (this.status !== "loaded") {
				return [{value: this.value, name: this.status}];
			}
			
			//Shallow copy the source data so that more elements can be added
			var opt = this.rawData.slice();
			
			if (opt.length === 0) {
				//No data
				opt.push({value: this.value, name: "No " + this.friendlyName + " available"});
			}

			if (opt.length > 0 && this.showDefault) {
				//Default option - Put at top of list
				var defaultTextExt = this.defaultText == null ? "--Select " + this.friendlyName + "--" : this.defaultText;
				opt.unshift({value: this.defaultValue, name: defaultTextExt});
			}
			
			if (opt.length > 0 && !this.hasSelected(opt)) {
				//Selected item not in data - Put dummy option at very start of list
				//TODO: Have way to keep this value in list after user makes a different selection to allow them to set it back to this
				opt.unshift({value: this.value, name: "Unknown " + this.friendlyName + "ID: " + this.value});
			}
			
			return opt;
		}
	},
	methods: {
		hasSelected: function (data) {
			for (var i = 0; i < data.length; i++) {
				if (data[i].value === this.value || (data[i].value === null && this.value === null)){
					return true;
				}
			}
			return false;
		},
		//Ensures that api is not called due to multiple data fields changing within a single blocking JS operation
		queueData: function () {
			if (this.status !== "pending") {
				this.status = "pending";
				var vm = this;
				setTimeout(function () {
					vm.callApi();
				}, 0);
			}
		},
		callApi: function () {
			var vm = this;
			vm.status = "loading";
			
			var apiFull = Object.assign({}, vm.apiData, {[vm.apiKey]: vm.value});

			vm.api({
				action: vm.apiUrl,
				formData: apiFull,
				done: function (data) {
					vm.rawData = data;
					vm.status = "loaded";
				},
				fail: function (message) {
					vm.status = "error";
					alert("Error: " + message);
				}
			});
		},
		toNullInt: function (str) {
			return isNaN(parseInt(str)) ? null: parseInt(str);
		}
	},
	watch: {
		apiUrl: function (newVal, oldVal) {
			console.log("apiUrl changed:", newVal, oldVal); //TODO: remove
			this.queueData();
		},
		apiKey: function (newVal, oldVal) {
			console.log("apiKey changed:", newVal, oldVal); //TODO: remove
			this.queueData();
		},
		apiData: {
			handler: function (newVal, oldVal) {
				console.log("apiData changed:", newVal, oldVal); //TODO: remove
				this.queueData();
			},
			deep: true
		}
	},
	created: function () {
		this.queueData();
	}
});