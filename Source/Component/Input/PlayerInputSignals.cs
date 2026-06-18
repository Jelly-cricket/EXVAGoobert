using Godot;
namespace EXVAG.Component.Input;

[GlobalClass]
public partial class PlayerInputSignals : CharacterInputSignals
{
	public override void _Input(InputEvent e)
	{
		// Collapse
		if (e.IsActionPressed("move_collapse"))
			EmitSignal(SignalName.MoveCollapsePressed);

		if (e.IsActionReleased("move_collapse"))
			EmitSignal(SignalName.MoveCollapseReleased);

		// Bounce
		if (e.IsActionPressed("move_bounce"))
			EmitSignal(SignalName.MoveBouncePressed);

		if (e.IsActionReleased("move_bounce"))
			EmitSignal(SignalName.MoveBounceReleased);

		// Item fire
		if (e.IsActionPressed("item_fire"))
			EmitSignal(SignalName.ItemFirePressed);

		if (e.IsActionReleased("item_fire"))
			EmitSignal(SignalName.ItemFireReleased);

		// Item utility
		if (e.IsActionPressed("item_utility"))
			EmitSignal(SignalName.ItemUtilityPressed);

		if (e.IsActionReleased("item_utility"))
			EmitSignal(SignalName.ItemUtilityReleased);

		// Item zoom
		if (e.IsActionPressed("item_zoom"))
			EmitSignal(SignalName.ItemZoomPressed);

		if (e.IsActionReleased("item_zoom"))
			EmitSignal(SignalName.ItemZoomReleased);

		// Kits
		if (e.IsActionPressed("kit_aux"))
			EmitSignal(SignalName.KitAuxPressed);

		if (e.IsActionReleased("kit_aux"))
			EmitSignal(SignalName.KitAuxReleased);

		if (e.IsActionPressed("kit_special"))
			EmitSignal(SignalName.KitSpecialPressed);

		if (e.IsActionReleased("kit_special"))
			EmitSignal(SignalName.KitSpecialReleased);

		if (e.IsActionPressed("kit_super"))
			EmitSignal(SignalName.KitSuperPressed);

		if (e.IsActionReleased("kit_super"))
			EmitSignal(SignalName.KitSuperReleased);
	}
}