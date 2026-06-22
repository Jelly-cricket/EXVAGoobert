using EXVAG.Component.Input;
using Godot;
namespace EXVAG.Component.Item;

[GlobalClass]
public partial class HandComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterInputSignals InputSource { get; set; }
	[Export] public StatComponent AmmoSource { get; set; }
	[Export] public RayCast3D CharacterEyeline { get; set; }
	[Export] public Node3D CharacterHand { get; set; }
	[Export] public PackedScene DefaultScene { get; set; }

	public WeaponRoot EquippedWeapon { get; private set; }

	public void EquipScene(PackedScene itemScene)
	{
		Unequip();
		EquippedWeapon = itemScene.Instantiate<WeaponRoot>();
		CharacterHand.AddChild(EquippedWeapon);
		EquippedWeapon.EquipTo(InputSource,AmmoSource);
	}
	public void Unequip()
	{
		EquippedWeapon?.QueueFree();
	}
	public override void _Ready()
	{
		EquipScene(DefaultScene);
	}
	public void DriftAim(double delta)
	{
		Vector3 target;

		if (CharacterEyeline.IsColliding())
		{
			target = CharacterEyeline.GetCollisionPoint();
		}
		else
		{
			target = CharacterEyeline.GlobalPosition +
					 CharacterEyeline.GlobalBasis.Z * -1000f;
		}

		Transform3D desired = CharacterHand.GlobalTransform.LookingAt(
			target,
			Vector3.Up
		);

		CharacterHand.GlobalBasis = CharacterHand.GlobalBasis.Slerp(
			desired.Basis,
			(float)(delta * 15.0)
		);
	}
	public override void _PhysicsProcess(double delta)
	{
		DriftAim(delta);
	}
}