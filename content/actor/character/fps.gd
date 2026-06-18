extends Label

@export var body : CharacterBody3D
func _process(delta: float) -> void:
	text = str(Engine.get_frames_per_second(),"\nvelocity: ",body.velocity,"\nspeed: ",body.velocity.length(),"\nhorizontalspeed:",Vector3(body.velocity.x,0,body.velocity.z).length())
