Vue.component("dual-checkboxes", {
	template: "#dual-checkboxes-template",
	sync: ["value"],
	props: {
		defaultClasses: {
			default: function () {
				return ["form-group"];
			},
			type: Array
		}
	},
	data: function () {
		return {
			isActive: false,
			isInactive: false
		};
	},
	methods: {
		valueChange: function(newVal) {
			if (newVal !== null) {
				//active is true or false, so both checkboxes have a defined state to be in
				this.isActive = newVal === true;
				this.isInactive = newVal === false;
			}
			else if (newVal === null && this.isActive !== this.isInactive) {
				//Only one checkbox is checked but both should be in the same state - default to unchecked
				this.isActive = false;
				this.isInactive = false;
			}
		}	
	},
	watch: {
		isActive: function(newVal) {
			this.value = newVal === this.isInactive ? null : newVal;
		},
		isInactive: function(newVal) {
			this.value = newVal === this.isActive ? null : !newVal;
		},
		value: function(newVal) {
			this.valueChange(newVal);
		}
	},
	created: function () {
		this.valueChange(this.value);
	}
});