extends Node

@export var LevelRoot : Node3D
@export var LoadingScreen : LoadingGUI

var loadProgress : Array = []
var loading : bool = false
var loadingScreenActive : bool = false
var queuedLevel : StringName

func _ready() -> void:
	EndLoading()

func UnloadCurrentLevel() -> void:
	var currentLevel = LevelRoot.get_children(false)
	for childNode in currentLevel:
		childNode.queue_free()

func LoadSceneAsLevel(level : PackedScene) -> void: ## This function does NOT unload the currently loaded level.
	var instance = level.instantiate()
	LevelRoot.add_child(instance)
	
func QueueThreadedLoadSceneAsLevel(levelPath : StringName) -> void:
	if loading:
		return
	
	if not FileAccess.file_exists(levelPath):
		push_error("level does not exist: " + levelPath)
		return
	
	ResourceLoader.load_threaded_request(levelPath)
	
	BeginLoading()
	queuedLevel = levelPath
	loading = true

	
func ChangeLevel(level : PackedScene):
	UnloadCurrentLevel()
	LoadSceneAsLevel(level)
	
func EvaluateLoad():
	if not loading:
		return
	var status = ResourceLoader.load_threaded_get_status(
		queuedLevel,
		loadProgress
	)
	
	match status:
		ResourceLoader.THREAD_LOAD_IN_PROGRESS:
			pass
		
		ResourceLoader.THREAD_LOAD_LOADED:
			loading = false
			
			var level : PackedScene = ResourceLoader.load_threaded_get(
				queuedLevel
			) as PackedScene
			
			if level == null:
				push_error("loaded resource is not a PackedScene: " + queuedLevel)
				EndLoading()
				return
				
			ChangeLevel(level)
			EndLoading()
			
		ResourceLoader.THREAD_LOAD_FAILED:
			loading = false
			push_error("failed to load level: " + queuedLevel)
			
func BeginLoading() -> void:
	loadingScreenActive = true
	LoadingScreen.visible = true
	LoadingScreen.UpdateProgress(0.0)


func EndLoading() -> void:
	loadingScreenActive = false
	LoadingScreen.visible = false
	
		

func _process(_delta : float) -> void:
	EvaluateLoad()
	if loading:
		LoadingScreen.UpdateProgress(loadProgress[0] * 100)
