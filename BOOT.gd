extends Node

@export var BootLevel : LevelFace

var splash_timer : float = 0.1
var boot_started : bool = false


func _process(delta : float) -> void:
	splash_timer -= delta
	
	if splash_timer > 0:
		return
	
	if boot_started:
		return
	
	boot_started = true
	
	ClientLevelManager.QueueThreadedLoadSceneAsLevel(
		BootLevel.LevelScene.resource_path
	)
