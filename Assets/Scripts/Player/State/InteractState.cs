public class InteractState : BaseState
{
    public InteractState(PlayerController controller)
        : base(playerController: controller) { }

    public override void Enter()
    {
        if (playerController != null)
        {
            playerController.PlayAnimation(AnimationHash.ReturnAnimation(4));
        }
    }

    public override void Update() { }

    public override void Exit() { }
}
