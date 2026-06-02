extends Marker3D
class_name CameraMan

@export_category("References")
@export var body : CharacterBody3D
@export var camera : Marker3D

@export_category("Control")
@export var sensitivity : float = 0.004
@export var camera_vertical_bound_down : float = -60
@export var camera_vertical_bound_up : float = 60

@onready var ready_waiter : float = 0.2

var rotation_x : float = 0

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _process(delta: float) -> void:
	if ready_waiter > 0:
		ready_waiter -= delta

func _unhandled_input(event: InputEvent) -> void:
	if ready_waiter > 0:
		return
	if event is InputEventMouseMotion:

		body.rotate_y(-event.relative.x * sensitivity)

		rotation_x -= event.relative.y * sensitivity

		rotation_x = clamp(
			rotation_x,
			deg_to_rad(camera_vertical_bound_down),
			deg_to_rad(camera_vertical_bound_up)
		)
		camera.rotation.x = rotation_x
