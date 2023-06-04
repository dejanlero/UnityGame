
namespace MyDice.Players
{
    public enum PlayerState
    {
        Null = 0,
        Idle = 1,
        Moving = 2,
        MovingComplete = 3,
        SelectPath = 4,
        ArrangeMoving = 10,
        ArrangeComplete = 11
    }
}