using Godot;
using System;

public partial class GroundedState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        GD.Print("grouded");
    }
    public void HandleInput(PlayerController player, InputEvent @event)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards").Normalized();
        direction *= Input.IsActionPressed("move_run") ? player.runSpeed : player.walkSpeed;
        player.movement = new Vector3(direction.X, 0, direction.Y).Rotated(Vector3.Up, player.Rotation.Y);
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