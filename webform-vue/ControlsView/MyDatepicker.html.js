Vue.component("my-datepicker", {
	template: "#my-datepicker-template",
	components: {vuejsDatepicker},
	props: {
		value: Date,
		format: {
			default: "MM/dd/yyyy",
			type: String
		},
		highlighted: {
			default: function () {
				return {dates: [new Date()]}; //Highlight today
			},
			type: Object
		},
		inputClass: {
			default: "form-control",
			type: String
		}
	},
	methods: {
		updateValue: function (val) {
			this.$emit('input', val);
		},
		clearAndClose: function () {
			this.updateValue(null);
			this.$refs["picker"].close(true);
		}
	}
});