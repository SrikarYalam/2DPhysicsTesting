using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public static Arena Instance;
    private List<AABB> _walls = new List<AABB>();

    void Awake()
    {
        Instance = this;
        CreateWalls();
    }

    void CreateWalls()
    {
        float width = 30.0f;
        float height = 12.0f;
        float wallThickness = 2.0f;

        // Bottom Wall
        _walls.Add(new AABB(new Vector2(0, -height / 2 - wallThickness / 2), new Vector2(width + wallThickness * 2, wallThickness)));

        // Top Wall
        _walls.Add(new AABB(new Vector2(0, height / 2 + wallThickness / 2), new Vector2(width + wallThickness * 2, wallThickness)));

        // Left Wall
        _walls.Add(new AABB(new Vector2(-width / 2 - wallThickness / 2, 0), new Vector2(wallThickness, height)));

        // Right Wall
        _walls.Add(new AABB(new Vector2(width / 2 + wallThickness / 2, 0), new Vector2(wallThickness, height)));
    }

    // Resolve collisions between wall and physObject
    public void ResolveCollisions(PhysObject obj)
    {
        foreach (AABB wall in _walls)
        {
            Collision.Resolve(obj, wall);
        }
    }

    // Draw gizmos
    void OnDrawGizmos()
    {
        if (_walls == null || _walls.Count == 0) return;

        Gizmos.color = Color.red;
        foreach (AABB wall in _walls)
        {
            Vector2 size = wall.HalfSize * 2;
            Gizmos.DrawWireCube(wall.Center, size);
        }
    }
}