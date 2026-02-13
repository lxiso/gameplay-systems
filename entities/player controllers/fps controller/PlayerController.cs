using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    public Marker3D head;
    public Camera3D camera;

    public IPlayerState currentState;
    public float gravity = (float)ProjectSettings.GetSetting("physics/3d/gravity");
    public float inertia = .05f;

    public Vector3 movement = Vector3.Zero;
    [Export] public float walkSpeed = 10f;
    [Export] public float runSpeed = 20f;

    [Export] float mouseSens = .02f;

    public override void _Ready()
    {
        base._Ready();
        ChangeState(new GroundedState());

        head = FindChild("Head") as Marker3D;
        camera = FindChild("Camera3D") as Camera3D;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void ChangeState(IPlayerState state)
    {
        currentState?.Exit(this);
        currentState = state;
        currentState?.Enter(this);
    }

    private void MouseMovement(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            RotateY(-mouseMotion.Relative.X * mouseSens);
            head.RotateX(-mouseMotion.Relative.Y * mouseSens);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        MouseMovement(@event);
        currentState?.HandleInput(this, @event);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        currentState?.Update(this, delta);
        Velocity = movement;
        MoveAndSlide();
        movement = Velocity;
    }

}
