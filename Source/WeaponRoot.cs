using EXVAG.Component.Input;
using Godot;

namespace EXVAG.Component.Item;
[GlobalClass]
public partial class WeaponRoot : Node3D
{
	public CharacterInputSignals InputSource { get; set; }
	public StatComponent AmmoSource { get; set; }
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
