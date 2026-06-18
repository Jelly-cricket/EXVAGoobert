using Godot;
namespace EXVAG.Component.Input;

[GlobalClass]
public partial class PlayerInputStream : CharacterInputStream
{
	public override Vector3 MoveWishDir
	{
		get
		{
			Vector2 plain = Godot.Input.GetVector(
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
	}

}