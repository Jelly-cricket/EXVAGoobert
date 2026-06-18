using System;
using Godot;
using Godot.Collections;
namespace EXVAG.Modulator;

[GlobalClass]
public partial class CompoundMod : BaseMod
{
	[Export] public Array<BaseMod> ModArray { get; set; }
	public override float Modulate(float amount)
	{
		float currentAmount = amount;

		for (int i = 0; i < ModArray.Count; i++)
		{
			currentAmount = ModArray[i].Modulate(currentAmount);
		}
		return currentAmount;
	}
}