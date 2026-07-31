using Godot;
using EXVAG.Common;

using EXVAG.Motion;
using System.Runtime.InteropServices.JavaScript;
using System;
namespace EXVAG.Source.VFX;

[GlobalClass]
public partial class CameraViewEffectComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public Camera3D Camera { get; private set; }
	[Export] public StatComponent LifeStat { get; private set; }
	[Export] public GravityComponent GravityHandler { get; private set; }
	[ExportCategory("Tuning")]
	[Export] public bool ReactToFall { get; private set; } = true;
	[Export] public float FallReactThreshold { get; private set; } = 5.4f;
	[Export] public float FallReactLimit { get; private set; } = 0.45f;
	[Export] public float FallReactMultiplier { get; private set; } = 0.08f;
	[Export] public bool ReactToDamage { get; private set; } = true;
	[Export] public float DamageReactThreshold { get; private set; } = 2.5f;
	[Export] public float DamageReactMultiplier { get; private set;  } = 0.03f;
	public override void _Ready()
	{
		LifeStat.Drained += OnLifeDrained;
		GravityHandler.Landed += OnPlayerLanded;
	}

	public void OnLifeDrained(float change, float previous, float current)
	{
		float pole = (float)GD.RandRange(0, 1);
		if (pole == 0)
		{
			pole = -1;
		}

		if (Mathf.Abs(change) > DamageReactThreshold)
		{
			Camera.RotateObjectLocal(new Vector3(0, 0, 1), Mathf.Abs(change)
				* pole
				* DamageReactMultiplier
				);
		}
	}
	public void OnPlayerLanded(Vector3 lastFallingVector)
	{
		if (Mathf.Abs(lastFallingVector.Y) > FallReactThreshold)
		{
			Vector3 newPos = new(Camera.GlobalPosition.X,
				Camera.GlobalPosition.Y + Mathf.Clamp(lastFallingVector.Y * FallReactMultiplier, -FallReactLimit, FallReactLimit), 
				Camera.GlobalPosition.Z);
			Camera.GlobalPosition = newPos;
		}
	}
}
