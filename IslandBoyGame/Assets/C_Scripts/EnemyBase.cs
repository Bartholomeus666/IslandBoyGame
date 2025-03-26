using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private Transform enemyTransform;

    private void Awake()
    {
        enemyTransform = transform;
    }

    public bool IsGrounded()
    {
        // Use the enemy's local down direction for ground checking
        Vector3 localDownDirection = -enemyTransform.up;

        // Cast a ray in the local down direction from the enemy's position
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(
            enemyTransform.position,
            localDownDirection,
            out hit,
            groundCheckDistance,
            groundLayer
        );

        // Optional: Visualize the ground check ray in the scene view
        Debug.DrawRay(enemyTransform.position, localDownDirection * groundCheckDistance,
            isGrounded ? Color.green : Color.red);

        return isGrounded;
    }

    // Optional: Method to get the surface normal when grounded
    public Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemyTransform.position, -enemyTransform.up, out hit, groundCheckDistance, groundLayer))
        {
            return hit.normal;
        }
        return Vector3.up;
    }
}
