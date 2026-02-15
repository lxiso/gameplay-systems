using Godot;
using System;

public partial class ObjectPicker : Node3D
{
	public PlayerController player;
	public Camera3D camera;
	public RayCast3D ray;
	public SpringArm3D spring;
	public Marker3D aim;

	[Export] public float pickDistance = 10f;
	[Export] public float pickForce = 10f;
	[Export] public float throwForce = 300f;
	[Export] public float aimMaxDistance = 100f;

	private RigidBody3D _currentObject;

	public override async void _Ready()
	{
		player = GetParentOrNull<PlayerController>();
		if (player != null)
		{
			await ToSignal(player, Node.SignalName.Ready);

			camera = player.camera;

			ray = new RayCast3D{TargetPosition = new Vector3(0f, 0f, -pickDistance)};

			spring = new SpringArm3D{Shape = new SphereShape3D{Radius = .5f},
			SpringLength = aimMaxDistance,
			RotationDegrees = new Vector3(0f, 180f, 0f),
			Position = Vector3.Zero};

			aim = new Marker3D();

			camera.AddChild(ray);
			camera.AddChild(spring);
			spring.AddChild(aim);
		}
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

		if (@event.IsActionPressed("action_interact", false))
		{
			if (_currentObject == null) PickObject();
			else DropObject();
		}
		if (@event.IsActionPressed("action_left"))
		{
			if (_currentObject != null) ThrowObject();
		}
    }

	private void PickObject()
	{
		ray.ForceRaycastUpdate();
		if (ray.IsColliding() && ray.GetCollider() is RigidBody3D && (ray.GetCollider() as RigidBody3D).IsInGroup("Pickable"))
		{
			_currentObject = ray.GetCollider() as RigidBody3D;
			_currentObject.SetCollisionLayerValue(1, false);
			_currentObject.SetCollisionLayerValue(2, false);
		}
	}

	private void ThrowObject()
	{
		Vector3 direction = aim.GlobalPosition - _currentObject.GlobalPosition;
		float distance = _currentObject.GlobalPosition.DistanceTo(aim.GlobalPosition);
		_currentObject.ApplyCentralImpulse(direction.Normalized() * (throwForce + (distance * 2.5f)));
		_currentObject.SetCollisionLayerValue(1, true);
		_currentObject.SetCollisionLayerValue(2, false);
		_currentObject = null;
	}

	private void DropObject()
	{
		_currentObject.LinearVelocity = Vector3.Zero;
		_currentObject.SetCollisionLayerValue(1, true);
		_currentObject.SetCollisionLayerValue(2, false);
		_currentObject = null;
	}

	private void ApplyForces()
	{
		if (_currentObject != null)
		{
			Vector3 targetPosition = camera.GlobalPosition + (camera.GlobalBasis * new Vector3(1f, 1f, -1f));
			Vector3 objectPos = _currentObject.GlobalPosition;
			
			if (_currentObject.GlobalPosition.DistanceTo(targetPosition) > pickDistance)
			{
				DropObject();
				return;
			}
			
			_currentObject.GlobalRotation = camera.GlobalRotation;
			_currentObject.LinearVelocity = (targetPosition - objectPos) * pickForce;
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		ApplyForces();
    }

}
