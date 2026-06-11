using Godot;
using System;
namespace EXVAG.Component.Motion;

[GlobalClass]
public partial class GravityComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }
	[ExportCategory("Gravity")]
	[Export] public Vector3 GravityDirection { get; private set; } = Vector3.Down;
	[Export] public float GravityStrength { get; private set; } = 9.81f;

	public override void _PhysicsProcess(double delta)
	{
		Body.Velocity += (GravityDirection * GravityStrength)
			* (float)delta;
	}
}
