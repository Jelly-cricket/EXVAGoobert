using Godot;

public abstract partial class InputComponent : Component
{
    public virtual Vector3 GetDir()
    {
		return Vector3.Zero;
    }

    public virtual bool GetBounce()
    {
        return false;
    }

    public virtual bool GetFire()
    {
        return false;
    }

    public virtual bool GetUtility()
    {
        return false;
    }
}