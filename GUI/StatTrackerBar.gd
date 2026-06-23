extends ProgressBar
class_name StatTrackerBar

@export_category("References")
@export var StatSource : StatComponent

func _ready() -> void : 
	AttachSignals()

func AttachSignals() -> void :
	StatSource.Absorbed.connect(OnAbsorbed)
	StatSource.Drained.connect(OnDrained)

func OnAbsorbed(change : float, previous : float, new_amount : float) -> void :
	ChangeTo(new_amount)
func OnDrained(change : float, previous : float, new_amount : float) -> void :
	ChangeTo(new_amount)
	
func ChangeTo(amount : float) -> void :
	value = amount
