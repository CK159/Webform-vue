//Helper functions
var globalAjaxDelay = 350;

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

			if (!key) {
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
				done: function (data) {
					//Define custom success action
				},
				fail: function (messages, details) {
					//Define custom failure action
				},
				always: null,

				//Uncommon properties
				dataType: "json",
				baseURL: "",
				method: "POST",
				//Turn on or off standard failure messaging behavior
				//If enabled, this will run IN ADDITION TO fail() callback. See failDefault()
				failDefaultEnabled: true, 
				formDataDateConverter: true,
				doneHandler: function (data) {
					/*if (typeof data !== "undefined" && data.hasOwnProperty("error") && data.error === false) {
						opt.failHandler(data.message, data)
					}
					else if (typeof opt.done === "function") {
						opt.done(data);
					}*/
					if (typeof opt.done === "function") {
						opt.done(data);
					}
				},
				failHandler: function (jqXHR) {
					var messages = jqXHR.statusText;
					var details = jqXHR.responseText;

					if (jqXHR.responseJSON) {
						var response = jqXHR.responseJSON;

						//Custom error response format {Messages: [], Details: []}
						//generously provided by GlobalLocalLiveDebugMessagingDetailsExceptionHandlerMKⅫ
						if (response.hasOwnProperty("Messages")) {
							messages = response.Messages;
						}
						if (response.hasOwnProperty("Details")) {
							details = response.Details;
						}

						//Web API standard error response format {Message: "", MessageDetail: ""}
						if (response.hasOwnProperty("Message")) {
							messages = [response.Message];
						}
						if (response.hasOwnProperty("MessageDetail")) {
							details = response.MessageDetail;
						}
					}

					var logStr = "API Error:\n" + messages.join("\n");
					//Ensures text part ends with new line so that details part starts on its own line in browser console
					logStr += logStr.endsWith("\n") ? "" : "\n";
					console.log(logStr, details);
					if (typeof opt.fail === "function") {
						opt.fail(messages, details);
					}
					if (opt.failDefaultEnabled) {
						opt.failDefault(messages, details);
					}
				},
				//Displays error messages in fancy Vue modal popup if available or simple alert otherwise
				failDefault: function (messages, details) {
					if (EventBus) {
						//Fail Fancy
						EventBus.$emit("popup-error", {
							body: messages
						});
					}
					else {
						//Fail boring
						alert(messages.join("\n"));
					}
				},
				//Extra time in ms added to delay AJAX requests.
				delay: globalAjaxDelay || 0
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
						opt.failHandler(jqXHR);
					})
					.always(function () {
						if (typeof opt.always === "function") {
							opt.always();
						}
					});
			}
		}
	},
	//Allows specifying sync: ["prop1", "prop2", ...] on components for automatic 2-way data binding of each prop
	//Parent component just need to use v-bind.sync
	//https://jsfiddle.net/Herteby/daL40n19
	//Now also correctly translates prop-names kebab-case in templates to propNames camelCase in JS
	beforeCreate() {
		const sync = this.$options.sync;
		if (!sync)
			return;

		if (!this.$options.computed) {
			this.$options.computed = {}
		}
		const attrs = Object.keys(this.$attrs);
		sync.forEach(key => {
			var kebabKey = camelToKebab(key);
			if (!attrs.includes(kebabKey)) {
				Vue.util.warn(`Missing required sync-prop: "${kebabKey}"`, this)
			}

			this.$options.computed[key] = {
				get() {
					return this.$attrs[kebabKey];
				},
				set(val) {
					this.$emit('update:' + key, val);
				}
			};
		});

		function camelToKebab(input) {
			return input.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
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

//https://stackoverflow.com/a/52311051
function file2Base64(file) {
	return new Promise((resolve, reject) => {
		const reader = new FileReader();
		reader.readAsDataURL(file);
		reader.onload = () => {
			let encoded = reader.result.replace(/^.*;base64,/, '');
			if ((encoded.length % 4) > 0) {
				encoded += '='.repeat(4 - (encoded.length % 4));
			}
			resolve(encoded);
		};
		reader.onerror = error => reject(error);
	});
}

var nextNewId = (function () {
	var counter = -1;
	return function () {
		return counter--;
	}
})();

function fixSortOrder(arr, key) {
	key = key || "SortOrder";

	for (var i = 0; i < arr.length; i++) {
		arr[i][key] = i + 1;
	}
}