using Godot;
using System;

public partial class ObjectPicker : Node3D
{
	public PlayerController player;
	public Camera3D camera;
	public ShapeCast3D shapeCast;

	[Export] public float pickForce = 5f;
	[Export] public float maxDistance = 5f;

	private RigidBody3D _currentObject;

	public override async void _Ready()
	{
		player = GetParentOrNull<PlayerController>();
		if (player != null)
		{
			await ToSignal(player, Node.SignalName.Ready);

			camera = player.camera;
			shapeCast = new ShapeCast3D();
			camera.AddChild(shapeCast);
			shapeCast.TargetPosition = new Vector3(0f, 0f, -2f);
			shapeCast.Shape = new SphereShape3D{Radius = .1f};
			shapeCast.Position = Vector3.Zero;

			var debugMesh = new MeshInstance3D();
			debugMesh.Mesh = new BoxMesh{Size = new Vector3(.1f, .1f, .1f)};
			shapeCast.AddChild(debugMesh);
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
    }

	private void PickObject()
	{
		shapeCast.ForceShapecastUpdate();
		if (shapeCast.IsColliding())
		{
			int collisionCount = shapeCast.GetCollisionCount();

			for (int i = 0; i < collisionCount; i++)
			{
				RigidBody3D collider = (RigidBody3D)shapeCast.GetCollider(i);
				if (collider != null || collider.IsInGroup("Pickable") || collider is RigidBody3D)
				{
					collider.SetCollisionLayerValue(1, false);
					collider.SetCollisionLayerValue(2, true);
					_currentObject = collider;
				}
			}
		}
	}

	private void DropObject()
	{
		_currentObject.LinearVelocity = Vector3.Zero;
		_currentObject.SetCollisionLayerValue(1, true);
		_currentObject.SetCollisionLayerValue(2, false);
		_currentObject = null;
	}

	private void ApplyForces(double delta)
	{
		if (_currentObject != null)
		{
			Vector3 targetPosition = camera.GlobalPosition + (camera.GlobalBasis * new Vector3(0, 0, -2f));
			Vector3 objectPos = _currentObject.GlobalPosition;
			_currentObject.LinearVelocity = (targetPosition - objectPos) * pickForce * (objectPos.DistanceTo(targetPosition) * 8f);
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		ApplyForces(delta);
    }

}
