using EXVAG.Component;
using Godot;
using System;
namespace EXVAG.Component.Input;

[GlobalClass]
public partial class CameraControllerComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body { get; private set; }
	[Export] public Node3D Pivot { get; private set; }
	[ExportCategory("Sensitivities")]
	[Export] public float VerticalSensitivity { get; private set; } = 0.003f;
	[Export] public float HorizontalSensitivity { get; private set; } = 0.004f;
	[ExportCategory("Bounds")]
	[Export] public float UpperBoundDegrees { get; private set; } = 60f;
	[Export] public float LowerBoundDegrees { get; private set; } = -65f;

	public float UpperBoundRadians => (float)Mathf.DegToRad(UpperBoundDegrees);
	public float LowerBoundRadians => (float)Mathf.DegToRad(LowerBoundDegrees);
		
	public static void CaptureMouse()
	{
		Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Captured;
	}
	
	public static void ReleaseMouse()
	{
		Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Visible;
	}

	public void ProcessMouseMotion(InputEventMouseMotion @event)
	{
		Body.RotateY(
			-@event.Relative.X * HorizontalSensitivity
		);

		float newPitch =
			Pivot.Rotation.X
			- @event.Relative.Y * VerticalSensitivity;

		newPitch = Math.Clamp(
			newPitch,
			LowerBoundRadians,
			UpperBoundRadians
		);

		Pivot.Rotation = new Vector3(
			newPitch,
			Pivot.Rotation.Y,
			Pivot.Rotation.Z
		);
	}
	public override void _Ready()
	{
		CaptureMouse();
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion passedEvent)
		{
			ProcessMouseMotion(passedEvent);
		}
	}
	
}
