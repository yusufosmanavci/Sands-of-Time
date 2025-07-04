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

    private void SetAnimation()
    {
        if (playerValues.InputX != 0)
        {
            playerValues.animator.Play("Run Animation");
        }
        else
        {
            playerValues.animator.Play("Idle Animation");
        }
    }
}
