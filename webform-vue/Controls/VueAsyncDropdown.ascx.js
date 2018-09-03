var vueApp = new Vue({
	el: "#vueContainer",
	data: function () {
		return {
			codeId: 1,
			codeAttributes: [],
			codeAttributeValues: {}
		};
	},
	methods: {
		loadCodeAttributes: function () {
			var vm = this;
			vm.api({
				action: "/api/SimpleSelect/GetCodeAttributes",
				formData: {CodeId: vm.codeId},
				done: function (data) {
					vm.codeAttributes = data;
				}
			});
		},
		loadCodeAttributeValues: function (codeAttributeId) {
			var vm = this;
			vm.api({
				action: "/api/SimpleSelect/GetCodeAttributeValues",
				formData: {CodeAttributeId: codeAttributeId},
				done: function (data) {
					//Vue can't detect direct changes to arrays by index. Use helper method.
					Vue.set(vm.codeAttributeValues, codeAttributeId, data);
				}
			});
		},
		safeCodeAttrVal: function (CodeAttributeId) {
			return this.codeAttributeValues[CodeAttributeId] || [];
		}
	},
	watch: {
		codeId: function (newVal) {
			this.loadCodeAttributes();
		},
		codeAttributes: function (newVal) {
			this.codeAttributeValues = {};
			for (var i = 0; i < newVal.length; i++) {
				this.loadCodeAttributeValues(newVal[i].CodeAttributeId);
			}
		}
	},
	created: function () {
		this.loadCodeAttributes();
	}
});