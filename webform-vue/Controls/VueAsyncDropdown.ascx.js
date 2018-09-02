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
		getCodeAttributes: function () {
			var vm = this;
			vm.api({
				action: "/api/VuexSelect/GetCodeAttributes",
				formData: {CodeId: vm.codeId},
				done: function (data) {
					vm.codeAttributes = data;
				}
			});
		},
		getCodeAttributeValues: function (codeAttributeId) {
			var vm = this;
			vm.api({
				action: "/api/VuexSelect/GetCodeAttributeValues",
				formData: {CodeAttributeId: codeAttributeId},
				done: function (data) {
					//Vue can't detect direct changes to arrays by index. Use helper method.
					Vue.set(vm.codeAttributeValues, codeAttributeId, data);
				}
			});
		}
	},
	watch: {
		codeId: function (newVal) {
			this.getCodeAttributes();
		},
		codeAttributes: function (newVal) {
			this.codeAttributeValues = {};
			for (var i = 0; i < newVal.length; i++) {
				this.getCodeAttributeValues(newVal[i].CodeAttributeId);
			}
		}
	},
	created: function () {
		this.getCodeAttributes();
	}
});