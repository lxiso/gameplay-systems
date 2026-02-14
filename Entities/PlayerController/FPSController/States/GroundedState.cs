using Godot;
using System;

public partial class GroundedState : IPlayerState
{
    public void Enter(PlayerController player)
    {
    }
    public void HandleInput(PlayerController player, InputEvent @event)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards").Normalized();
        float speed = Input.IsActionPressed("move_run") ? player.runSpeed : player.walkSpeed;
        player.targetMovement.X = direction.X * speed;
        player.targetMovement.Z = direction.Y * speed;
        player.targetMovement = player.targetMovement.Rotated(Vector3.Up, player.Rotation.Y);

        if (@event.IsActionPressed("move_jump"))
        {
            player.targetMovement.Y = player.jumpForce;
            player.ChangeState(new AirborneState());
        }
        if (@event.IsActionPressed("move_crouch"))
        {
            player.ChangeState(new CrouchedState());
        }
    }
    public void Update(PlayerController player, double delta)
    {
        if (!player.IsOnFloor())
        {
            player.ChangeState(new AirborneState());
        }
    }
    public void Exit(PlayerController player)
    {
        
    }
}