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
		componentDataReset: function (key) {
			//https://stackoverflow.com/a/43643407
			var origData = this.$options.data.call(this);
			
			if (!key){
				//Reset data entirely
				Object.assign(this.$data, origData);
			}
			else {
				//Reset one data key only
				Object.assign(this.$data[key], origData[key]);
			}
		},
		api: function (argOpts) {
			var opt = $.extend({
				/*Common properties*/
				action: "",
				formData: null,
				jsonData: null,
				done: null,
				fail: function (message, details) {
					alert("Error: " + message);
				},
				always: null,

				//Uncommon properties
				dataType: "json",
				baseURL: "",
				method: "POST",
				formDataDateConverter: true,
				doneHandler: function (data) {
					if (typeof data !== "undefined" && data.hasOwnProperty("error") && data.error === false) {
						opt.failHandler(data.message, data)
					}
					else if (typeof opt.done === "function") {
						opt.done(data);
					}
				},
				failHandler: function (message, details) {
					console.log("Error: " + message, details);
					if (typeof opt.fail === "function") {
						opt.fail(message, details);
					}
				},
				//Minimum time in ms to complete AJAX request. 
				//Delay will be added if request is shorter than this - for local testing
				delay: 350
			}, argOpts);

			var finalData = null;
			var contentType = null;

			//Validation
			if (opt.hasOwnProperty("data")) {
				console.log("Use formData or jsonData instead of just 'data'. Always pass in an object (no serialization).", opt);
			}
			if (opt.formData != null && opt.jsonData != null) {
				console.log("Use either formData or jsonData, not both.", opt);
			}

			if (opt.formData != null) {
				finalData = opt.formData;
				if (opt.formDataDateConverter) {
					DatesToISO8601(finalData);
				}
				contentType = "application/x-www-form-urlencoded";
			}
			else if (opt.jsonData != null) {
				finalData = JSON.stringify(opt.jsonData);
				contentType = "application/json";
			}

			if (opt.delay > 0) {
				window.setTimeout(doTheStuff, opt.delay);
			}
			else {
				doTheStuff();
			}

			function doTheStuff() {
				$.ajax({
					url: opt.baseURL + opt.action,
					contentType: contentType,
					data: finalData,
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
		}
	},
	//Allows specifying sync: ["prop1", "prop2"] on components for automatic 2-way data binding
	//Parent component just need to use v-bind.sync
	//https://jsfiddle.net/Herteby/daL40n19
	beforeCreate() {
		const sync = this.$options.sync;
		if (sync) {
			if (!this.$options.computed) {
				this.$options.computed = {}
			}
			const attrs = Object.keys(this.$attrs);
			sync.forEach(key => {
				if (!attrs.includes(key)) {
					Vue.util.warn(`Missing required sync-prop: "${key}"`, this)
				}
				this.$options.computed[key] = {
					get() {
						return this.$attrs[key]
					},
					set(val) {
						this.$emit('update:' + key, val)
					}
				}
			})
		}
	}
});

//Object.Assign polyfill. Because care about IE I guess...
//from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/assign
if (typeof Object.assign != 'function') {
	// Must be writable: true, enumerable: false, configurable: true
	Object.defineProperty(Object, "assign", {
		value: function assign(target, varArgs) { // .length of function is 2
			'use strict';
			if (target == null) { // TypeError if undefined or null
				throw new TypeError('Cannot convert undefined or null to object');
			}

			var to = Object(target);

			for (var index = 1; index < arguments.length; index++) {
				var nextSource = arguments[index];

				if (nextSource != null) { // Skip over if undefined or null
					for (var nextKey in nextSource) {
						// Avoid bugs when hasOwnProperty is shadowed
						if (Object.prototype.hasOwnProperty.call(nextSource, nextKey)) {
							to[nextKey] = nextSource[nextKey];
						}
					}
				}
			}
			return to;
		},
		writable: true,
		configurable: true
	});
}

function DatesToISO8601(obj) {
	for (var prop in obj) {
		if (obj.hasOwnProperty(prop)) {
			if (obj[prop] instanceof Date) {
				obj[prop] = obj[prop].toISOString();
			}
		}
	}
}