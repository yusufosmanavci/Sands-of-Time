using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerValues playerValues;
    PlayerAnimations playerAnimations;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerValues = GetComponent<PlayerValues>();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerInput = new PlayerInput();
        playerInput.PlayerInputs.Enable();
    }

    void Update()
    {
        HorizontalMove();
        //Dash();
    }

    private void HorizontalMove()
    {
        playerValues.InputX = playerInput.PlayerInputs.MoveInputs.ReadValue<Vector2>().x;
        playerValues.rb.linearVelocity = new Vector2(playerValues.InputX * playerValues.moveSpeed, playerValues.rb.linearVelocity.y);
        if (playerValues.IsfacingRight && playerValues.InputX < 0)
        {
            playerValues.IsfacingRight = false;
            playerValues.spriteRenderer.flipX = true;
        }
        else if (!playerValues.IsfacingRight && playerValues.InputX > 0)
        {
            playerValues.IsfacingRight = true;
            playerValues.spriteRenderer.flipX = false;
        }
    }

    /*private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerValues.currentDashCooldown == 0)
        {
            playerValues.currentDashCooldown = playerValues.dashCooldown;
        }
        if (playerValues.currentDashCooldown > 0)
        {
            playerValues.currentDashCooldown -= Time.deltaTime;
        }
        else
        {
            playerValues.currentDashCooldown = 0;
        }
    }*/
}
