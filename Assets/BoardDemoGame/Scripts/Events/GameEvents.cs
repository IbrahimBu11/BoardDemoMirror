using System;

public delegate void OnScoreUpdate(int id,int score);
public delegate void OnRoundIndexUpdate(int index);
public delegate void OnBoardClickIndexUpdate(int x, int y);

public static class GameEvents
{
    public static OnScoreUpdate OnPlayerScoreUpdateLocal;
    public static OnScoreUpdate OnPlayerScoreUpdateSync;
    
    public static OnRoundIndexUpdate OnRoundIndexUpdateLocal;
    
    public static OnBoardClickIndexUpdate ClickUpdateLocal;
    public static OnBoardClickIndexUpdate ClickUpdateSync;

    public static Action<bool> OnTurnEnabled;
    public static Action<int,int> OnPlayerJoinIndex;
}
public static class GameData
{
    public static int roundIndex = 0;
}