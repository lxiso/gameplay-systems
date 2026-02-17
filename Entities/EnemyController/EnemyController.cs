using Godot;

public partial class EnemyController : CharacterBody3D
{
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    
    public enum TYPE {GROUNDED, FLYING}
    [Export] public TYPE type = TYPE.GROUNDED;

    public Vector3 motionVelocity;
    public Vector3 impulseVelocity;
    public float verticalVelocity;

    public Timer stunTimer;
	public IEnemyState currentState;

    [Export] public float health = 100;
    [Export] public float friction = 10f;

    public void TakeDamage (float amount, Node source, float knockback = 0f, bool canStun = false)
    {
        GD.Print($"current health: {health}");
        if (canStun == true) ChangeState(new IEStunnedState());
        currentState?.TakeDamage(this, amount, source, knockback, canStun);
    }

    public void ApplyImpulse(Vector3 force)
    {
        impulseVelocity = new Vector3(force.X, 0, force.Z);
        if (force.Y != 0) verticalVelocity += force.Y;
    }

    public void ApplyVerticalForce(double delta)
    {
        if (IsOnFloor() && verticalVelocity <= 0) verticalVelocity = -.1f;
        else verticalVelocity -= type != TYPE.FLYING ? gravity * (float)delta : 0f;
    }

    private void ApplyFriction(double delta)
    {
        impulseVelocity = impulseVelocity.Lerp(Vector3.Zero, friction);
    }

	public void ChangeState(IEnemyState state)
    {
        currentState?.Exit(this);
        currentState = state;
        currentState?.Enter(this);
    }

    private void OnStunTimeout()
    {
        ChangeState(new IEAirborneState());
    }

    public override void _Ready()
    {
        ChangeState(new IEGroundedState());
        base._Ready();

        stunTimer = FindChild("StunTimer") as Timer;
        stunTimer.Timeout += OnStunTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        currentState?.Update(this, delta);
        ApplyVerticalForce(delta);
        ApplyFriction(delta);
        Velocity = motionVelocity + impulseVelocity + new Vector3(0, verticalVelocity, 0);
        MoveAndSlide();
        motionVelocity = Vector3.Zero;
    }
}
