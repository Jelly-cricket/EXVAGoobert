extends Label
class_name DeltaLabel

@export var CountSpeed : float = 100
@export var CountDelay : float = 0.5

var Current : float
var CountTimer : float

func _process(delta: float) -> void:
	if CountTimer <= 0:
		Current = move_toward(Current, 0 , delta * CountSpeed)
	else:
		CountTimer -= delta
	if Current == 0:
		text = ""
	else:
		var prefix = ""
		if Current > 0:
			prefix = "+"
		var cur = "%.1f" % Current
		text = str(prefix,cur)
func QueueChange(amount : float) -> void:
	Current += amount
	CountTimer = CountDelay
