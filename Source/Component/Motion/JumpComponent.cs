using Godot;
using System;
using System.Runtime.CompilerServices;
namespace EXVAG.Component.Motion;

[GlobalClass]
public partial class JumpComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }
	[Export] public Input.CharacterInputSignals SignalSource { get; private set; }

	[ExportCategory("Restrictions")]
	[Export] public double JumpCooldownDuration { get; private set; } = 0.3;
	[Export] public double CoyoteDuration { get; private set; } = 0.17;
	[Export] public double JumpBufferDuration { get; private set; } = 0.34;
	[ExportCategory("Jumping Power")]
	[Export] public float JumpPower { get; private set; } = 6.4f;
	[Export] public float SkipPower { get; private set; } = 1.2f;
	[Export] public int AirJumpLimit { get; private set; } = 0;
	public bool CanJump => _jumpCooldownTimer == 0 && _coyoteTimer > 0 && _jumpBufferTimer > 0;

	private int _airJumpsRemaining = 0;
	private double _jumpCooldownTimer = 0;
	private double _coyoteTimer = 0;
	private double _jumpBufferTimer = 0;


	public override void _Ready()
	{
		SignalSource.MoveBouncePressed += OnBouncePressed;
	}
	public override void _PhysicsProcess(double delta)
	{
		TickTimers(delta);
		RefreshCoyote();
		TryJump();
	}
	private void OnBouncePressed()
	{
		QueueJump();
	}
	private void QueueJump()
	{
		_jumpBufferTimer = JumpBufferDuration;
	}
	private void TryJump()
	{
		if (_jumpCooldownTimer > 0)
			return;

		bool groundedJump =
			_coyoteTimer > 0 &&
			_jumpBufferTimer > 0;

		bool airJump =
			_jumpBufferTimer > 0 &&
			!Body.IsOnFloor() &&
			_airJumpsRemaining > 0;

		if (groundedJump)
		{
			JumpImpulse();
		}
		else if (airJump)
		{
			_airJumpsRemaining--;
			JumpImpulse();
		}
	}
	private void TickTimers(double delta)
	{
		_jumpCooldownTimer = Math.Max(
			0,
			_jumpCooldownTimer - delta
		);
		_coyoteTimer = Math.Max(
			0,
			_coyoteTimer - delta
		);
		_jumpBufferTimer = Math.Max(
			0,
			_jumpBufferTimer - delta
		);
	}

	private void JumpImpulse()
	{
		_jumpBufferTimer = 0;
		_jumpCooldownTimer = JumpCooldownDuration;
		_coyoteTimer = 0;
	
		float _newX = Body.Velocity.X;
		float _newY = Body.Velocity.Y;
		float _newZ = Body.Velocity.Z;

		Vector3 _skipBoost = Body.Velocity.Normalized() * SkipPower;

		_newX += _skipBoost.X;
		_newY += JumpPower;
		_newZ += _skipBoost.Z;

		Body.Velocity = new Vector3(
			_newX,
			_newY,
			_newZ
		);
	}

	private void RefreshCoyote()
	{
		if (Body.IsOnFloor())
		{
			_coyoteTimer = CoyoteDuration;
			_airJumpsRemaining = AirJumpLimit;
		}
	}
}