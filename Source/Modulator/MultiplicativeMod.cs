using System;
using Godot;
namespace EXVAG.Modulator;

[GlobalClass]
public abstract partial class MultiplicativeMod : SimpleMod
{
	public override float Modulate(float amount)
	{
		return amount * Operand;
	}
}
