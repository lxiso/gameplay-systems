using Godot;
using System;

public partial class AirborneState : IPlayerState
{
    Vector2 direction = Vector2.Zero;
    public void Enter(PlayerController player)
    {
        GD.Print("airborne");
    }
    public void HandleInput(PlayerController player, InputEvent @event)
    {
        direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards").Normalized();
        float speed = Input.IsActionPressed("move_run") ? player.runSpeed : player.walkSpeed;
        Vector3 player_movement = player.movement.Rotated(Vector3.Up, player.Rotation.Y);
        player_movement.X = Mathf.Lerp(player_movement.X, direction.X * speed, player.inertia);
        //player.movement.X = direction.X * speed;
        player_movement.Z = Mathf.Lerp(player_movement.Z, direction.Y * speed, player.inertia);
        //player.movement.Z = direction.Y * speed;
        player.movement = player_movement;
    }
    public void Update(PlayerController player, double delta)
    {
        player.movement.Y -= player.gravity * (float)delta;
        if (player.IsOnFloor())
        {
            player.ChangeState(new GroundedState());
            return;
        }
    }
    public void Exit(PlayerController player)
    {
        
    }
}