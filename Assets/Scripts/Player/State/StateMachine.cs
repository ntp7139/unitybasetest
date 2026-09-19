public class StateMachine
{
    private IState currentState;

    public StateMachine(IState initState)
    {
        currentState = initState;
        currentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void OnCollisionEnter()
    {
        if (currentState != null)
        {
            IOnCollisionEnter stateCollision = currentState as IOnCollisionEnter;
            if (stateCollision != null)
            {
                stateCollision.OnCollisionEnter();
            }
        }
    }
}
