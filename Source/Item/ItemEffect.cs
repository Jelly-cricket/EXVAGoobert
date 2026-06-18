using Godot;
using System;
namespace EXVAG.Item;

public abstract partial class ItemEffect : Resource
{
	[Export] public Flavor Spice { get; set; }

	public abstract void Apply();
	public abstract void Remove();
}