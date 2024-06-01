using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : NetworkBehaviour
{
    [SyncVar (hook = nameof(SetTurnVal))][SerializeField] private bool isTurnEven;
    
    public static int localPlayerID;
    [SerializeField] private bool isLocalTurnEven;

    [SerializeField] private BoardView _boardView;
    [SerializeField] private GameObject blocker;
    
    public void Awake()
    {
        GameEvents.InputEvents.OnCellClickedLocal += OnCellClickedLocal;
    }
    public  void OnDestroy()
    {
        GameEvents.InputEvents.OnCellClickedLocal -= OnCellClickedLocal;
    }

    public override void OnStartClient()
    {
        StartCoroutine(Wait());
        bool isEven = GameData.RunTimeData.CurrentTurnIndex % 2 == 0;
        isTurnEven = isEven;
        
        GameEvents.OnTurnViewUpdate?.Invoke(isTurnEven);
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        isLocalTurnEven = localPlayerID % 2 == 0;
        
        blocker.SetActive(IsMyTurn());
    }
    
    
    [Command(requiresAuthority = false)]
    private void TellServerCellclick(int x, int y)
    {
        ProcessTurn(x,y);
    }

    [ClientRpc]
    private void UpdateClickViewClient(int x,int y)
    {
        GameEvents.ClickUpdateSync?.Invoke(x,y);
    }

    private void SetTurnVal(bool oldVal, bool newVal)
    {
        isTurnEven = newVal;
        blocker.SetActive(IsMyTurn());
        GameEvents.OnTurnViewUpdate?.Invoke(isTurnEven);
    }

    private void ProcessTurn(int x, int y)
    {
        GameEvents.ClickUpdateSync?.Invoke(x,y);

        GameData.RunTimeData.CurrentClickCount++;

        if (GameData.RunTimeData.CurrentClickCount >= GameData.MetaData.TurnCap)
        {
            GameData.RunTimeData.CurrentTurnIndex++;
            GameData.RunTimeData.CurrentClickCount = 0;
        }
        
        bool isEven = GameData.RunTimeData.CurrentTurnIndex % 2 == 0;
        isTurnEven = isEven;
        
        UpdateClickViewClient(x,y);
        UpdateTurn();
        if (_boardView.IsGridFinished())
        {
            OnRoundCompletedClient();
            
        }
    }

    [ClientRpc]
    private void UpdateTurn()
    {
        blocker.SetActive(IsMyTurn());
        GameEvents.OnTurnViewUpdate?.Invoke(isTurnEven);
        print($"Is My turn : {IsMyTurn()} : {GameData.RunTimeData.CurrentClickCount} " +
              $"{GameData.RunTimeData.CurrentTurnIndex}");
    }

    [ClientRpc]
    private void OnRoundCompletedClient()
    {
        GameData.RunTimeData.RoundIndex++; 
        if (GameData.RunTimeData.RoundIndex > GameData.MetaData.RoundCap)
            GameData.RunTimeData.RoundIndex = 0;
        GameEvents.OnRoundCompleted?.Invoke(GameData.RunTimeData.RoundIndex);
        
    }

    private bool IsMyTurn()
    {
        return isLocalTurnEven == isTurnEven;
    }

    private void OnCellClickedLocal(int x, int y)
    {
        if (isServer)
        {
            ProcessTurn(x,y);
        }
        else
            TellServerCellclick(x, y);
        
        
    }
}