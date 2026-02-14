using Godot;
using System;

public partial class DebugHud : Container
{
    public PlayerController player;

    public Label currentState;
    public Label velocity;

    public override void _Ready()
    {
        base._Ready();

        player = GetParentOrNull<PlayerController>();
        if (player != null)
        {
            currentState = FindChild("CurrentState") as Label;
            velocity = FindChild("Velocity") as Label;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (player != null)
        {
            currentState.Text = $"current state: {player.currentState}";
            Vector3 plVel = player.Velocity * player.Transform.Basis;
            velocity.Text = $"velocity: \nX:{plVel.X:F2}\nY:{plVel.Y:F2}\nZ:{plVel.Z:F2}";
        }
    }
}
