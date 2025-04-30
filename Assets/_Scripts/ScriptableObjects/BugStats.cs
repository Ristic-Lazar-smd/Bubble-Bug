using UnityEngine;

[CreateAssetMenu(menuName = "BugStats")]
public class BugStats : ScriptableObject
{
    [Header("Movement Settings")]
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float jumpingPower { get; private set; }
    [field: SerializeField] public int maxJumps { get; private set; }

    [Header("Wall Interaction")]
    [field: SerializeField] public Vector2 wallJumpingPower { get; private set; }
    [field: SerializeField] public float wallSlidingSpeed { get; private set; }

    [Header("Coyote Time")]
    [field: SerializeField] public float wallJumpCoyoteWindow { get; private set; }
    [field: SerializeField] public float jumpCoyoteWindow { get; private set; }
}
