using UnityEngine;

public class DasherDoubleJump : PlayerMovement
{

    float screenWidth;
    float midPointX;
    protected override void Start(){
        screenWidth = Screen.width;
        midPointX = screenWidth / 2;
        base.Start();
    }
    protected override void OnTouchStart(Vector2 screenPosition, float time) {
        if (CanJump()){
            if (!IsWalled()) {
                if (screenPosition.x > midPointX) {
                    //Jump right
                    if (direction != 1) Flip();
                } else {
                    //Jump Left
                    if (direction != -1) Flip();
                }
            }
        }
    base.OnTouchStart(screenPosition, time);
    }
}
