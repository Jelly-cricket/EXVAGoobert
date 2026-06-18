using Godot;
using System;
namespace EXVAG.Modulator;

[GlobalClass]
public abstract partial class BaseMod : Resource
{
	public abstract float Modulate(float amount);
}
