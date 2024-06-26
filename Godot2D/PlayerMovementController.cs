using Godot;

public partial class PlayerMovementController : Node
{
	private float _movementInput;
	private float Multiplier = 100;
	[Export]
	public float AccelerationFactor { get; set; }
	[Export]
	private SlopedGroundRigidBody2D _groundInfo;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_movementInput = Input.GetAxis("ui_left", "ui_right");
		_groundInfo.UpdateTargetMomentum(_movementInput);
		Vector2 force = _movementInput * AccelerationFactor * Multiplier * (float)delta * _groundInfo.Right;
		_groundInfo.ApplyForce(force);
	}
}
