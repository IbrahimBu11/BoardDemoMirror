using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : NetworkBehaviour
{
    [SyncVar][SerializeField] private bool isTurnEven;
    
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
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        isLocalTurnEven = localPlayerID % 2 == 0;
        print($"islocal turn even :{localPlayerID} {isLocalTurnEven}");
        blocker.SetActive(isLocalTurnEven);
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

    private void ProcessTurn(int x, int y)
    {
        GameEvents.ClickUpdateSync?.Invoke(x,y);

        GameData.RunTimeData.currentClickCount++;

        if (GameData.RunTimeData.currentClickCount >= GameData.MetaData.TurnCap)
        {
            GameData.RunTimeData.currentTurnIndex++;
            GameData.RunTimeData.currentClickCount = 0;
        }
        
        bool isEven = GameData.RunTimeData.currentTurnIndex % 2 == 0;
        isTurnEven = isEven;
        
        print($"IsLocalPlayer even {isLocalTurnEven} : Turn : {isTurnEven}");
        UpdateClickViewClient(x,y);
        
        if (_boardView.IsGridFinished())
        {
            OnRoundCompletedClient(isTurnEven);
        }
    }

    [ClientRpc]
    private void OnRoundCompletedClient(bool isTurnEven)
    {
        GameData.RunTimeData.RoundIndex++; 
        GameEvents.OnRoundCompleted?.Invoke(GameData.RunTimeData.RoundIndex);
        
        blocker.SetActive(IsMyTurn());
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
        else if(isClient)
            TellServerCellclick(x, y);
        
        
    }
}