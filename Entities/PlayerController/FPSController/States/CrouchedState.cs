using Godot;
using System;

public partial class CrouchedState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.collider.Shape.Set("height", player.height/2f);
        player.GlobalPosition -= new Vector3(0, player.height/4f, 0);
    }
    public void HandleInput(PlayerController player, InputEvent @event)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards").Normalized();
        player.targetMovement.X = direction.X * player.crouchSpeed;
        player.targetMovement.Z = direction.Y * player.crouchSpeed;
        player.targetMovement = player.targetMovement.Rotated(Vector3.Up, player.Rotation.Y);

        if (@event.IsActionPressed("move_crouch") && !player.crouchCheck.IsColliding())
        {
            player.ChangeState(new GroundedState());
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
        player.collider.Shape.Set("height", player.height);
    }
}