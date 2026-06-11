extends Modulator
class_name AdditiveMod

@export var _change : float = 0

func applied(x : float) -> float :
	return x + _change
