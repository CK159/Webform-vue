Vue.component("my-checkbox", {
	template: "#my-checkbox-template",
	props: {
		title: String,
		checked: Boolean
	},
	methods: {
		check: function() { this.$emit('update:checked', !this.checked); }
	}
});