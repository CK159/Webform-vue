EventBus = EventBus || new Vue();

Vue.component("alert-popup", {
	template: "#alert-popup-template",
	data: function () {
		return {
			showPopup: false,
			header: "",
			body: "",
			bodyHtml: ""
		}
	},
	computed: {
		finalHeader: function(){
			return this.header == null || this.header === "" ? "Alert!" : this.header;
		},
		isHtml: function(){
			//HTML body content takes priority over plain text message
			return this.bodyHtml !== null || this.bodyHtml !== "";
		}
	},
	methods: {
		alertDefaultExtend: function(data){
			return Object.assign({
				showPopup: true,
				header: "",
				body: "",
				bodyHtml: null
			}, data);
		},
		errorDefaultExtend: function(data){
			return Object.assign({
				showPopup: true,
				header: "Error:",
				body: "Something went wrong.",
				bodyHtml: null
			}, data);
		},
		validationDefaultExtend: function(data){
			return Object.assign({
				showPopup: true,
				header: "Please correct the following issues and try again:",
				body: "Something went wrong.",
				bodyHtml: null
			}, data);
		},
		assignData: function(data){
			this.showPopup = data.showPopup;
			this.header = data.header;
			this.body = data.body;
			this.bodyHtml = data.bodyHtml;
		}
	},
	mounted() {
		EventBus.$on('popup-alert', function(data){
			this.assignData(this.alertDefaultExtend(data));
		});
		EventBus.$on('popup-error', function(data){
			this.assignData(this.errorDefaultExtend(data));
		});
		EventBus.$on('popup-validation', function(data){
			this.assignData(this.validationDefaultExtend(data));
		});
	}
});