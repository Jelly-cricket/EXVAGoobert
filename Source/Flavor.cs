using Godot;
using Godot.Collections;
namespace EXVAG.Component.Item;

[GlobalClass]
public partial class Flavor : Resource
{
	[Export] public string Title { get; set; }
	[Export] public string Description { get; set; }
	[Export] public Image Icon { get; set; }
	[Export] public Array<StringName> LooseTags { get; set; }
}
