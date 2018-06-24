//Helper functions

//Get random int in range [min, max] a.k.a. Can incluse min value and max value in result
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

