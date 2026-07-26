using EXVAG.Common;
using EXVAG.Input;
using Godot;

namespace EXVAG.Item; 
[GlobalClass] public partial class HandComponent : BaseComponent 
{
	[ExportCategory("References")]
	[Export] public CharacterInputSignals InputSource { get; set; } 
	[Export] public StatComponent AmmoSource { get; set; } 
	[Export] public RayCast3D CharacterEyeline { get; set; } 
	[Export] public Node3D CharacterHand { get; set; } 
	[ExportCategory("Items")]
	[Export] public ItemDefinition DefaultItem { get; set; } 
	[ExportCategory("Aiming")]
	[Export] public float BaseDriftSpeed { get; set; } = 12; 
	[Signal] public delegate void ItemEquippedEventHandler(HoldableDefinition holdableItem); 
	[Signal] public delegate void ItemUnequippedEventHandler(); 
	[Signal] public delegate void ItemManuallyUnequippedEventHandler(); 
	public LogicalHoldableRuntime EquippedHoldable { get; private set; }
	/// <summary> 
	/// The actual direction of aiming. 
	/// </summary> 
	public Quaternion DesiredAimAngle { get; private set; }
	/// <summary> 
	/// A direction that lags behind DesiredAimAngle
	/// </summary> 
	public Quaternion DriftAimAngle { get; private set; } 
	public override void _Ready()
	{ 
		DriftAimAngle = CharacterHand.GlobalBasis
			.GetRotationQuaternion()
			.Normalized();
		DesiredAimAngle = DriftAimAngle; 
	}
	public override void _PhysicsProcess(double delta) => UpdateAims(delta);
	/// <summary> 
	/// Equip an item from HoldableDefinition. 
	/// </summary> 
	/// <param name="holdableItem">
	/// Holdable Item Definition
	/// </param> 
	public void EquipItem(HoldableDefinition holdableItem) 
	{
		Unequip();
		EquippedHoldable = holdableItem.LogicalRuntime.Instantiate<LogicalHoldableRuntime>(); 
		CharacterHand.AddChild(EquippedHoldable); 
		EquippedHoldable.EquipTo(InputSource, AmmoSource); 
		EmitSignal(SignalName.ItemEquipped,holdableItem);
	} 
	/// <summary> 
	/// Remove the EquippedHoldable. 
	/// </summary>
	public void Unequip()
	{
		if (EquippedHoldable is not null) 
		{
			EquippedHoldable.QueueFree();
			EquippedHoldable = null; 
			EmitSignal(SignalName.ItemUnequipped); 
		}
	}
	/// <summary> 
	/// Get the target point of the aim. 
	/// </summary> 
	/// <returns>
	/// Ray intersect point, or 1000 meters away in the direction of aim if there is no intersection.
	/// </returns>
	public Vector3 GetEyeTargetPoint() 
	{
		Vector3 target; 
		if (CharacterEyeline.IsColliding()) 
		{
			target = CharacterEyeline.GetCollisionPoint(); 
		}
		else 
		{
			target = CharacterEyeline.GlobalPosition
				+ CharacterEyeline.GlobalBasis.Z
				* -1000f;
		}
		return target;
	} 
	/// <summary> 
	/// Updates the desired aim rotation to face the target aim point. 
	/// </summary> 
	public void UpdateDesiredAim() 
	{
		Vector3 target = GetEyeTargetPoint(); 
		Transform3D desired = CharacterHand.GlobalTransform.LookingAt
			( 
			target, 
			Vector3.Up 
			); 
		DesiredAimAngle = desired.Basis
			.GetRotationQuaternion()
			.Normalized(); 
	}
	/// <summary> 
	/// Updates the drifted aim rotation, moving it towards the DesiredAimAngle by a bit depending on delta. 
	/// </summary> 
	/// <param name="delta">
	/// Delta time.
	/// </param> 
	public void UpdateDriftedAim(double delta) 
	{
		float t = Mathf.Clamp(
			(float)(delta * BaseDriftSpeed),
			0f, 
			1f
			); 
		DriftAimAngle = DriftAimAngle 
			.Slerp(DesiredAimAngle, t) 
			.Normalized(); 
		CharacterHand.GlobalBasis = new Basis(DriftAimAngle); 
	}
	/// <summary> 
	/// Update the desired and drifted aim. Simple pairing to make sure its in the right order. 
	/// </summary> 
	/// <param name="delta">
	/// Delta time.
	/// </param> 
	public void UpdateAims(double delta) 
	{ 
		UpdateDesiredAim();
		UpdateDriftedAim(delta);
		//GD.Print($"Desired: {DesiredAimAngle.Length()}"); 
		//GD.Print($"Drift: {DriftAimAngle.Length()}");
	}
}