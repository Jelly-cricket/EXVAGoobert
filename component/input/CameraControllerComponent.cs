using Godot;
using System;

public partial class CameraControllerComponent : Node
{
	[ExportCategory("References")]
	[Export] public CharacterBody3D Body {  get; set; }
	[Export] public Node3D Pivot { get; set; }
	[ExportCategory("Sensitivities")]
	[Export] public float VerticalSensitivity { get; set; } = 0.35f;
	[Export] public float HorizontalSensitivity { get; set; } = 0.4f;
	[ExportCategory("Bounds")]
	[Export] public float UpperBoundDegrees { get; set; } = 60;
	[Export] public float LowerBoundDegrees { get; set; } = -65;

	public float UpperBoundRadians => (float)Mathf.DegToRad(UpperBoundDegrees);
	public float LowerBoundRadians => (float)Mathf.DegToRad(LowerBoundDegrees);
	private float rotationX = 0;
	private float rotationY = 0;public static void CaptureMouse()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public static void ReleaseMouse()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	
	
}
