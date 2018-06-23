Vue.component("my-checkbox", {
	template: "#checkbox-template",
	props: {
		title: String,
		checked: Boolean
	},
	data() {
		return { }
	},
	methods: {
		check() { this.$emit('update:checked', !this.checked); }
	}
});


var vueApp = new Vue({
	el: "#vueContainer",
	data: {
		simpleMessage: "Hello Vue.js!",
		simpleChecked: true
	},
	computed: {
		simpleLengthLeft: function () {
			return 30 - this.simpleMessage.length;
		}
	},
	methods: {
		
	}
});