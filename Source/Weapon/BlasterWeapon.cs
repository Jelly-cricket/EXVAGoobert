using EXVAG.Source.Component.Item;
using Godot;
using System;

namespace EXVAG.Weapon;
public partial class BlasterWeapon : WeaponRoot
{
	[Export] public float AmmoConsumption { get; set; }
	[Export] public PackedScene ProjectileScene { get; set; }


	public override void OnPrimaryFirePressed()
	{
		
	}
	public override void OnPrimaryFireReleased() 
	{

	}
	public override void OnUtilityFirePressed()
	{
		
	}
	public override void OnUtilityFireReleased()
	{
		
	}
	public virtual void TriggerPrimaryFire()
	{
		
	}
	public virtual void SpawnProjectile(Vector3 direction)
	{
		var projectile = ProjectileScene.Instantiate();
		GetTree().Root.AddChild(projectile);
		if (projectile is Node3D)
		{
			((Node3D)projectile).Rotation = direction;
		}
		else
		{
			GD.Print("Error: Projectile scene is not 3D!");
		}
	}

}
