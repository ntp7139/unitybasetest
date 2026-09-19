public class DeathState : BaseState
{
    public DeathState(PlayerController controller)
        : base(playerController: controller) { }

    public override void Enter()
    {
        if (playerController != null)
        {
            playerController.PlayAnimation(AnimationHash.ReturnAnimation(5));
        }
    }

    public override void Update() { }

    public override void Exit() { }
}
