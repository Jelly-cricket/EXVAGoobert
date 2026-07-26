using Godot;
using EXVAG.Common;
namespace EXVAG.Input;

[GlobalClass]
public abstract partial class CharacterInputSignals : BaseComponent
{
	[Signal] public delegate void MoveCollapsePressedEventHandler();
	[Signal] public delegate void MoveCollapseReleasedEventHandler();

	[Signal] public delegate void MoveBouncePressedEventHandler();
	[Signal] public delegate void MoveBounceReleasedEventHandler();

	[Signal] public delegate void ItemFirePressedEventHandler();
	[Signal] public delegate void ItemFireReleasedEventHandler();

	[Signal] public delegate void ItemUtilityPressedEventHandler();
	[Signal] public delegate void ItemUtilityReleasedEventHandler();

	[Signal] public delegate void ItemZoomPressedEventHandler();
	[Signal] public delegate void ItemZoomReleasedEventHandler();

	[Signal] public delegate void KitAuxPressedEventHandler();
	[Signal] public delegate void KitAuxReleasedEventHandler();

	[Signal] public delegate void KitSpecialEquipPressedEventHandler(); // Equip item
	[Signal] public delegate void KitSpecialEquipReleasedEventHandler(); // Equip item

	[Signal] public delegate void KitSpecialDeployPressedEventHandler();  // Quick use
	[Signal] public delegate void KitSpecialDeployReleasedEventHandler(); // Quick use

	[Signal] public delegate void KitSuperPressedEventHandler();
	[Signal] public delegate void KitSuperReleasedEventHandler();

}