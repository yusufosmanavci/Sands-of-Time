using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerValues playerValues;
    private void Awake()
    {
        playerValues = GetComponent<PlayerValues>();
    }
    private void Update()
    {
        SetAnimation();
    }
    // rigidbody y deðerini kontrol edip duruma göre düþme ya da zýplama animasyonunu oynat.
    private void SetAnimation()
    {
        if (playerValues.IsDashing)
        {
            playerValues.animator.Play("Dash Animation");
            return;
        }
        if(!playerValues.IsGrounded)
        {
            playerValues.animator.Play("Jump Animation");
            return;

        }
        if (playerValues.InputX != 0)
        {
            playerValues.animator.Play("Run Animation");
            return;
        }
        else
        {
            playerValues.animator.Play("Idle Animation");
        }
    }
}
