extends Component
class_name LifeCo

@export_category("References")
@export var _loadout : LoadoutCo

@export_category("Amounts")
@export var _base_max : float = 100 ## If amount goes over this value, it will be adjusted to be this.
@export var _base_min : float = 0 ## When amount reaches this value, the signal death will be emitted.
@export var _base_start_value : float = 100 ## Starting number for amount.
@export var _base_min_buffer : float = 5 ## The minimum amount life can be. If non-fatal, non-killing damage brings life below this, life will be set to this.

@export_category("Regeneration")
@export var _base_regen_delay : float = 1.2 ## Time, in seconds, before healing begins after taking damage.
@export var _base_regen_rate : float = 38.5 ## Amount regenerated per second after the delay.
@export var _base_regen_snap : float = 85.0 ## Amount required to reach to snap the health to the max.

# on readies

@onready var max_life = _base_max
@onready var min_life = _base_min
@onready var start_value = _base_start_value
@onready var min_buffer = _base_min_buffer

@onready var regen_delay = _base_regen_delay
@onready var regen_rate = _base_regen_rate
@onready var regen_snap = _base_regen_snap

@onready var cur_life = start_value

var regen_cooldown = 0
var has_announced_regen_start := true

signal died
signal hurt
signal regen_started

func is_damage_fatal(amount : float) -> bool:
	return cur_life - amount > min_life


func do_regen(delta : float) -> void:
	if cur_life > max_life:
		cur_life = max_life 
		return
	if regen_cooldown > 0:
		regen_cooldown -= delta
		return
	if cur_life > regen_snap:
		cur_life = max_life
		
	cur_life += regen_rate * delta
	

func do_damage(amount : float, exchange : DamageExchange, fatal : bool = true) -> void:
	cur_life -= amount
	has_announced_regen_start = false
	regen_cooldown = regen_delay
	hurt.emit(amount)
	if cur_life > min_buffer:
		return
	if cur_life < min_life and fatal:
		died.emit(exchange)
		return
	# if non-fatal OR between min_life and min_buffer
	cur_life = min_buffer # bring up to min buffer
		
