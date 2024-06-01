using Godot;

public partial class SlopedGroundRigidBody2D : RigidBody2D
{
    public int TargetMomentum = -1;
    public const float _90Degrees = 1.5708f;
    [Export]
    private Node2D _raycastTarget;
    public Vector2 Up { get; private set; } = Vector2.Up;
    public Vector2 Right { get; private set; } = Vector2.Right;
    public Vector2 _unappliedForce = Vector2.Zero;
    [Export]
    public Node2D Collider;
    public bool IsGrounded { get; set; }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        var playerWorldSpaceState = Collider.GetWorld2D().DirectSpaceState;
        Vector2 center = Collider.GlobalPosition;
        Vector2 bottom = _raycastTarget.GlobalPosition;
        var query = PhysicsRayQueryParameters2D.Create(center, bottom);
        var result = playerWorldSpaceState.IntersectRay(query);
        IsGrounded = result.Count > 0;
        if (IsGrounded)
        {
            Vector2 normal = (Vector2)result["normal"];
            SetUp(normal, state);
            SnapToGround(result);
        }
        else
        {
            ClearGround();
        }
        base._IntegrateForces(state);
        QueueRedraw();
    }

    private void SetUp(Vector2 newUp, PhysicsDirectBodyState2D state)
    {
        float angle = -newUp.AngleTo(Vector2.Up);
        Collider.Rotation = angle;
        Up = newUp;
        Right = Up.Rotated(_90Degrees).Normalized();

        int momentum = Mathf.Sign(state.LinearVelocity.X);
        if (momentum != 0 && TargetMomentum == momentum)
        {
            float magnitude = state.LinearVelocity.Length();
            state.LinearVelocity = momentum * magnitude * Right;
            GD.Print($"LinearVelocity: {state.LinearVelocity} | Magnitude: {magnitude} | Length: {state.LinearVelocity.Length()}");
        }
    }

    private void SnapToGround(Godot.Collections.Dictionary castResult)
    {
        Vector2 groundEdge = (Vector2)castResult["position"];
        var query = PhysicsRayQueryParameters2D.Create(groundEdge, Collider.GlobalPosition);
        var playerWorldSpaceState = Collider.GetWorld2D().DirectSpaceState;
        var result = playerWorldSpaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            GD.Print("Cast?");
            GD.Print(result["collider"]);
            Vector2 playerEdge = (Vector2)result["position"];
            // Collider.Position += playerEdge - groundEdge;
        }
    }

    private void ClearGround()
    {
        Up = Vector2.Up;
        Right = Vector2.Right;
        Collider.Rotation = 0;
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
