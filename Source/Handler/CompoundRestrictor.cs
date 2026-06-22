using EXVAG.Component;
using Godot;
using Godot.Collections;
using System.Collections.Generic;

namespace EXVAG.Handler;

[GlobalClass]
public partial class CompoundRestrictor : UsageRestrictor
{
	[ExportCategory("References")]
	[Export] public Array<UsageRestrictor> Restrictors { get; set; }

	public override bool QueryAvailability(StatComponent statSource, float amount)
	{
		List<UsageRestrictor> _optimizedRestrictors = [.. Restrictors]; // squish it to c# list so its faster
		for (int i = 0; i < _optimizedRestrictors.Count; i++)
		{
			if (!QueryAvailability(statSource, amount))
			{
				return false;
			}
		}
		return true;
	}
}
