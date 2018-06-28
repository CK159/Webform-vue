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
Vue.filter('formatDate', function(value) {
	if (value) {
		if (!value) return "";
		return new Date(value).toLocaleDateString("en-US", {day: "2-digit", month: "2-digit", year: "numeric"});
	}
});