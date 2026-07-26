using Godot;
using System;
namespace EXVAG.Animation;

[GlobalClass]
public partial class PlayerModelAnimator : Node3D
{
	[ExportCategory("References")]
	[ExportGroup("Limbs")]
	[ExportSubgroup("Central")]
	[Export] public Node3D Head { get; set; }
	[Export] public Node3D Torso { get; set; }
	[Export] public Node3D Hip { get; set; }
	[ExportSubgroup("Arms")]
	
	[Export] public Node3D LeftArm { get; set; }
	[Export] public Node3D RightArm { get; set; }
	[ExportSubgroup("Legs")]
	[Export] public Node3D LeftLeg { get; set; }
	[Export] public Node3D RightLeg { get; set; }
	[ExportSubgroup("Feet")]
	[Export] public Node3D LeftFoot { get; set; }
	[Export] public RayCast3D LeftFootRay { get; set; }
	[Export] public Node3D RightFoot { get; set; }
	[Export] public RayCast3D RightFootRay { get; set; }
	[ExportGroup("PlayerData")]
	[Export] public RayCast3D HandRay { get; set; }
	[Export] public RayCast3D EyeRay { get; set; }
	[Export] public Node3D HipAlignmentReference { get; set; }
	[ExportCategory("Tuning")]
	[Export] public bool DefaultRightHanded { get; set; } = true;
	[Export] public double FootPlantShiftDelay { get; set; } = 1.55;
	[Export] public double FootReplantDelay { get; set; } = 0.35;
	[Obsolete("Unimplemented.")] private int _limpTier = 0; // Unused

	/// <summary>
	/// Update the arms to aim towards the target position
	/// </summary>
	public static Vector3 GetTargetOfRay(RayCast3D ray)
	{
		ray.ForceUpdateTransform();
		ray.ForceRaycastUpdate();
		if (ray.IsColliding())
		{
			return ray.GetCollisionPoint();
		}
		else
		{
			return ray.GlobalPosition
				+ (ray.GlobalBasis.Z
				* -1000f);
		}
	}
	/// <summary>
	/// Points an arm at a position
	/// </summary>
	/// <param name="limb">Limb to aim.</param>
	/// <param name="position">Position to aim at.</param>
	public static void AimLimb(Node3D limb, Vector3 position) => limb.LookAt(position,Vector3.Up);
	public void AimArmTowardTarget()
	{
		Node3D arm = DefaultRightHanded ? RightArm : LeftArm;
		AimLimb(arm, GetTargetOfRay(HandRay));
	}
	public void AlignHip() => Hip.Basis = HipAlignmentReference.Basis;
	public void AlignTorso()
	{
		Quaternion hipQuaternion = Hip.Basis.GetRotationQuaternion();
		Quaternion headQuaternion = Head.Basis.GetRotationQuaternion();
		Quaternion torsoQuaternion = hipQuaternion.Slerp(headQuaternion, 0.3f);
		
		Torso.Quaternion = torsoQuaternion.Normalized();
	}

	public void AimHeadTowardTarget() => AimLimb(Head, GetTargetOfRay(EyeRay));
	public override void _PhysicsProcess(double delta)
	{
		AimArmTowardTarget();
		AimHeadTowardTarget();
		AlignHip();
		AlignTorso();
	}


}
