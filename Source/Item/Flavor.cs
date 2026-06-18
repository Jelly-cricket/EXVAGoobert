using Godot;
using Godot.Collections;
using System;
namespace EXVAG.Item;
public partial class Flavor : Resource
{
	public string Title { get; set; }
	public string Description { get; set; }
	public Image Icon { get; set; }
	public Array<StringName> LooseTags { get; set; }
}
