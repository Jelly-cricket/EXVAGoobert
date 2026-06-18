using Godot;
using System;
namespace EXVAG.Modulator;

[GlobalClass]
public abstract partial class SimpleMod : BaseMod
{
	/// <summary>
	/// Operand's function changes depending on the type of modulation.
	/// </summary>
	[Export] public virtual float Operand { get; set; }
}
