using Godot;
using Godot.Collections;
namespace EXVAG.Common;

[GlobalClass]
public partial class Flavor : Resource
{
	[Export] public string Title { get; set; }
	[Export] public string Description { get; set; }
	[Export] public Texture2D Icon { get; set; }
	[Export] public Array<StringName> LooseTags { get; set; }
}
