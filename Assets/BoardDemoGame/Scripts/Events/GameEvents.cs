using System;

public delegate void OnScoreUpdate(int id,int score);
public delegate void OnRoundIndexUpdate(int index);
public delegate void OnBoardClickIndexUpdate(int x, int y);

public static partial class GameEvents
{
    public static OnScoreUpdate OnPlayerScoreUpdate;
    
    public static OnRoundIndexUpdate OnRoundCompleted;
    public static OnBoardClickIndexUpdate ClickUpdateSync;

    public static Action OnGameStart;
    public static Action OnAllPlayersCConnected;
    public static Action<bool> OnTurnViewUpdate;

}

public static partial class GameEvents
{
    public static class InputEvents
    {
        public static OnBoardClickIndexUpdate OnCellClickedLocal;
    }
}

public static class GameData
{
    public static class MetaData
    {
        public const int TurnCap = 2;
        public const int RoundCap = 6;
        public const int ScorePerClick = 10;
    }

    public static class RunTimeData
    {
        public static int RoundIndex;
        public static int CurrentTurnIndex = 1;
        public static int CurrentClickCount = 0;
    }
}