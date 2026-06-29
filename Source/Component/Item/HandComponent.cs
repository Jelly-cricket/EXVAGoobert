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
	
	[ExportCategory("Items")]
	[Export] public PackedScene DefaultScene { get; set; }
	
	[ExportCategory("Aiming")]
	[Export] public float BaseDriftSpeed { get; set; }

	public WeaponRoot EquippedWeapon { get; private set; }

	public Quaternion DesiredAimAngle { get; private set; }
	public Quaternion DriftAimAngle { get; private set; }

	public override void _Ready()
	{
		//EquipScene(DefaultScene);

		DriftAimAngle = CharacterHand.GlobalBasis
			.GetRotationQuaternion()
			.Normalized();

		DesiredAimAngle = DriftAimAngle;
	}


	public void EquipScene(PackedScene itemScene)
	{
		Unequip();
		EquippedWeapon = itemScene.Instantiate<WeaponRoot>();
		CharacterHand.AddChild(EquippedWeapon);
		EquippedWeapon.EquipTo(InputSource, AmmoSource);
	}
	public void Unequip()
	{
		EquippedWeapon?.QueueFree();
	}

	public void UpdateDesiredAim()
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

		DesiredAimAngle = desired.Basis.GetRotationQuaternion().Normalized();
	}

	public void UpdateDriftedAim(double delta)
	{
		float t = Mathf.Clamp((float)(delta * BaseDriftSpeed), 0f, 1f);

		DriftAimAngle = DriftAimAngle
			.Slerp(DesiredAimAngle, t)
			.Normalized();

		CharacterHand.GlobalBasis = new Basis(DriftAimAngle);
	}

	public void UpdateAims(double delta)
	{
		UpdateDesiredAim();
		UpdateDriftedAim(delta);
		//GD.Print($"Desired: {DesiredAimAngle.Length()}");
		//GD.Print($"Drift: {DriftAimAngle.Length()}");
	}
	public override void _PhysicsProcess(double delta) 
	{
		UpdateAims(delta);
	}
}