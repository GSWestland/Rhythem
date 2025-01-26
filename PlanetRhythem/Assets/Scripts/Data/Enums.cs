/// <summary>
/// The desired hand that the player should be using for a given situation.
/// </summary>
public enum DesiredHand
{
    Left,
    Right
}

/// <summary>
/// The desired play position the player will be in while playing the game.
/// </summary>
public enum PlayPosition
{
    Seated,
    Standing
}

/// <summary>
/// The possible types of note that the player could encounter in the game
/// </summary>
public enum NoteType
{
    Note,
    Obstacle,
    NoteHoldStart,
    NoteHoldEnd
}

public enum ScoreZone
{
    Miss,
    Close,
    Good,
    Great,
    Stellar
}
