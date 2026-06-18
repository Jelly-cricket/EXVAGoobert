using Godot;
using System;
using System.Diagnostics.Metrics;
namespace EXVAG.Item;

public partial class BaseItem : Resource
{
	[Export] public Flavor Spice { get; set; }
	[Export] public Godot.Collections.Array<ItemEffect> EffectArray { get; set; }
}