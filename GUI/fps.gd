extends Label

@export var body : CharacterBody3D
func _process(delta: float) -> void:
	text = str(
		"afps: ",Engine.get_frames_per_second(),
		"\nspf: ",Performance.get_monitor(Performance.TIME_PROCESS),
		"\nvelocity: ",body.velocity,
		"\nspeed: ",body.velocity.length(),
		"\nhorizontalspeed:",Vector3(body.velocity.x,0,body.velocity.z).length()
	)
