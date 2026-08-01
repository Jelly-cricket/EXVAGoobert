using Godot;
namespace EXVAG.Common;

[GlobalClass]
public partial class LevelManager : Node
{
	public static LevelManager Instance { get; private set; }
	public override void _Ready()
	{
		base._Ready();

		// make sure it a single ton
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			// if another instance tries to spawn, destroy it!!!
			QueueFree();
		}
	}
}
