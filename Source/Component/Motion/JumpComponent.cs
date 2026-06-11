using Godot;
using System;
using System.Runtime.CompilerServices;
namespace EXVAG.Component.Motion;

[GlobalClass]
public partial class JumpComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }
	[Export] public Input.InputComponent Input { get; private set; }

	[ExportCategory("Ability")]
	[Export] public float JumpPower { get; private set; } = 6.4f;
	[Export] public double JumpCooldownDuration { get; private set; } = 0.3;
	[Export] public double CoyoteDuration { get; private set; } = 0.17;
	[Export] public double JumpBufferDuration { get; private set; } = 0.34;

	public bool CanJump => _jumpCooldownTimer == 0 && _coyoteTimer > 0 && _jumpBufferTimer > 0;

	private double _jumpCooldownTimer = 0;
	private double _coyoteTimer = 0;
	private double _jumpBufferTimer = 0;

	public override void _PhysicsProcess(double delta)
	{
		TickTimers(delta);
		RefreshCoyote();
		CheckQueueJump();
		TryJump();
	}
	private void CheckQueueJump()
	{
		if (Input.Bounce)
		{
			_jumpBufferTimer = JumpBufferDuration;
		}
	}
	private void TryJump()
	{
		if (CanJump)
		{
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

    Body.Velocity = new Vector3(
        Body.Velocity.X,
        JumpPower,
        Body.Velocity.Z
    );
}

	private void RefreshCoyote()
	{
		if (Body.IsOnFloor())
		{
			_coyoteTimer = CoyoteDuration;
		}
	}
}