using Godot;
using System;
using EXVAG.Common;
using EXVAG.Input;
namespace EXVAG.Motion;

[GlobalClass]
public partial class LocomotionComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }
	[Export] public Node3D FrameOfReference { get; private set; }
	[Export] public CharacterInputStream InputStream { get; private set; }

	[ExportCategory("Base Speeds")]
	[Export] public float BaseGroundSpeed { get; private set; } = 7.8f;
	[Export] public float BaseAirSpeed { get; private set; } = 3.2f;
	[Export] public float BaseGroundAccel { get; private set; } = 14.6f;
	[Export] public float BaseAirAccel { get; private set; } = 1.4f;
	[ExportCategory("Physics")]
	[Export] public float BaseFriction { get; private set; } = 22.4f;

	private Vector3 _wishDir;

	public float ActingGroundSpeed { get; private set; }
	public float ActingAirSpeed { get; private set; }

	public float ActingGroundAccel { get; private set; }
	public float ActingAirAccel { get; private set; }

	public float ActingFriction { get; private set; }

	public override void _Ready()
	{

		ActingGroundSpeed = BaseGroundSpeed;
		ActingAirSpeed = BaseAirSpeed;

		ActingGroundAccel = BaseGroundAccel;
		ActingAirAccel = BaseAirAccel;

		ActingFriction = BaseFriction;
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;


		FindWishes();
		FigureHorizontalMovement(dt);

		Body.MoveAndSlide();
	}
	private void FindWishes()
	{
		Vector3 localInput = InputStream.MoveWishDir;

		_wishDir = (
			FrameOfReference.GlobalTransform.Basis.X * localInput.X +
			FrameOfReference.GlobalTransform.Basis.Z * localInput.Z
		).Normalized();
	}

	private void FigureHorizontalMovement(float delta)
	{

		float wishSpeed = Body.IsOnFloor()
			? BaseGroundSpeed
			: BaseAirSpeed;

		Vector3 vel = Body.Velocity;
		Vector3 horizontal = new(vel.X, 0f, vel.Z);

		if (Body.IsOnFloor())
		{
			horizontal = FrictionApplied(horizontal, delta);

			horizontal = Accelerated(
				horizontal,
				_wishDir,
				wishSpeed,
				BaseGroundAccel,
				delta
			);
		}
		else
		{
			horizontal = Accelerated(
				horizontal,
				_wishDir,
				MathF.Min(wishSpeed, BaseAirSpeed),
				BaseAirAccel,
				delta
			);
		}

		vel.X = horizontal.X;
		vel.Z = horizontal.Z;
		//GD.Print(
		//	$"Speed: {horizontal.Length():F2} " +
		//	$"WishDir: {_wishDir} " +
		//	$"Dot: {horizontal.Dot(_wishDir):F2}"
		//);
		Body.Velocity = vel;

	}

	public static Vector3 Accelerated
	(
		Vector3 velocity,
		Vector3 wishDirection,
		float wishSpeed,
		float acceleration,
		float delta
	)
	{
		float currentSpeed = velocity.Dot(wishDirection);
		float addSpeed = wishSpeed - currentSpeed;

		if (addSpeed <= 0)
		{
			return velocity;
		}

		float accelSpeed = acceleration * wishSpeed * delta;
		if (accelSpeed > addSpeed)
		{
			accelSpeed = addSpeed;
		}
		return velocity + wishDirection * accelSpeed;
	}

	private Vector3 FrictionApplied(Vector3 vel, float delta)
	{
		float speed = vel.Length();

		if (speed < 0.001f)
			return Vector3.Zero;

		float drop = speed * BaseFriction * delta;
		float newSpeed = Mathf.Max(speed - drop, 0f);

		if (newSpeed != speed)
		{
			newSpeed /= speed;
			vel *= newSpeed;
		}

		return vel;
	}
}