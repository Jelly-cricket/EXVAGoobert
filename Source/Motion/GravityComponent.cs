using Godot;
using EXVAG.Common;
namespace EXVAG.Motion;

[GlobalClass]
public partial class GravityComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }
	[Export] public StatComponent FallDamageReceiver { get; private set; }
	[ExportCategory("Fall Damage")]
	[Export] public bool FallDamageEnabled {  get; private set; } = true;
	/// <summary>
	/// Fall speed required to cause fall damage, in meters/sec. 
	/// </summary>
	[Export] public float FallDamageThreshold { get; private set; } = -8f;
	[Export] public float FallDamageBaseLine { get; private set; } = 5f;
	[Export] public float FallDamageMeterAddition { get; private set; } = 0.2f;
	[Export] public float FallDamageMeterMultiplication { get; private set; } = 1.1f;

	private bool _wasOnFloor;
	private Vector3 _gravity;
	private Vector3 _lastFallingVelocity;

	[Signal] public delegate void LandedEventHandler(Vector3 vel);
	public override void _Ready()
	{
		_gravity = Body.GetGravity();
	}
	public override void _PhysicsProcess(double delta)
	{
		TryFallDamage();
		ApplyGravity(delta);
	}
	public void ApplyGravity(double delta)
	{
		float dt = (float)delta;
		if (!Body.IsOnFloor())
		{
			_wasOnFloor = false;
			_lastFallingVelocity = Body.Velocity;
			Body.Velocity += _gravity * dt;
		}
		else
		{
			_wasOnFloor = true;
		}
	}
	public void TryFallDamage()
	{
		if (Body.IsOnFloor() && !_wasOnFloor) // if you are on the floor and were not the floor for the previous check
		{
			if (_lastFallingVelocity.Y < FallDamageThreshold)
			{
				float referenceVelocity = FallDamageThreshold - _lastFallingVelocity.Y;
				int iterationTarget = (int)referenceVelocity;
				float computedDamage = FallDamageBaseLine;
				for (int i = 0; i < iterationTarget; i++)
				{
					computedDamage *= FallDamageMeterMultiplication;
					computedDamage += FallDamageMeterAddition;
				}
				FallDamageReceiver.DrainAmount(computedDamage, true);
			}
			EmitSignal(SignalName.Landed, _lastFallingVelocity);
		}
	}
}

