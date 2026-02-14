using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    public Marker3D head;
    public Camera3D camera;
    public CollisionShape3D collider;
    public ShapeCast3D crouchCheck;

    public IPlayerState currentState;
    public float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    [Export] public float inertia = .25f;
    [Export] public float height = 1.7f;

    private Vector3 _movement;
    public Vector3 targetMovement;
    [Export] public float walkSpeed = 10f;
    [Export] public float crouchSpeed = 6f;
    [Export] public float runSpeed = 20f;
    [Export] public float jumpForce = 8f;

    private Vector3 _headbob;
    [Export] public Vector2 cameraInertia = new Vector2(.5f, 2.5f);

    [Export] float mouseSens = .005f;

    public override void _Ready()
    {
        base._Ready();

        head = FindChild("Head") as Marker3D;
        camera = FindChild("Camera3D") as Camera3D;
        collider = FindChild("CollisionShape3D") as CollisionShape3D;
        crouchCheck = FindChild("CrouchCheck") as ShapeCast3D;

        ChangeState(new GroundedState());

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public void ChangeState(IPlayerState state)
    {
        currentState?.Exit(this);
        currentState = state;
        currentState?.Enter(this);
        float colliderHeight = collider.Shape.Get("height").AsSingle();
        head.Position = new Vector3(0, colliderHeight/2 * .7f, 0);
    }

    private void MouseMovement(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            RotateY(-mouseMotion.Relative.X * mouseSens);
            head.RotateX(-mouseMotion.Relative.Y * mouseSens);
        }
    }

    private Vector3 ApplyInertia(Vector3 target, float weight)
    {
        _movement.X = Mathf.Lerp(_movement.X, target.X, weight);
        _movement.Y = target.Y;
        _movement.Z = Mathf.Lerp(_movement.Z, target.Z, weight);
        return _movement;
    }

    private void Headbob(Vector3 velocity)
    {
        if (velocity.X != 0)
        {
            velocity *= Transform.Basis;
            _headbob.X = -Mathf.DegToRad(Mathf.Clamp(velocity.Z, -cameraInertia.X, cameraInertia.X));
            _headbob.Z = -Mathf.DegToRad(Mathf.Clamp(velocity.X, -cameraInertia.Y, cameraInertia.Y));
            camera.Rotation = camera.Rotation.Lerp(_headbob, inertia);
        }
        else
        {
            camera.Rotation = camera.Rotation.Lerp(Vector3.Zero, inertia);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        MouseMovement(@event);
        currentState?.HandleInput(this, @event);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Headbob(Velocity);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        currentState?.Update(this, delta);
        Velocity = ApplyInertia(targetMovement, inertia);
        MoveAndSlide();
    }

}
