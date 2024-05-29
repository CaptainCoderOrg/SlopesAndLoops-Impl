using System.Diagnostics;
using System.Text;
using Godot;

public partial class SlopedGroundRigidBody2D : RigidBody2D
{
    public int TargetMomentum = -1;
    public const float _90Degrees = 1.5708f;
    [Export]
    private Node2D _playerCenter;
    [Export]
    private Node2D _raycastTarget;
    public Vector2 Up { get; private set; }
    public Vector2 Right { get; private set; }

    public Vector2 _unappliedForce = Vector2.Zero;

    public void AddForce(Vector2 force)
    {
        _unappliedForce += force;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        state.LinearVelocity += _unappliedForce;
        _unappliedForce = Vector2.Zero;
        // var spaceState = GetWorld2D().DirectSpaceState;
        var playerWorldSpaceState = _playerCenter.GetWorld2D().DirectSpaceState;
        Vector2 center = _playerCenter.GlobalPosition;
        Vector2 bottom = _raycastTarget.GlobalPosition;
        Up = (center - bottom).Normalized();
        Right = Up.Rotated(_90Degrees).Normalized();
        var query = PhysicsRayQueryParameters2D.Create(center, bottom);
        var result = playerWorldSpaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            Vector2 normal = (Vector2)result["normal"];
            float angle = -normal.AngleTo(Vector2.Up);
            _playerCenter.Rotation = angle;

            int momentum = Mathf.Sign(state.LinearVelocity.X);
            if (momentum != 0 && TargetMomentum == momentum)
            {
                float magnitude = state.LinearVelocity.Length();
                state.LinearVelocity = momentum * magnitude * Right;
                GD.Print($"LinearVelocity: {state.LinearVelocity} | Magnitude: {magnitude} | Length: {state.LinearVelocity.Length()}");
            }
        }
        else
        {
            _playerCenter.Rotation = 0;
        }
        base._IntegrateForces(state);
        QueueRedraw();
    }

    public override void _Draw()
    {
        base._Draw();
        Vector2 up = Up * 100;
        Vector2 right = Right * 100;
        Vector2 velocity = LinearVelocity;
        DrawLine(Vector2.Zero, up, Color.Color8(255, 0, 0, 255));
        DrawLine(Vector2.Zero, velocity, Color.Color8(0, 0, 255, 255), 3);
        DrawLine(Vector2.Zero, right, Color.Color8(0, 255, 0, 255));

    }

    public void UpdateTargetMomentum(float targetMomentum)
    {
        // If the incoming momentum is different than the previous direction, the player is either stopping or turning
        // and clear their ground connection. This allows the player to turn off walls rather than changing directions
        // when they are running up / down a wall.
        bool isStoppingOrTurning = TargetMomentum != Mathf.Sign(targetMomentum);
        // if (isStoppingOrTurning) { ClearGround(); }
        TargetMomentum = Mathf.Sign(targetMomentum);
    }


}
