using Godot;
using System;

namespace EXVAG.Weapon;
public abstract partial class WeaponRoot : Node3D
{
	[Export] public Component.Input.PlayerInputSignals InputSource { get; set; }
	[Export] public Component.Stat.BaseStat AmmoSource { get; set; }

	public void ConnectSignals()
	{
		InputSource.ItemFirePressed += OnPrimaryFirePressed;
		InputSource.ItemFireReleased += OnPrimaryFireReleased;
		InputSource.ItemUtilityPressed += OnUtilityFirePressed;
		InputSource.ItemUtilityReleased += OnUtilityFireReleased;
	}
	public void DetachSignals()
	{
		InputSource.ItemFirePressed -= OnPrimaryFirePressed;
		InputSource.ItemFireReleased -= OnPrimaryFireReleased;
		InputSource.ItemUtilityPressed -= OnUtilityFirePressed;
		InputSource.ItemUtilityReleased -= OnUtilityFireReleased;
	}

	public abstract void OnPrimaryFirePressed();
	public abstract void OnUtilityFirePressed();
	public abstract void OnPrimaryFireReleased();
	public abstract void OnUtilityFireReleased();
}
