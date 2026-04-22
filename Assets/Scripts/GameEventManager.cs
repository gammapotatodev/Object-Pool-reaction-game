using System;

public static class GameEventManager
{
    public static Action<NoteType, HitResult> OnHit;
    public static Action OnGameOver;

    public static void Reset()
    {
        OnHit = null;
        OnGameOver = null;
    }
}

public enum HitResult
{
    Perfect,
    Good,
    Bad,
    Miss
}
