extends Node
class_name BounceCo

@export_category("References")
@export var input : InputCo
@export var body : CharacterBody3D
@export_category("Stats")
@export var jump_power : float = 6.4
@export var jump_cooldown_amount : float = 0.3
@export var coyote_amount : float = 0.17
@export var jump_buffer_amount : float = 0.34


var jump_cooldown_timer : float = 0
var coyote_timer : float = 0
var jump_buffer_timer : float = 0

func _physics_process(delta : float) -> void:
	_tick_timers(delta)
	_refresh_coyote()
	_try_jump()
	do_jump()
	
func _tick_timers(delta : float) -> void:
	if jump_cooldown_timer >= 0:
		jump_cooldown_timer -= delta
	if jump_buffer_timer >= 0:
		jump_buffer_timer -= delta
	if coyote_timer >= 0:
		coyote_timer -= delta
		
func _try_jump() -> void:
	if input.get_bounce():
		jump_buffer_timer = jump_buffer_amount
	
func do_jump() -> void:
	if jump_cooldown_timer > 0:
		return
	if coyote_timer < 0:
		return
	if jump_buffer_timer > 0:
		impulse_jump()
		
func impulse_jump() -> void:
	coyote_timer = 0
	body.velocity.y += jump_power
	
func _refresh_coyote() -> void:
	if body.is_on_floor():
		coyote_timer = coyote_amount
