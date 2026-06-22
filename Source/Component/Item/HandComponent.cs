using EXVAG.Component.Input;
using Godot;
namespace EXVAG.Component.Item;

[GlobalClass]
public partial class HandComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterInputSignals InputSource { get; set; }
	[Export] public StatComponent AmmoSource { get; set; }
	[Export] public Node3D CharacterHand { get; set; }
	[Export] public PackedScene DefaultScene { get; set; }

	public WeaponRoot EquippedWeapon { get; private set; }

	public void EquipScene(PackedScene itemScene)
	{
		Unequip();
		EquippedWeapon = itemScene.Instantiate<WeaponRoot>();
		CharacterHand.AddChild(EquippedWeapon);
		EquippedWeapon.EquipTo(InputSource,AmmoSource);
	}
	public void Unequip()
	{
		EquippedWeapon?.QueueFree();
	}
	public override void _Ready()
	{
		EquipScene(DefaultScene);
	}
}