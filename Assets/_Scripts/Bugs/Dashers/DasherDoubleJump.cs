using UnityEngine;

public class DasherDoubleJump : PlayerMovement
{
    protected override void OnTouchStart(Vector2 worldPosition, float time) {
        if (CanJump()){
            if (!IsWalled()) {
                if (worldPosition.x > 0f) {
                    //Jump right
                    if (direction != 1) Flip();
                } else {
                    //Jump Left
                    if (direction != -1) Flip();
                }
            }
        }
    base.OnTouchStart(worldPosition, time);
    }
}
