using Godot;
namespace EXVAG.Component.Input;

[GlobalClass]
public abstract partial class CharacterInputStream : BaseComponent // Inputs that are more requested rather than actions. Like locomotion! That's a constant input stream, which is what this component is for.
{
	public virtual Vector3 MoveWishDir => Vector3.Zero;
	public virtual Vector2 LookDelta => Vector2.Zero;

}