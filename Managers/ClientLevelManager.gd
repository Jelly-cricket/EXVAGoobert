extends Node

@export var LevelRoot : Node3D
@export var LoadingScreen : LoadingGUI



var _loading : bool = false
var _loadingScreenActive : bool = false
var _queuedLevel : StringName

var LoadProgress : Array = []
var RegisteredLevel : LevelFace

func RequestLevelChange(level : LevelFace) -> void :
	RegisteredLevel = level
	_queueThreadedLoadSceneAsLevel(level.LevelScene.resource_path)

func _ready() -> void:
	_endLoading()

func _unloadCurrentLevel() -> void:
	var currentLevel = LevelRoot.get_children(false)
	for childNode in currentLevel:
		childNode.queue_free()

func _loadSceneAsLevel(level : PackedScene) -> void: ## This function does NOT unload the currently loaded level.
	var instance = level.instantiate()
	LevelRoot.add_child(instance)
	
func _queueThreadedLoadSceneAsLevel(levelPath : StringName) -> void: 
	if _loading:
		return
	
	if not FileAccess.file_exists(levelPath):
		push_error("level does not exist: " + levelPath)
		return
	
	ResourceLoader.load_threaded_request(levelPath)
	
	_beginLoading()
	_queuedLevel = levelPath
	_loading = true

func _changeLevelToPackedScene(level : PackedScene):
	_unloadCurrentLevel()
	_loadSceneAsLevel(level) 
	
func _evaluateLoad():
	if not _loading:
		return
	var status = ResourceLoader.load_threaded_get_status(
		_queuedLevel,
		LoadProgress
	)
	
	match status:
		ResourceLoader.THREAD_LOAD_IN_PROGRESS:
			pass
		
		ResourceLoader.THREAD_LOAD_LOADED:
			_loading = false
			
			var level : PackedScene = ResourceLoader.load_threaded_get(
				_queuedLevel
			) as PackedScene
			
			if level == null:
				push_error("loaded resource is not a PackedScene: " + _queuedLevel)
				_endLoading()
				return
				
			_changeLevelToPackedScene(level)
			_endLoading()
			
		ResourceLoader.THREAD_LOAD_FAILED:
			_loading = false
			push_error("failed to load level: " + _queuedLevel)
			
func _beginLoading() -> void: 
	_loadingScreenActive = true
	LoadingScreen.visible = true
	LoadingScreen.UpdateProgress(0.0)


func _endLoading() -> void:
	_loadingScreenActive = false
	LoadingScreen.visible = false
	
		

func _process(_delta : float) -> void:
	_evaluateLoad()
	if _loading:
		LoadingScreen.UpdateProgress(LoadProgress[0] * 100)
