using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PhysObject
{
    // CONTSANTS
    public const float EPSILON = 0.0001f;

    // STATE
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 _frameAcceleration;

    public bool IsGrounded = false;


    // TUNING
    [Header("Movement Settings")]
    public float MaxSpeed = 20.0f;
    public float Acceleration = 120.0f;

    [Header("Control Settings")]
    public float AirDrag = 5.0f;
    public float GroundFriction = 5.0f;
    public float Traction = 3f;
    public float CounterSteerThreshold = -0.5f;

    [Header("Environment Settings")]
    public float Gravity = 60.0f;
    public float Restitution = 0.5f;
    public float Radius = 0.5f;

    public void Tick(float deltaTime)
    {
        // Apply Gravity
        Velocity.y -= Gravity * deltaTime;

        // Update Velocity
        Velocity += _frameAcceleration * deltaTime;
        _frameAcceleration = Vector2.zero;

        // Apply Drag
        float currentDrag = IsGrounded ? GroundFriction : AirDrag;
        ApplyDrag(currentDrag, deltaTime);

        // Clamp Speed
        Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);

        // Update Position
        Position += Velocity * deltaTime;
    }

    public void ApplyForce(Vector2 force)
    {
        _frameAcceleration += force;
    }

    public void ApplyDrag(float dragCoefficient, float deltaTime)
    {
        if (Velocity.magnitude > EPSILON)
        {
            Vector2 drag = Velocity.normalized * dragCoefficient * deltaTime;
            if (drag.magnitude > Velocity.magnitude)
            {
                Velocity = Vector2.zero;
            }
            else
            {
                Velocity -= drag;
            }
        }
    }

    // Add impulse for jump
    public void ApplyImpulse(Vector2 impulse)
    {
        Velocity += impulse;
    }

    // Kill vertical velocity
    public void KillVerticalMomentum()
    {
        if (Velocity.y < 0)
        {
            Velocity.y = 0;
        }
    }

}