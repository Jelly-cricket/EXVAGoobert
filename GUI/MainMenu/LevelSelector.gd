extends Control
class_name LevelSelector

@export var Levels : Array[LevelFace]

@export var DropDown : OptionButton
@export var ConfirmButton : Button

func setup() -> void :
	DropDown.clear()
	for face in Levels:
		DropDown.add_item(face.Spice.Title)
	ConfirmButton.pressed.connect(load_selected)
	
func _ready() -> void : 
	setup()
	
func load_selected() -> void :
	print("I wanna load %s" % str(get_selected_face().LevelScene.resource_path))
	ClientLevelManager.QueueThreadedLoadSceneAsLevel(get_selected_face().LevelScene.resource_path)

func get_selected_face() -> LevelFace :
	var d_id = DropDown.get_selected_id()
	return Levels[d_id]
