extends StatModifier
class_name LoadoutCo

@export_category("References")
@export var hand : HandCo
@export_category("Slots")
@export_group("Outfit")
@export var _starter_head : HeadItem
@export var _starter_body : BodyItem
@export var _starter_feet : FeetItem
@export_group("Kit")
@export var _starter_aux : AuxKitItem ## Self-charging, no mana usage.
@export var _starter_special : SpecialKitItem ## Low impact ability, uses little mana.
@export var _starter_super : SuperKitItem ## High impact ability, uses full mana.
@export_group("Armaments")
@export var _starter_weapon : PrimaryItem ## Primary weapon.
@export var _starter_mantle : MantleItem ## Active in squid form.

@onready var cur_head : HeadItem = _starter_head
@onready var cur_body : BodyItem = _starter_body
@onready var cur_legs : LegsItem = _starter_legs

func equip_kit(kit : KitItem) -> bool :
	if kit is AuxKitItem :
		pass
