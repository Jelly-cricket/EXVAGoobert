extends Modulator
class_name CompoundMod

@export var _mods : Array[Modulator]

func applied(x : float) -> float:
	var final = x
	for i in _mods :
		final = i.applied(final)
	return final
