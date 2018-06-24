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

Vue.component("my-input", {
	template: "#my-input-template",
	props: {
		type: Number,
		value: String
	},
	//Data in components must be a function that returns an object
	//(So Vue can get a fresh copy of data for every instance created or something)
	data: function () {
		return {
			showModal: false
		}
	},
	computed: {
		//the 'value' prop is controlled by the parent component.
		//We can't change it directly - we need to notify the parent to change it via an event ($emit)
		//then the change will propagate back down to this component
		//Vue.js has handy constructs that assume the element is read and writable (v-model)
		//This computed property emulates a read-write element because it tells the parent to update on write
		//Parent component listens to events manually or uses .sync when binding to auto-listen for update events
		intVal: {
			get: function () {
				return this.value;
			},
			set: function (newVal) {
				this.$emit('update:value', newVal);
			}
		},
		//Select and Buttons input method don't support arbitrary strings so lowercase input and convert unknowns to '?'
		intValQ: {
			get: function () {
				var val = this.value.toLowerCase();
				if (!["a", "b", "c", "1", "2", "3", "?"].includes(val)) {
					val = "?";
				}
				return val;
			},
			set: function (newVal) {
				this.$emit('update:value', newVal);
			}
		}
	}
});

// register modal component
Vue.component('modal', {
	template: '#modal-template'
});

var vueApp = new Vue({
	el: "#vueContainer",
	data: {
		//Left panel examples
		simpleMessage: "Hello Vue.js!",
		simpleChecked: true,
		simpleCheckboxes: [],
		
		//Right panel examples
		multiInputSelect: 1,
		multiInputData: [
			{id:0, type: 1, value: "1"},
			{id:1, type: 3, value: "b"},
			{id:2, type: 4, value: "3"},
			{id:3, type: 5, value: "2"}
		],
		multiInputNames: [
			"N/A",
			"Plain Input",
			"Textarea",
			"Select",
			"Buttons",
			"Mildly Inconvenient Text Box"
		]
	},
	computed: {
		//Left panel examples
		simpleLengthLeft: function () {
			return 30 - this.simpleMessage.length;
		},
		simple2sComplement: function () {
			var sum = this.simpleCheckboxes.reduce((a, b) => parseInt(a, 10) + parseInt(b, 10), 0);
			return window.toTwosComplement(sum, 5);
		}
	},
	methods: {
		//Right panel examples
		multiInputAdd: function () {
			var nextID = this.multiInputData.reduce((max, p) => p.id > max ? p.id : max, 0) + 1;
			this.multiInputData.push({id: nextID, type: this.multiInputSelect, value: "New " + nextID});
		},
		multiInputRandomize: function () {
			this.multiInputData.forEach(function (element) {
				element.type = getRandomInt(1, 5);
			});
		},
		multiInputRemove: function (index) {
			this.multiInputData.splice(index, 1);
		}
	}
});