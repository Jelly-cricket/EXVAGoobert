using EXVAG.Component.Input;
using EXVAG.Component.Stat;
using Godot;

namespace EXVAG.Weapon;
public abstract partial class WeaponRoot : Node3D
{
	public CharacterInputSignals InputSource { get; set; }
	public BaseStat AmmoSource { get; set; }
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

	public void EquipTo(CharacterInputSignals input,BaseStat ammo)
	{
		InputSource = input;
		AmmoSource = ammo;
		ConnectActionSignals();
	}
	public override void _ExitTree()
	{
		DetachActionSignals();
	}

	public abstract void OnPrimaryFirePressed();
	public abstract void OnUtilityFirePressed();
	public abstract void OnPrimaryFireReleased();
	public abstract void OnUtilityFireReleased();
}
