using Godot;
using System;

namespace EXVAG.Weapon;
public partial class BlasterWeapon : WeaponRoot
{
	[Export] float AmmoConsumption { get; set; }
	[Export] PackedScene ProjectileScene { get; set; }

	
	public override void PrimaryFirePressed()
	{
		// pew pew
	}
	public virtual void FireProjectile(Vector3 direction)
	{
		var projectile = ProjectileScene.Instantiate<WeaponDischarge>();
		GetTree().CurrentScene.AddChild(projectile);
	}

}
