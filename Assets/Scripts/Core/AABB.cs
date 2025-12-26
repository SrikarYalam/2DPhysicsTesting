// Axis Aligned Bounding Box
using UnityEngine;

public struct AABB
{
    public Vector2 Center;
    public Vector2 HalfSize; // Half width and half height

    public AABB(Vector2 center, Vector2 size)
    {
        Center = center;
        HalfSize = size * 0.5f;
    }

    public Vector2 GetClosestPoint(Vector2 point)
    {
        Vector2 min = Center - HalfSize;
        Vector2 max = Center + HalfSize;

        float clampedX = Mathf.Clamp(point.x, min.x, max.x);
        float clampedY = Mathf.Clamp(point.y, min.y, max.y);

        return new Vector2(clampedX, clampedY);
    }
}