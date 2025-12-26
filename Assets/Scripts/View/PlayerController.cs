using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PhysObject PhysicsData = new PhysObject();

    [Header("Player Settings")]
    public float BoostPower = 40.0f;
    private bool _wantsToBoost = false;

    void Start()
    {
        GameLoop.Instance.OnTick += HandleTick;
        PhysicsData.Position = transform.position;
    }

    private void OnDestroy()
    {
        if (GameLoop.Instance != null)
        {
            GameLoop.Instance.OnTick -= HandleTick;
        }
    }

    // visual update
    void Update()
    {
        transform.position = PhysicsData.Position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _wantsToBoost = true;
        }
    }

    // physics update
    void HandleTick()
    {
        float x = Input.GetAxis("Horizontal");
        // float y = Input.GetAxis("Vertical");
        float y = 0;

        // normalize diagonal movement
        Vector2 inputDir = new Vector2(x, y).normalized;

        // target force
        Vector2 targetForce = inputDir * PhysicsData.Acceleration;

        // Check for counter steering
        if (PhysicsData.Velocity.magnitude > PhysObject.EPSILON)
        {
            float velocityDotInput = Vector2.Dot(PhysicsData.Velocity.normalized, inputDir);
            if (velocityDotInput < PhysicsData.CounterSteerThreshold) // counter steering
            {
                targetForce *= PhysicsData.Traction;
            }
        }

        // Apply force
        if (inputDir.magnitude > PhysObject.EPSILON)
        {
            PhysicsData.ApplyForce(targetForce);
        }

        // Handle Boost Input
        if (_wantsToBoost)
        {
            PhysicsData.KillVerticalMomentum();
            PhysicsData.ApplyImpulse(Vector2.up * BoostPower);
            _wantsToBoost = false;
        }
        
        // Tick physics
        PhysicsData.Tick(GameLoop.DeltaTime);

        // Reset Grounded State
        PhysicsData.IsGrounded = false;

        // Resolve collisions with arena walls
        if (Arena.Instance != null)
        {
            Arena.Instance.ResolveCollisions(PhysicsData);
        }
    }

    // Draw gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(PhysicsData.Position, PhysicsData.Radius);
    }
}