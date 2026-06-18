using System;
using Godot;
namespace EXVAG.Modulator;

[GlobalClass]
public partial class AdditiveMod : SimpleMod
{
	public override float Modulate(float amount)
	{
		return amount + Operand;
	}
}
