using UnityEngine;

public class RunState : BaseState
{
    public RunState(PlayerController controller)
        : base(playerController: controller) { }

    public override void Enter()
    {
        if (playerController != null)
        {
            playerController.PlayAnimation(AnimationHash.ReturnAnimation(2));
        }
    }

    public override void Update()
    {
        if (playerController != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerController.ChangeJumpState();
                return;
            }
            Vector3 direction = playerController.Move();
            if (direction == Vector3.zero)
            {
                playerController.ChangeIdleState();
            }
        }
    }

    public override void Exit() { }
}
