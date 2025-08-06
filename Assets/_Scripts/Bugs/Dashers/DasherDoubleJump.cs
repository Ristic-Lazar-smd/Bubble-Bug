using UnityEngine;

public class DasherDoubleJump : PlayerMovement {
    // Old double jump using left/right tap
    //protected override void OnTouchStart(Vector2 worldPosition, float time) {
    //    if (CanJump() && jumpCounter>0){
    //        if (!IsWalled()) {
    //            if (worldPosition.x > 0f) {
    //                //Jump right
    //                if (direction != 1) Flip();
    //            } else {
    //                //Jump Left
    //                if (direction != -1) Flip();
    //            }
    //        }
    //    }
    //base.OnTouchStart(worldPosition, time);
    //}

    protected new void Start() {
        base.Start();
        InputManager.Instance.swipeDetection.OnSwipeLeft += AirJumpLeft;
        InputManager.Instance.swipeDetection.OnSwipeRight += AirJumpRight;
    }

    private void AirJumpLeft() {
        if (jumpCounter == stats.maxJumps) {
            if (!isWallJump && direction == 1) Flip();
        }
    }

    private void AirJumpRight() {
        if (jumpCounter == stats.maxJumps) {
            if (!isWallJump && direction == -1) Flip();
        }
    }
}
