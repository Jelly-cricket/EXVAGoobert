using Godot;
public partial class PlayerInputComponent : InputComponent
{
    public override Vector3 GetDir()
    {
        Vector2 plain = Input.GetVector(
            "move_forward",
            "move_backward",
            "move_left",
            "move_right"
        );
		return new Vector3(
            plain.Y,
            0f,
            plain.X
        );
       
    }

    public override bool GetBounce()
    {
        return Input.IsActionJustPressed("move_bounce");
    }

    public override bool GetFire()
    {
        return Input.IsActionPressed("item_fire");
    }

    public override bool GetUtility()
    {
        return Input.IsActionPressed("item_utility");
    }
}