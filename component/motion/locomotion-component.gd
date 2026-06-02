extends Component
class_name LocoCo
@export_category("References")
@export var body : CharacterBody3D
@export var input : InputCo
@export_category("Base Speeds")
@export var base_ground_speed : float = 7.8
@export var base_air_speed : float = 3.2
@export var base_ground_accel : float = 14.6
@export var base_air_accel : float = 1.4
@export var base_frict : float = 22.4

@onready var world_gravity = body.get_gravity()

var wish_dir : Vector3
var wish_vel : Vector3

#region Acting declarations and helpers
@onready var acting_ground_speed : float = base_ground_speed
@onready var acting_air_speed : float = base_air_speed

func reset_ground_speed() -> bool: ## Returns true if it was already matching, false otherwise.
	var same = acting_ground_speed == base_ground_speed
	acting_ground_speed = base_ground_speed
	return same
func reset_air_speed() -> bool: ## Returns true if it was already matching, false otherwise.
	var same = acting_air_speed == base_air_speed
	acting_air_speed = base_air_speed
	return same

@onready var acting_ground_accel : float = base_ground_accel
@onready var acting_air_accel : float = base_air_accel

func reset_ground_accel() -> bool: ## Returns true if it was already matching, false otherwise.
	var same = acting_ground_accel == base_ground_accel
	acting_ground_accel = base_ground_accel
	return same
func reset_air_accel() -> bool: ## Returns true if it was already matching, false otherwise.
	var same = acting_air_accel == base_air_accel
	acting_air_accel = base_air_accel
	return same

@onready var acting_frict : float = base_frict

func reset_frict() -> bool: ## Returns true if it was already matching, false otherwise.
	var same = acting_frict == base_frict
	acting_frict = base_frict
	return same
	
#endregion

func _physics_process(delta : float) -> void:
	_find_wishes()
	_figure_horizontal_movement(delta)
	_do_gravity(delta)
	
	body.move_and_slide()
	
func _do_gravity(delta : float) -> void:
	body.velocity += world_gravity * delta
	
func _find_wishes() -> void:
	var local_input = input.get_dir()

	# convert local movement into world-space relative to body rotation
	wish_dir = (
		body.global_transform.basis.x * local_input.x +
		body.global_transform.basis.z * local_input.z
	).normalized()

	wish_vel = wish_dir * base_ground_speed
	
func _figure_horizontal_movement(delta):
	var vel = body.velocity
	var horizontal = Vector3(vel.x, 0, vel.z)
	var wish_speed = wish_vel.length()

	if body.is_on_floor():
		horizontal = friction_applied(horizontal, delta)
		horizontal = accelerate(
			horizontal,
			wish_dir,
			wish_speed,
			base_ground_accel,
			delta,
			wish_speed
		)
	else:
		var air_cap = base_air_speed

		horizontal = accelerate(
			horizontal,
			wish_dir,
			wish_speed,
			base_air_accel,
			delta,
			air_cap # prevent too fast
		)

	body.velocity.x = horizontal.x
	body.velocity.z = horizontal.z
func accelerate(vel: Vector3, wish_dir: Vector3, wish_speed: float, accel: float, delta: float, max_speed: float) -> Vector3:
	var current_speed = vel.dot(wish_dir)
	var add_speed = wish_speed - current_speed

	if add_speed <= 0:
		return vel

	var accel_speed = accel * wish_speed * delta

	# cap speed gain
	if accel_speed > add_speed:
		accel_speed = add_speed

	# hard cap
	var new_speed = current_speed + accel_speed
	if new_speed > max_speed:
		accel_speed = max_speed - current_speed

	return vel + wish_dir * accel_speed
	
func friction_applied(vel: Vector3, delta: float) -> Vector3:
	var speed = vel.length()
	if speed < 0.001:
		return Vector3.ZERO

	var drop = speed * base_frict * delta
	var new_speed = max(speed - drop, 0)

	if new_speed != speed:
		new_speed /= speed
		vel *= new_speed

	return vel
