using Godot;
using System;

public partial class FlyingState : IPlayerState
{
    [Export] public float flyInertia = .05f;
    public void Enter(PlayerController player)
    {
        player.yInertia = true;
        player.inertia = flyInertia;
        player.cameraInertia.Y = 0f;
        player.targetMovement = Vector3.Zero;
    }
    public void HandleInput(PlayerController player, InputEvent @event)
    {
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards").Normalized();
        float verticalDir = Input.GetAxis("move_crouch", "move_jump");
        float speed = Input.IsActionPressed("move_run") ? player.flySpeed : player.walkSpeed;
        Vector3 direction = ((player.camera.GlobalTransform.Basis.Z * inputDir.Y)
        + (player.camera.GlobalTransform.Basis.X * inputDir.X) + (player.GlobalTransform.Basis.Y * verticalDir)).Normalized();

        player.targetMovement = direction * speed;

        if (@event.IsActionPressed("action_cancel"))
        {
            player.ChangeState(new AirborneState());
        }
    }
    public void Update(PlayerController player, double delta)
    {
        
    }
    public void Exit(PlayerController player)
    {
        player.yInertia = false;
        player.cameraInertia = PlayerController.defaultCameraInertia;
        player.inertia = PlayerController.defaultInertia;
    }
}