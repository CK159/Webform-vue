Vue.component("clear-btn", {
	template: "#clear-btn-template",
	sync: ["value"],
	props: {
		default: {
			default: function () {
				return ["", null];
			},
			type: [Array, String, Number, Boolean, Date]},
		defaultClasses: {
			default: function () {
				return ["btn", "btn-xs", "btn-danger"];
			},
			type: Array
		}
	},
	computed: {
		showBtn: function () {
			if (Array.isArray(this.default)) {
				return this.default.indexOf(this.value) < 0;
			}
			return this.default !== this.value;
		}
	},
	methods: {
		clear: function () {
			if (Array.isArray(this.default)) {
				if (this.default.length > 0) {
					this.value = this.default[0];
					return;
				}
				
				this.value = null;
				return;
			}

			this.value = this.default;
		}
	}
});