using Godot;
using System;
namespace EXVAG.Component.Stat;

[GlobalClass]
public partial class BaseStat : BaseComponent
{
	[ExportCategory("Limits")]
	[Export] public virtual float MaxAmount { get; private set; } = 100f;
	[Export] public virtual float MinAmount { get; private set; } = 0f;
	[ExportCategory("Regeneration")]
	[Export] public virtual double RegenDelay { get; private set; } = 2;
	[Export] public float RegenRate { get; private set; } = 5f;
	[ExportCategory("Protections")]
	[Export] public virtual float UpperSnap { get; private set; } = 100f; // if regen is active and value is above this, set value to max.
	[Export] public	virtual float LowerSnap { get; private set; } = 0f; // if regen is active and value is below this, set value to this.
	[Export] public virtual float MinBounce { get; private set; } = 1f; // if value is below this but above MinAmount, set value to this.
	public bool CanRegenerate => Math.Round(_regenCooldown, 3) == 0 && Amount > MinAmount;
	public float Amount
	{
		get
		{
			return _amount;
		}
		private set
		{
			_amount = value;
		}
	}
	private float _amount;
	private double _regenCooldown = 0;

	[Signal] public delegate void DrainEventHandler(
		float change,
		float previous,
		float current
	);
	[Signal] public delegate void EmptyEventHandler();
	[Signal] public delegate void FilledEventHandler();

	public bool CanAbsorbAmount(float desired)
	{
		return Amount + desired < MaxAmount;
	}
	public bool CanDrainAmount(float desired)
	{
		return Amount - desired > MinAmount;
	}
	public void ClampToBounds()
	{
		Amount = Mathf.Clamp(Amount, MinAmount, MaxAmount);
	}
	public virtual void DrainAmount(float desired, bool exhaustive)
	{
		float _hypothetical = Amount - desired; // the final amount assuming all checks are passed
		_regenCooldown = RegenDelay;
		if (exhaustive || _hypothetical > MinAmount)
		{
			Amount = _hypothetical;
			return;
		}
		// if _hypothetical < MinAmount and not exhaustive
		Amount = MinBounce;
	}
	private void TickRegeneration(double delta)
	{
		if (!CanRegenerate) return;

		Amount = RegenRate * (float)delta; // Standard regeneration.

		// Then snap if it is at a certain amount.
		if (Amount < LowerSnap)
		{
			Amount = LowerSnap;
		}
		if (Amount > UpperSnap)
		{
			Amount = MaxAmount;
		}
	}
	private void TickRegenCooldown(double delta)
	{
		_regenCooldown = Mathf.Max(_regenCooldown - delta,0);
	}

}
