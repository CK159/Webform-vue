var vueApp = new Vue({
	el: "#vueContainer",
	data: function () {
		return {
			codeId: 1,
			codeName: "",
			isNewCode: false,
			codeAttributes: [],
			codeAttributeValues: {},
			newIdCounter: 0,
			newCodeName: "",
			modal: {
				showCodeDelete: false,
				showCodeAttributeAdd: false,
				addAttributeId: null
			},
		};
	},
	methods: {
		loadCodeAttributes: function () {
			var vm = this;

			if (vm.codeId <= 0) {
				vm.codeAttributes = [];
				return;
			}

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

			if (codeAttributeId <= 0) {
				Vue.set(vm.codeAttributeValues, codeAttributeId, []);
				return;
			}

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
		},
		newId: function () {
			return --this.newIdCounter;
		},
		openCreateCode: function () {
			var vm = this;
			vm.$nextTick(function () {
				//TODO: This doesn't work for the first open for some reason
				vm.$refs["newCodeName"].focus();
				console.log(vm.$refs["newCodeName"]);
			});
		},
		newCodeClick: function () {
			this.codeId = this.newId();
			this.codeName = this.newCodeName;
			this.isNewCode = true;
		},
		addAttrClick: function (AttributeId) {
			var vm = this;
			vm.modal.showCodeAttributeAdd = false;
			
			vm.api({
				action: "/api/select/attributeSelect",
				formData: {AttributeId: AttributeId, single: true},
				done: function (data) {
					var newItem = vm.getNewCodeAttribute(AttributeId, data[0].name);
					vm.codeAttributes.unshift(newItem);
				}
			});
		},
		addCodeAttrValClick: function (CodeAttributeId, AttributeId) {
			var newAttrVal = this.getNewCodeAttributeValue(AttributeId);
			this.codeAttributeValues[CodeAttributeId].unshift(newAttrVal);
		},
		//Generate new entities
		getNewCodeAttribute: function (AttributeId, AttributeName) {
			return {
				CodeAttributeId: this.newId(),
				AttributeId: AttributeId,
				AttributeName: AttributeName
			};
		},
		getNewCodeAttributeValue: function (AttributeId) {
			return {
				CodeAttributeValueId: this.newId(),
				AttributeId: AttributeId,
				AttributeValueId: null,
				ValueName: ""
			};
		},
		removeCodeClick: function () {
			this.codeId = null;
			this.codeName = "";
			this.isNewCode = false;
			this.modal.showCodeDelete = false;
		},
		removeCodeAttrClick: function (CodeAttributeId) {
			var index = this.codeAttributes.findIndex(function (item) {
				return item.CodeAttributeId === CodeAttributeId;
			});
			
			this.codeAttributes.splice(index, 1);
		},
		removeCodeAttrValClick: function (CodeAttributeId, CodeAttributeValueId) {
			var data = this.codeAttributeValues[CodeAttributeId];
			
			var index = data.findIndex(function (item) {
				return item.CodeAttributeValueId === CodeAttributeValueId;
			});
			
			data.splice(index, 1);
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