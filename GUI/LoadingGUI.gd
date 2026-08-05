extends Panel
class_name LoadingGUI

@export var Progress : ProgressBar
@export var TipLabel : Label

func UpdateProgress(newAmount) -> void:
	Progress.value = newAmount
