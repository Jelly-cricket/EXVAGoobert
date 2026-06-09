using Godot;
using System;

public partial class GravityComponent : Component
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; set; }
	[ExportCategory("Gravity")]
	[Export] public Vector3 GravityDirection { get; set; } = Vector3.Down;
	[Export] public float GravityStrength { get; set; } = 9.81f;

	public override void _PhysicsProcess(double delta)
	{
		Body.Velocity += (GravityDirection * GravityStrength)
			* (float)delta;
	}
}
