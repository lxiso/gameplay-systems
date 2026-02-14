using Godot;
using System;

public partial class AirborneState : IPlayerState
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
    }
    public void Update(PlayerController player, double delta)
    {
        player.targetMovement -= new Vector3(0, player.gravity, 0) * (float)delta;
        if (player.IsOnFloor())
        {
            player.ChangeState(new GroundedState());
            return;
        }
        if (player.IsOnCeiling())
        {
            player.targetMovement.Y = Mathf.Lerp(player.targetMovement.Y, 0f, player.inertia * 1.5f);
        }
    }
    public void Exit(PlayerController player)
    {
        
    }
}