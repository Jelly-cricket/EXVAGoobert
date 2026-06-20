using EXVAG.Component.Input;
using EXVAG.Weapon;
using Godot;
using EXVAG.Component.Stat;
namespace EXVAG.Component.Item;

public partial class HandComponent : BaseComponent
{
	[ExportCategory("References")]
	[Export] public CharacterInputSignals InputSource { get; set; }
	[Export] public BaseStat AmmoSource { get; set; }
	[Export] public Node3D CharacterHand { get; set;  }

	public WeaponRoot EquippedWeapon { get; private set; }

	public void EquipItem(PackedScene itemScene)
	{
		EquippedWeapon?.QueueFree();
		EquippedWeapon = itemScene.Instantiate<WeaponRoot>();
		CharacterHand.AddChild(EquippedWeapon);
		EquippedWeapon.EquipTo(InputSource,AmmoSource);
	}
}