extends Node
class_name OutfitItem ## Base class for equippable items, with mod_* functions to get modified amounts for different stats.

@export var spice : Flavor

func get_flavor() -> Flavor:
	return spice

#region taking damage

func mod_hurt(amount : float) -> float :
	return amount
	
func mod_move_slow(amount : float) -> float :
	return amount

#endregion

#region doing damage

func mod_crit_chance(amount : float) -> float :
	return amount

func mod_roller_damage(amount : float) -> float :
	return amount
	
func mod_pistol_damage(amount : float) -> float :
	return amount

func mod_submach_damage(amount : float) -> float :
	return amount

func mod_railer_damage(amount : float) -> float :
	return amount

func mod_mantle_damage(amount : float) -> float :
	return amount
	
#endregion

#region self life

func mod_life_cap(amount : float) -> float :
	return amount

func mod_life_regen(amount : float) -> float :
	return life_regen_modulator.applied(amount)

#endregion

#region self mana

func mod_mana_cap(amount : float) -> float : 
	return amount

func mod_mana_regen(amount : float) -> float :
	return amount
	
#endregion

#region self movement 

func mod_max_speed(amount : float) -> float : 
	return amount
	
func mod_accel(amount : float) -> float : 
	return amount

func mod_frict(amount : float) -> float :
	return amount
	
#endregion
