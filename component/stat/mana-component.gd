extends Component
class_name ManaCo

@export_category("References")
@export var _loadout : LoadoutCo

@export_category("Amounts")
@export var _base_max : float = 100
@export var _base_min : float = 0
@export var _base_start_value : float = 10

@export_category("Regeneration")
@export var _base_regen_delay : float = 0.2 ## Time, in seconds, before mana regen begins after being used.
@export var _base_regen_rate : float = 0.16 ## Amount regenerated per second after the delay.
@export var _base_charge_mult : float = 1 ## amount charges effectiveness is multiplied by.

# on readies

@onready var max_mana = _base_max
@onready var min_mana = _base_min
@onready var start_value = _base_start_value

@onready var regen_delay = _base_regen_delay
@onready var regen_rate = _base_regen_rate
@onready var charge_mult = _base_charge_mult

@onready var cur_mana = start_value

var regen_cooldown = 0

signal spent
signal exhausted
signal filled

func can_consume(amount : float) -> bool:
	return cur_mana - amount > min_mana
	
func do_regen(delta : float) -> void:
	if cur_mana >= max_mana:
		cur_mana = max_mana
		filled.emit()
		return
	if regen_cooldown > 0:
		regen_cooldown -= delta
		return
	
	cur_mana += regen_rate * delta
	
func grant_charge(amount : float) -> void:
	cur_mana += amount * charge_mult
	regen_cooldown = regen_delay
	if cur_mana >= max_mana:
		cur_mana = max_mana
		filled.emit()
		
func consume(amount : float) -> void:
	cur_mana -= amount
	spent.emit(amount)
	if cur_mana < min_mana:
		exhausted.emit()
		cur_mana = min_mana
		return
	return
