using Godot;
using EXVAG.Common;
namespace EXVAG.Figure;

[GlobalClass]
public partial class PlayerModelAnimator : Node3D
{
	class FootState
	{
		public Vector3 PlantPosition;

		public Vector3 StepStartPosition;

		public Vector3 TargetPosition;

		public bool IsPlanted;

		public float StepProgress;
	}

	[ExportCategory("References")]
	[ExportGroup("Limbs")]
	[ExportSubgroup("Central")]
	[Export] public Limb3D Head { get; private set; }
	[Export] public Limb3D Torso { get; private set; }
	[Export] public Limb3D Hip { get; private set; }
	[ExportSubgroup("Arms")]
	
	[Export] public Limb3D LeftArm { get; private set; }
	[Export] public Limb3D RightArm { get; private set; }
	[ExportSubgroup("Legs")]
	[Export] public Limb3D LeftLeg { get; private set; }
	[Export] public Limb3D RightLeg { get; private set; }
	[ExportSubgroup("Feet")]
	[Export] public Limb3D LeftFoot { get; private set; }
	[Export] public RayCast3D LeftFootRay { get; private set; }
	[Export] public Limb3D RightFoot { get; private set; }
	[Export] public RayCast3D RightFootRay { get; private set; }
	[ExportGroup("PlayerData")]
	[Export] public RayCast3D HandRay { get; private set; }
	[Export] public RayCast3D EyeRay { get; private set; }
	[Export] public Node3D HipAlignmentReference { get; private set; }
	[Export] public EnhancedCharacterBody3D PlayerCharacterBody { get; private set; }
	[ExportCategory("Tuning")]
	[Export] public float TorsoHeadWeight { get; private set; } = 0.3f;
	[Export] public float HipTurnSpeed { get; private set; } = 16f;
	[Export] public bool DefaultRightHanded { get; private set; } = true;
	[Export] public float FootMaxDistance { get; private set; } = 1.38f; // its like michael jackson
	[Export] public float FootReplantPredictionStrength { get; private set; } = 6.4f;
	[Export] public double FootStepDuration { get; private set; } = 0.26;
	[Export] public float PlayerFootVerticalOffset { get; private set; } = -1.0f;
	private FootState _leftFootState = new();
	private FootState _rightFootState = new();
	public override void _Ready()
	{
		_leftFootState.PlantPosition = LeftFoot.GlobalPosition;
		_rightFootState.PlantPosition = RightFoot.GlobalPosition;

		_leftFootState.IsPlanted = true;
		_rightFootState.IsPlanted = true;
	}
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
			return GetFarOfRay(ray);
		}
	}
	public static Vector3 GetFarOfRay(RayCast3D ray)
	{
		ray.ForceUpdateTransform();
		ray.ForceRaycastUpdate();
		return ray.GlobalPosition
			+ (ray.GlobalBasis.Z
			* -1000f);
	}
	/// <summary>
	/// Pulls a limb toward a position
	/// </summary>
	/// <param name="limb">Limb to aim.</param>
	/// <param name="position">Position to aim at.</param>
	/// <param name="strength">Strength of pull to the target.</param>
	public static void AimLimb(Node3D limb, Vector3 position, float strength = 0.225f) 
	{
		Basis desired = new Transform3D(
			Basis.Identity, 
			limb.GlobalPosition)
			.LookingAt(
			position, 
			Vector3.Up).Basis;
		Quaternion current = limb.GlobalBasis.GetRotationQuaternion();
		Quaternion target = desired.GetRotationQuaternion();
		limb.GlobalBasis = new Basis(current.Slerp(target, strength)); 
	}

	private void AimArmTowardTarget()
	{
		Node3D arm = DefaultRightHanded ? RightArm : LeftArm;
		AimLimb(arm, GetFarOfRay(HandRay), 0.4f);
	}
	/// <summary>
	/// Moves the hip towards the direction the player is facing. Generally aligns with the camera.
	/// </summary>
	/// <param name="delta"></param>
	private void PullHipRotation(double delta)
	{
		float dt = (float)delta;
		//Quaternion hipQuaternion = Hip.Basis.GetRotationQuaternion();
		//Quaternion pivotQuaternion = HipAlignmentReference.Basis.GetRotationQuaternion();

		Hip.GlobalBasis = Hip.GlobalBasis
			.Orthonormalized()
			.Slerp(HipAlignmentReference.GlobalBasis.Orthonormalized(),
			dt * HipTurnSpeed
			);
	}
	/// <summary>
	/// Sets the torso to a rotation between the head and the hip. 
	/// </summary>
	private void AlignTorso()
	{
		Quaternion hipQuaternion = Hip.Basis.GetRotationQuaternion();
		Quaternion headQuaternion = Head.Basis.GetRotationQuaternion();
		Quaternion torsoQuaternion = hipQuaternion.Slerp(
			headQuaternion, 
			TorsoHeadWeight); 
		
		Torso.Quaternion = torsoQuaternion.Normalized();
	}
	private void TickFoot(double delta, 
		FootState footState, 
		Limb3D leg,
		RayCast3D ray)
	{
		if (footState.IsPlanted)
		{
			if (CheckFootNeedsNewStep(footState.PlantPosition))
				BeginStep(footState, ray);
		}
		else
		{
			UpdateStep(delta, footState, ray);
		}

		AimLimb(leg, footState.PlantPosition);
	}

	private bool CheckFootNeedsNewStep(Vector3 plantPosition)
	{
		Vector3 playerFootPosition = new(
			PlayerCharacterBody.GlobalPosition.X,
			PlayerCharacterBody.GlobalPosition.Y + PlayerFootVerticalOffset,
			PlayerCharacterBody.GlobalPosition.Z);

		return (plantPosition.DistanceTo(playerFootPosition) > FootMaxDistance);
		//if (plantPosition.DistanceTo(playerFootPosition) > FootMaxDistance)
		//{
		//	GD.Print($"Foot should re-plant - {Time.GetTicksMsec()}");
		//	return true;
		//}
		//else
		//{
		//	return false;
		//}
	}
	private Vector3 FindTargetFootPlant(RayCast3D ray)
	{
		Vector3 playerFootPosition = new(
			PlayerCharacterBody.GlobalPosition.X,
			PlayerCharacterBody.GlobalPosition.Y + PlayerFootVerticalOffset,
			PlayerCharacterBody.GlobalPosition.Z);
		Vector3 predictedPosition = (PlayerCharacterBody.HorizontalVelocity * FootReplantPredictionStrength)
			+ PlayerCharacterBody.GlobalPosition;
		//ray.GlobalPosition = predictedPosition;
		ray.ForceUpdateTransform();
		ray.ForceRaycastUpdate();

		//if (ray.IsColliding())
			//return ray.GetCollisionPoint();

		return predictedPosition;
	}
	private void BeginStep(
	FootState foot,
	RayCast3D ray)
	{
		foot.IsPlanted = false;
		foot.StepProgress = 0f;
		foot.TargetPosition = FindTargetFootPlant(ray);
		foot.StepStartPosition = foot.PlantPosition;
	}
	private void UpdateStep(double delta,
		FootState foot,
		RayCast3D ray)
	{
		foot.StepProgress += (float)(delta / FootStepDuration);
		float t = Mathf.Clamp(foot.StepProgress, 0f, 1f);

		foot.PlantPosition =
		foot.StepStartPosition.Lerp(
			foot.TargetPosition,
			t);
		if (t >= 1f)
		{
			foot.IsPlanted = true;
			foot.PlantPosition = foot.TargetPosition;
		}
	}
	private void AimHeadTowardTarget() => AimLimb(Head, GetFarOfRay(EyeRay));
	public override void _PhysicsProcess(double delta)
	{
		AimArmTowardTarget();
		AimHeadTowardTarget();
		PullHipRotation(delta);
		AlignTorso();

		TickFoot(delta, _leftFootState, LeftLeg, LeftFootRay);
		TickFoot(delta, _rightFootState, RightLeg, RightFootRay);
	}

}
