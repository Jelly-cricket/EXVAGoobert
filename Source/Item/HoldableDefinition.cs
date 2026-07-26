using Godot;


namespace EXVAG.Item;

public partial class HoldableDefinition : ItemDefinition
{
	[ExportCategory("Runtime Scenes")]
	[Export] public PackedScene LogicalRuntime { get; set; } // Should have LogicalHoldableRoot as the root node
	[Export] public PackedScene VisualRuntime { get; set; } // Should have VisualHoldableRoot as the root node
}