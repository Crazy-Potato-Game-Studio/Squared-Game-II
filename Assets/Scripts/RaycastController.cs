using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public const float skinWidth = 0.015f;
    public float horizontalRayCount;
    public float verticalRayCount;
    public LayerMask collisionMask;

    [HideInInspector]
    public float raySpacing;

    [HideInInspector]
    public BoxCollider2D col;
    public RaycastOrigins raycastOrigins;
    
    public virtual void Start()
    {
        col = GetComponent<BoxCollider2D>();

        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins(){
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing(){
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        raySpacing = bounds.size.x / (horizontalRayCount - 1);
    }

    public struct RaycastOrigins{
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
