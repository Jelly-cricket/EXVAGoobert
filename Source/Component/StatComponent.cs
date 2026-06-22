using Godot;
using System;
namespace EXVAG.Component;

[GlobalClass]
public partial class StatComponent : BaseComponent
{
	[ExportCategory("Initialize")]
	[Export] public virtual float StartAmount { get; private set; } = 50;
	[ExportCategory("Limits")]
	[Export] public virtual float MaxAmount { get; private set; } = 100f;
	[Export] public virtual float MinAmount { get; private set; } = 0f;
	[ExportCategory("Regeneration")]
	[Export] public virtual bool RegenerateFromEmpty { get; private set; }
	[Export] public virtual double RegenDelay { get; private set; } = 2;
	[Export] public float RegenRate { get; private set; } = 5f;
	[ExportCategory("Protections")]
	[Export] public virtual float UpperSnap { get; private set; } = 100f; // if regen is active and value is above this, set value to max.
	[Export] public	virtual float LowerSnap { get; private set; } = 0f; // if regen is active and value is below this, set value to this.
	[Export] public virtual float MinBounce { get; private set; } = 1f; // if value is below this but above MinAmount, set value to this.
	public bool CanRegenerate => _regenCooldown <= 0 && (Amount > MinAmount || RegenerateFromEmpty) && Amount < MaxAmount;
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

	[Signal] public delegate void DrainedEventHandler(float change, float previous, float current);
	[Signal] public delegate void AbsorbedEventHandler(float change, float previous, float current);
	[Signal] public delegate void EmptiedEventHandler();
	[Signal] public delegate void FilledEventHandler();

	public override void _Ready()
	{
		MinBounce = Mathf.Clamp(MinBounce, MinAmount, MaxAmount);

		Amount = Mathf.Clamp(StartAmount, MinAmount, MaxAmount);
	}
	public override void _PhysicsProcess(double delta)
	{
		TickRegeneration(delta);
		TickRegenCooldown(delta);
	}

	public bool CanAbsorbAmount(float desired)
	{
		return Amount + desired <= MaxAmount;
	}
	public bool CanDrainAmount(float desired)
	{
		return Amount - desired >= MinAmount;
	}
	protected void SetAmount(float newAmount)
	{
		float previous = Amount;

		Amount = Mathf.Clamp(newAmount, MinAmount, MaxAmount);

		float delta = Amount - previous;

		if (delta > 0)
			EmitSignal(SignalName.Absorbed, delta, previous, Amount);

		if (delta < 0)
			EmitSignal(SignalName.Drained, -delta, previous, Amount);

		if (previous > MinAmount && Amount <= MinAmount)
			EmitSignal(SignalName.Emptied);

		if (previous < MaxAmount && Amount >= MaxAmount)
			EmitSignal(SignalName.Filled);
	}
	public virtual void DrainAmount(float desired, bool exhaustive)
	{
		_regenCooldown = RegenDelay;

		float hypothetical = Amount - desired;

		if (exhaustive || hypothetical > MinAmount)
		{
			SetAmount(hypothetical);
		}
		else
		{
			SetAmount(MinBounce);
		}
	}
	public virtual void AbsorbAmount(float desired)
	{
		SetAmount(Amount + desired);
	}
	private void TickRegeneration(double delta)
	{
		if (!CanRegenerate)
			return;

		AbsorbAmount(RegenRate * (float)delta);

		if (Amount < LowerSnap)
		{
			SetAmount(LowerSnap);
		}

		if (Amount > UpperSnap)
		{
			SetAmount(MaxAmount);
		}
	}
	private void TickRegenCooldown(double delta)
	{
		_regenCooldown = Mathf.Max(_regenCooldown - delta,0);
	}

}
