using Godot;
namespace EXVAG.Item;

public partial class VisualHoldableRuntime : Node
{
	[Export] public AnimationPlayer WorldModelAnimator { get; set; }
	
	public LogicalHoldableRuntime LogicalCounterpart { get; set; }
}