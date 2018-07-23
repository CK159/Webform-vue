Vue.component("my-input", {
	template: "#my-multi-input-template",
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