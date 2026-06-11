using Godot;
namespace EXVAG.Component.Input;

[GlobalClass]
public abstract partial class InputComponent : BaseComponent
{
	public virtual Vector3 MoveWishDir => Vector3.Zero;

	public virtual bool Bounce => false;

	public virtual bool Fire => false;
	public virtual bool Utility => false;

	public virtual bool KitAuxiliary => false;

	public virtual bool KitSpecial => false;

	public virtual bool KitSuper => false;
}