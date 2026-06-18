using EXVAG.Component;
using System;
using Godot;
namespace EXVAG.Component;


[GlobalClass]
internal partial class PlayerSkillSetComponent : BaseComponent
{
	[Export] public Motion.JumpComponent Jump { get; set; }
	[Export] public Motion.LocomotionComponent Locomotion { get; set; }
	[Export] public Stat.BaseStat Life { get; set; }
	[Export] public Stat.BaseStat Mana { get; set; }
	
}
