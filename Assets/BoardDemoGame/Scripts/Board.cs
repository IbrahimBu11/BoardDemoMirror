using Mirror;
using UnityEngine;

public class Board : NetworkBehaviour
{
    [SyncVar (hook = nameof(OnCellClicked))] [SerializeField] private Vector2 cellClicked;
    [SyncVar (hook = nameof(OnRoundCompleted))] [SerializeField] private int RoundIndex;
    [SyncVar (hook = nameof(OnIndexChanged))] [SerializeField] private int currentIndex;

    public int turnCap = 0;

    public static int localPlayerID;
    private bool _isLocalTurnEven;

    [SerializeField] private BoardView _boardView;
    [SerializeField] private GameObject blocker;
    
    public void Awake()
    {
        GameEvents.ClickUpdateLocal += OnCellClickedBroadCast;
        GameEvents.OnRoundIndexUpdateLocal += OnRoundCompletedlocal;
    }

    private void Start()
    {
        _isLocalTurnEven = localPlayerID % 2 == 0;
        blocker.SetActive(_isLocalTurnEven);
    }

    private void OnRoundCompletedlocal(int index)
    {
        if(isServer)
            RoundIndex = index;
    }

    public  void OnDestroy()
    {
        GameEvents.ClickUpdateLocal -= OnCellClickedBroadCast;
        GameEvents.OnRoundIndexUpdateLocal -= OnRoundCompletedlocal;
    }
    private void OnRoundCompleted(int old, int newV)
    {
        GameEvents.OnRoundIndexUpdateLocal.Invoke(newV);
    }
    
    private void OnCellClicked(Vector2 OldVal, Vector2 newVal)
    {
        GameEvents.ClickUpdateSync?.Invoke((int)newVal.x,(int)newVal.y);
        
        if (!_boardView.IsGridFinished()) return;
        GameData.roundIndex++; 
        GameEvents.OnRoundIndexUpdateLocal?.Invoke(GameData.roundIndex);
    }

    private void OnIndexChanged(int oldValue, int newValue)
    {
        bool isEven = newValue % 2 == 0;
        blocker.SetActive(!isEven && !_isLocalTurnEven);

        
        if (turnCap < 2) return;
        turnCap = 0;
        currentIndex++;
    }


    [Command(requiresAuthority = false)]
    private void TellServerCellclick(Vector2 newVal)
    {
        print($"On Cell clicked Working {newVal}");
        GameEvents.ClickUpdateSync?.Invoke((int)newVal.x,(int)newVal.y);

        if (_boardView.IsGridFinished())
        {
            GameData.roundIndex++; 
            GameEvents.OnRoundIndexUpdateLocal?.Invoke(GameData.roundIndex);
        }
    }

    private void OnCellClickedBroadCast(int x, int y)
    {
        if(isServer)
            cellClicked = new Vector2(x,y);
        else
            TellServerCellclick(new Vector2(x, y));
        
        turnCap++;
    }
}