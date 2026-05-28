extends InputCo
class_name PlayerIOCo

func get_dir() -> Vector3:
	var plain = Input.get_vector(&"move_forward",&"move_backward",&"move_left",&"move_right")
	return Vector3(plain.y,0,plain.x)

func get_bounce() -> bool:
	return Input.is_action_just_pressed(&"move_bounce")
