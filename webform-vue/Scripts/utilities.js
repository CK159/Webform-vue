//Helper functions

//Get random int in range [min, max] -> Can include min and max values in result
function getRandomInt(min, max) {
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

//Based on https://stackoverflow.com/a/48830705
//C# is basically JS, right?
function toTwosComplement(value, numberBits) {
	var MODULO = 1 << numberBits;
	var MAX_VALUE = (1 << (numberBits - 1)) - 1;

	if (value > MAX_VALUE) {
		value -= MODULO;
	}
	return value;
}

// Global filter used to format dates for display in preview table and elsewhere
Vue.filter("formatDate", function (value) {
	if (value) {
		if (!value) return "";
		return new Date(value).toLocaleDateString("en-US", {day: "2-digit", month: "2-digit", year: "numeric"});
	}
});

Vue.mixin({
	methods: {
		api: function (argOpts) {
			var opt = $.extend({
				/*Common properties*/
				action: "",
				data: {},
				done: null,
				fail: function (message, details) {
					alert("Error: " + message);
				},
				always: null,
				
				/*Uncommon properties*/
				dataType: "json",
				baseURL: "",
				method: "POST",
				doneHandler: function (data) {
					if (data.error === false) {
						opt.failHandler(data.message, data)
					}
					else if (typeof opt.done === "function") {
						opt.done(data);
					}
				},
				failHandler: function (message, details) {
					console.log("Error: " + message, details);
					if (typeof opt.done === "function") {
						opt.fail(message,  details);
					}
				},
			}, argOpts);

			$.ajax({
				url: opt.baseURL + opt.action,
				data: opt.data,
				method: opt.method,
				dataType: opt.dataType
			})
				.done(opt.doneHandler)
				.fail(function (jqXHR, textStatus) {
					opt.failHandler(textStatus, jqXHR); //Reverse the arguments for standardization
				})
				.always(function () {
					if (typeof opt.always === "function") {
						opt.always();
					}
				});
		}
	},
	//Allows specifying sync: ["prop1", "prop2"] on components for automatic 2-way data binding
	//Parent component just need to use v-bind.sync
	//https://jsfiddle.net/Herteby/daL40n19
	beforeCreate(){
		const sync = this.$options.sync;
		if(sync){
			if(!this.$options.computed){
				this.$options.computed = {}
			}
			const attrs = Object.keys(this.$attrs);
			sync.forEach(key => {
				if(!attrs.includes(key)){
					Vue.util.warn(`Missing required sync-prop: "${key}"`, this)
				}
				this.$options.computed[key] = {
					get(){
						return this.$attrs[key]
					},
					set(val){
						this.$emit('update:' + key, val)
					}
				}
			})
		}
	}
});