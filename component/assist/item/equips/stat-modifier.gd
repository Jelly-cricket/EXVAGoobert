extends Node
class_name StatModifier

#region self life

func mod_life_cap(amount : float) -> float :
	return amount

func mod_life_regen(amount : float) -> float :
	return amount

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
