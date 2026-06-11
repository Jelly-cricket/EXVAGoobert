extends Modulator
class_name DivideMod

@export var _divisor : float

func applied(x : float) -> float :
	return x / _divisor
