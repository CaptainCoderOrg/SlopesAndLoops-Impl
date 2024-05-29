// using System.Diagnostics;
// using System.Text;
// using Godot;

// public partial class SlopeGround : Node2D
// {
//     public int TargetMomentum = -1;
//     public const float _90Degrees = 1.5708f;
//     [Export]
//     private SlopedGroundRigidBody2D _slopeBody;
//     [Export]
//     private Node2D _playerCenter;
//     [Export]
//     private Node2D _raycastTarget;
//     public Vector2 Up { get; private set; }
//     public Vector2 Right { get; private set;}
	
// 	// Called when the node enters the scene tree for the first time.
// 	public override void _Ready()
// 	{
//         GD.Print(_slopeBody);
// 	}

// 	// Called every frame. 'delta' is the elapsed time since the previous frame.
// 	public override void _Process(double delta)
// 	{
//         _test++;
// 	}
//     private float _test = 0;
//     public override void _PhysicsProcess(double delta)
//     {
//         base._PhysicsProcess(delta);
//         // var spaceState = GetWorld2D().DirectSpaceState;
//         var playerWorldSpaceState = _playerCenter.GetWorld2D().DirectSpaceState;
//         Vector2 center = _playerCenter.GlobalPosition;
//         Vector2 bottom = _raycastTarget.GlobalPosition;
//         Up = (center - bottom).Normalized();
//         Right = Up.Rotated(_90Degrees);
//         var query = PhysicsRayQueryParameters2D.Create(center, bottom);
//         var result = playerWorldSpaceState.IntersectRay(query);
//         if (result.Count > 0)
//         {
//             Vector2 normal = (Vector2)result["normal"];
//             float angle = -normal.AngleTo(Vector2.Up);
//             _playerCenter.Rotation = angle; 
            
//             int momentum = Mathf.Sign(_slopeBody.LinearVelocity.X);
//             if (TargetMomentum == momentum)
//             {
//                 float magnitude = _slopeBody.LinearVelocity.Length();
//                 if (magnitude > 100)
//                 {
//                     GD.Print($"Momentum: {momentum} | {magnitude} | {Right}");
//                     _slopeBody.SetLinearVelocity(momentum * magnitude * Right);
//                 }
//             }
//         }
//         else
//         {
//             _playerCenter.Rotation = 0;
//         }
//         QueueRedraw();
//     }

//     public override void _Draw()
//     {
//         base._Draw();
//         Vector2 up = -Position + Up * 100;
//         Vector2 right = -Position + Right * 100;
//         Vector2 velocity = -Position + _slopeBody.LinearVelocity;
//         DrawLine(-Position, up, Color.Color8(255, 0, 0, 255));
//         DrawLine(-Position, velocity, Color.Color8(0, 0, 255, 255), 3);
//         DrawLine(-Position, right, Color.Color8(0, 255, 0, 255));
        
//     }

//     public void UpdateTargetMomentum(float targetMomentum)
//     {
//         // If the incoming momentum is different than the previous direction, the player is either stopping or turning
//         // and clear their ground connection. This allows the player to turn off walls rather than changing directions
//         // when they are running up / down a wall.
//         bool isStoppingOrTurning = TargetMomentum != Mathf.Sign(targetMomentum);
//         // if (isStoppingOrTurning) { ClearGround(); }
//         TargetMomentum = Mathf.Sign(targetMomentum);
//     }


// }
