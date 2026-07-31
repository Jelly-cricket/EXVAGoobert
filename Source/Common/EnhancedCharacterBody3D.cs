using Godot;
namespace EXVAG.Common;


/// <summary>
/// CharacterBody3D extension with extra properties and various things.
/// </summary>
[GlobalClass]
public partial class EnhancedCharacterBody3D : CharacterBody3D
{

	/// <summary>
	/// Velocity vector with Y set to zero.
	/// </summary>
	public Vector3 HorizontalVelocity => new(Velocity.X, 0, Velocity.Z);
}
