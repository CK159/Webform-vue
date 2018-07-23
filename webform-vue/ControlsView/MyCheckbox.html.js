Vue.component("my-checkbox", {
	template: "#my-checkbox-template",
	props: {
		title: String,
		checked: Boolean
	},
	methods: {
		check() { this.$emit('update:checked', !this.checked); }
	}
});