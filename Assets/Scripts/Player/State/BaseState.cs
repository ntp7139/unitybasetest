public abstract class BaseState : IState
{
    protected PlayerController playerController;

    public BaseState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }
}
