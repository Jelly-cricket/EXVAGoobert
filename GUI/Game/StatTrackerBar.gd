extends ProgressBar
class_name StatTrackerBar

@export_category("References")
@export var StatSource : StatComponent
@export var DeltaDisplay : DeltaLabel
@export var AmountDisplay : Label

func _ready() -> void : 
	AttachSignals()
	min_value = StatSource.MinAmount
	max_value = StatSource.MaxAmount
	

func AttachSignals() -> void :
	StatSource.Absorbed.connect(OnAbsorbed)
	StatSource.Drained.connect(OnDrained)

func OnAbsorbed(change : float, previous : float, new_amount : float) -> void :
	ChangeTo(new_amount)
func OnDrained(change : float, previous : float, new_amount : float) -> void :
	ChangeTo(new_amount)
	
func ChangeTo(amount : float) -> void :
	DeltaDisplay.QueueChange(amount - value)
	value = amount
	var formatter_amts = { "amt" : "%.1f" % amount, "mxamt" : "%.1f" % max_value}
	AmountDisplay.text = "{amt} / {mxamt}".format(formatter_amts)
