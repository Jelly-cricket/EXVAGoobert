extends Node
class_name GravityCo

@export var body : CharacterBody3D

@export var gravity_direction : Vector3 = Vector3.DOWN
@export var gravity_strength : float = 9.81

func _physics_process(delta: float) -> void:
	if !body.is_on_floor():
		body.velocity += gravity_direction * gravity_strength * delta
