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

    [System.Serializable]
    public enum HashtagButtonStatus
    {
        Used,
        Available
    }

    [System.Serializable]
    public enum ObjectPoolTag
    {
        HashtagButton,
    }
}