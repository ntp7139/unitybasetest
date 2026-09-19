using System.Collections.Generic;

public static class AnimationHash
{
    public static readonly Dictionary<int, string> _animationIndex = new Dictionary<int, string>()
    {
        { 1, "Player_Idle" },
        { 2, "Player_Run" },
        { 3, "Player_Jump" },
        { 4, "Player_Interact" },
        { 5, "Player_Death" },
    };

    public static string ReturnAnimation(int index)
    {
        if (_animationIndex.TryGetValue(index, out var animation))
        {
            return animation;
        }
        return string.Empty;
    }
}
