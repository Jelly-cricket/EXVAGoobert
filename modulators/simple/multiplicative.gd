extends Modulator
class_name MultMod

@export var _factor : float = 1

func applied(x : float) -> float :
	return x * _factor
