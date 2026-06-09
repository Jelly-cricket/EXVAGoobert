using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class JumpComponent : Component
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; set; }
	[Export] public InputComponent Input { get; set; }

	[ExportCategory("Ability")]
	[Export] public float JumpPower { get; set; } = 6.4f;
	[Export] public double JumpCooldownDuration { get; set; } = 0.3;
	[Export] public double CoyoteDuration { get; set; } = 0.17;
	[Export] public double JumpBufferDuration { get; set; } = 0.34;

	private double _jumpCooldownTimer = 0;
	private double _coyoteTimer = 0;
	private double _jumpBufferTimer = 0;

	public override void _PhysicsProcess(double delta)
	{
		TickTimers(delta);
		RefreshCoyote();
		CheckInputWantsJump();
		TryJump();
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
	private void CheckInputWantsJump()
	{
		if (Input.GetBounce())
		{
			_jumpBufferTimer = JumpBufferDuration;
		}
	}
	public void TryJump()
	{
		if (_jumpCooldownTimer > 0)
		{
			return;
		}
		if (_coyoteTimer < 0)
		{
			return;
		}
		if (_jumpBufferTimer > 0)
		{
			DoJump();
		}
	}
	private void DoJump()
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