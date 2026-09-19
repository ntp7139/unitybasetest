using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController controller)
        : base(playerController: controller) { }

    public override void Enter()
    {
        if (playerController != null)
        {
            playerController.PlayAnimation(AnimationHash.ReturnAnimation(1));
        }
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController?.ChangeJumpState();
            return;
        }
        if (
            Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.W)
        )
        {
            playerController?.ChangeRunState();
            return;
        }
    }

    public override void Exit() { }
}
