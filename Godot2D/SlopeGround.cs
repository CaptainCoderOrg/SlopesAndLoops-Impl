using System.Diagnostics;
using System.Text;
using Godot;

public partial class SlopeGround : Node2D
{
    public const float _90Degrees = 1.5708f;
    [Export]
    private RigidBody2D _rigidbody;
    [Export]
    private Node2D _playerCenter;
    [Export]
    private Node2D _raycastTarget;
    public Vector2 Up { get; private set; }
    public Vector2 Right { get; private set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        // var spaceState = GetWorld2D().DirectSpaceState;
        var playerWorldSpaceState = _playerCenter.GetWorld2D().DirectSpaceState;
        Vector2 center = _playerCenter.GlobalPosition;
        Vector2 bottom = _raycastTarget.GlobalPosition;
        Up = (bottom - center).Normalized();
        Right = Up.Rotated(-_90Degrees);
        var query = PhysicsRayQueryParameters2D.Create(center, bottom);
        var result = playerWorldSpaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            Vector2 normal = (Vector2)result["normal"];
            float angle = -normal.AngleTo(Vector2.Up);
            _playerCenter.Rotation = angle; 
        }
        else
        {
            _playerCenter.Rotation = 0;
        }
        
    }

    public override void _Draw()
    {
        base._Draw();
        Vector2 up = -Position + Up * 100;
        Vector2 right = -Position + Right * 100;
        DrawLine(-Position, up, Color.Color8(255, 0, 0, 255));
        DrawLine(-Position, right, Color.Color8(0, 255, 0, 255));
    }


}
