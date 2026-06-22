using Godot;
using EXVAG.Component;
namespace EXVAG.Handler;


[GlobalClass]
public abstract partial class UsageRestrictor : Resource
{
	public abstract bool QueryAvailability(StatComponent statSource, float amount);
}