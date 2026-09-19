using UnityEngine;

public class JumpState : BaseState, IOnCollisionEnter
{
    public JumpState(PlayerController controller)
        : base(playerController: controller) { }

    public override void Enter()
    {
        if (playerController != null)
        {
            playerController.PlayAnimation(AnimationHash.ReturnAnimation(3));
        }

        playerController?.Jump();
    }

    void CompleteJumping()
    {
        if (
            Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.W)
        )
        {
            playerController.ChangeRunState();
        }
        else
        {
            playerController.ChangeIdleState();
        }
    }

    public override void Update()
    {
        CheckOnGround();
        if (playerController != null)
        {
            playerController.Move();
        }
    }

    void CheckOnGround()
    {
        if (playerController.IsJumpDown())
        {
            if (playerController.IsGrounded())
            {
                CompleteJumping();
                return;
            }
        }
    }

    public override void Exit() { }

    public void OnCollisionEnter()
    {
        CompleteJumping();
    }
}
