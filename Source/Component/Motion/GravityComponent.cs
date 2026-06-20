using Godot;
using System;
namespace EXVAG.Component.Motion;

[GlobalClass]
public partial class GravityComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }

	private Vector3 _gravity;

	public override void _Ready()
	{
		_gravity = Body.GetGravity();
	}
	public override void _PhysicsProcess(double delta)
	{
		Body.Velocity += _gravity
			* (float)delta;
	}
}
