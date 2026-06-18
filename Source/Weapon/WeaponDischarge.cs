using Godot;
using System;

namespace EXVAG.Weapon;
public abstract partial class WeaponDischarge : Area3D
{
	[Export] public float InitialSpeed { get; set; }
	[Export] public float SpeedFalloff { get; set;}
	[Export] public float GravityStrength { get; set; }
}
