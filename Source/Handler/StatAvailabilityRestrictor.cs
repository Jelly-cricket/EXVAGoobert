using Godot;
using EXVAG.Component;
namespace EXVAG.Handler;

[GlobalClass]
public partial class StatAvailabilityRestrictor : UsageRestrictor
{
	public override bool QueryAvailability(StatComponent statSource, float amount)
	{
		return statSource.CanAbsorbAmount(amount);
	}
}