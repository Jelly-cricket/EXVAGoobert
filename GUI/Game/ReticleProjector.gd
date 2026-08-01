extends Control
class_name ReticleProjector

@export var AimRay : RayCast3D
@export var MuzRay : RayCast3D
@export var Camera : Camera3D

var PrimaryReticleCenter : Vector2 = Vector2(0,0)
var SecondaryReticleCenter : Vector2 = Vector2(0,0)
var BlockageReticleCenter : Vector2 = Vector2(0,0)

func _process(delta : float) -> void:
	var aim = CheckRayFar(AimRay)
	var muz = CheckRayFar(MuzRay)
	var blk = CheckRayTarget(MuzRay)
	PrimaryReticleCenter = Camera.unproject_position(aim)
	SecondaryReticleCenter = Camera.unproject_position(muz)
	BlockageReticleCenter = Camera.unproject_position(blk)
	queue_redraw()
	
func CheckRayTarget(ray : RayCast3D) -> Vector3:
	ray.force_update_transform()
	ray.force_raycast_update()
	if ray.is_colliding():
		return ray.get_collision_point()
	else:
		return CheckRayFar(ray)
		
func CheckRayFar(ray : RayCast3D) -> Vector3:
	return ray.global_position + ray.global_basis.z * - 1000


func _draw() -> void:
	draw_circle(PrimaryReticleCenter,12,Color.BLACK,false)
	draw_circle(SecondaryReticleCenter,8,Color.WHITE,false)
	draw_circle(BlockageReticleCenter,4,Color.RED,false)

func DrawCrosshair(pos : Vector3, cfg : CrosshairConfig) -> void:
	pass
