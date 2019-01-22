namespace InstaGen
{
    [System.Serializable]
    public enum MoveDirection
    {
        NoMovement,
        Left,
        Right,
        Up,
        Down
    }

    public enum HashtagButtonStatus
    {
        Used,
        Available
    }

    public enum ObjectPoolTag
    {
        HashtagButton,
    }
}