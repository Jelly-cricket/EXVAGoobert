using EXVAG.Input;
using EXVAG.Common;
using Godot;
namespace EXVAG.Item;
[GlobalClass]
public partial class LogicalHoldableRuntime : Node
{
	public Node3D AimReference { get; set; } // Given on instantiate
	public CharacterInputSignals InputSource { get; set; } // Given on instantiate.
	public StatComponent AmmoSource { get; set; } // Given on instantiate.
	public void ConnectActionSignals()
	{
		InputSource.ItemFirePressed += OnPrimaryFirePressed;
		InputSource.ItemFireReleased += OnPrimaryFireReleased;
		InputSource.ItemUtilityPressed += OnUtilityFirePressed;
		InputSource.ItemUtilityReleased += OnUtilityFireReleased;
	}
	public void DetachActionSignals()
	{
		InputSource.ItemFirePressed -= OnPrimaryFirePressed;
		InputSource.ItemFireReleased -= OnPrimaryFireReleased;
		InputSource.ItemUtilityPressed -= OnUtilityFirePressed;
		InputSource.ItemUtilityReleased -= OnUtilityFireReleased;
	}

	public void EquipTo(CharacterInputSignals input, StatComponent ammo)
	{
		InputSource = input;
		AmmoSource = ammo;
		ConnectActionSignals();
	}
	public override void _ExitTree()
	{
		DetachActionSignals();
	}

	public void OnPrimaryFirePressed()
	{

	}
	public void OnUtilityFirePressed()
	{

	}
	public void OnPrimaryFireReleased()
	{

	}
	public void OnUtilityFireReleased()
	{

	}
}
