using Godot;
using System;
using EXVAG.Common;
namespace EXVAG.Input;

[GlobalClass]
public partial class CameraControllerComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public Node3D BodyPivot { get; private set; }
	[Export] public Node3D CameraPivot { get; private set; }
	[Export] public Node3D CameraHook { get; private set; }
	[Export] public Camera3D ActualCamera { get; private set; }
	[ExportCategory("Base Sensitivities")]
	[Export] public float VerticalSensitivity { get; private set; } = 0.0022f;
	[Export] public float HorizontalSensitivity { get; private set; } = 0.0025f;
	[ExportCategory("Bounds")]
	[Export] public float UpperBoundDegrees { get; private set; } = 60f;
	[Export] public float LowerBoundDegrees { get; private set; } = -65f;
	[ExportCategory("Smoothing")]
	[Export] public float FocusDriftRotationSpeed { get; private set; } = 16;
	[Export] public float FocusDriftPositionSpeed { get; private set; } = 22;

	public float UpperBoundRadians => (float)Mathf.DegToRad(UpperBoundDegrees);
	public float LowerBoundRadians => (float)Mathf.DegToRad(LowerBoundDegrees);
	public static void CaptureMouse() => Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Captured;

	public static void ReleaseMouse() => Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Visible;

	public void ProcessMouseMotion(InputEventMouseMotion e)
	{
		BodyPivot.RotateY(
			-e.Relative.X * HorizontalSensitivity
		);

		float newPitch =
			CameraPivot.Rotation.X
			- e.Relative.Y * VerticalSensitivity;

		newPitch = Math.Clamp(
			newPitch,
			LowerBoundRadians,
			UpperBoundRadians
		);

		CameraPivot.Rotation = new Vector3(
			newPitch,
			CameraPivot.Rotation.Y,
			CameraPivot.Rotation.Z
		);
	}

	public void PullCamera(double delta)
	{
		float dt = (float)(delta);

		ActualCamera.GlobalPosition = ActualCamera.GlobalPosition.Lerp(
			CameraHook.GlobalPosition,
			dt * FocusDriftPositionSpeed
		);

		ActualCamera.GlobalBasis = ActualCamera.GlobalBasis
			.Orthonormalized()
			.Slerp(
				CameraHook.GlobalBasis.Orthonormalized(),
				dt * FocusDriftRotationSpeed
			);
	}
	public override void _Ready() => CaptureMouse();
	public override void _Process(double delta) => PullCamera(delta);
	public override void _UnhandledInput(InputEvent e)
	{
		if (e is InputEventMouseMotion passedEvent)
		{
			ProcessMouseMotion(passedEvent);
		}
	}

}
