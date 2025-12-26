using UnityEngine;

public static class Collision
{
    // physObject vs AABB collision
    public static void Resolve(PhysObject obj, AABB wall)
    {
        // get closest point on AABB to circle center
        Vector2 closestPoint = wall.GetClosestPoint(obj.Position);

        // vector from closest point to circle center
        Vector2 difference = obj.Position - closestPoint;
        float distance = difference.magnitude;

        // check for collision
        if (distance < obj.Radius)
        {
            // set grounded
            obj.IsGrounded = true;

            // normalize difference vector
            Vector2 normal = difference.normalized;

            // penetration depth
            float penetration = obj.Radius - distance;

            // move circle out of collision
            obj.Position += normal * penetration;

            // reflect velocity
            float velocityAlongNormal = Vector2.Dot(obj.Velocity, normal);
            if (velocityAlongNormal < 0) // only reflect if moving towards the wall
            {
                Vector2 reflection = obj.Velocity - (1 + obj.Restitution) * velocityAlongNormal * normal;
                obj.Velocity = reflection;
            }
        }
    }
}