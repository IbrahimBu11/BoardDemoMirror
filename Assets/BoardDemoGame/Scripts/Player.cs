using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
   [SyncVar (hook = nameof(SetPlayerIDClient))] public int index;
   [SyncVar(hook = nameof(SetPlayerScore))] public int score;

   private void Awake()
   {
      GameEvents.InputEvents.OnCellClickedLocal += OnCellClicked;
   }
   private void OnDestroy()
   {
      GameEvents.InputEvents.OnCellClickedLocal -= OnCellClicked;
   }

   private void OnCellClicked(int x, int y)
   {
      if(!isOwned)
         return;
      
      if (isServer)
      {
         score += GameData.MetaData.ScorePerClick;
         GameEvents.OnPlayerScoreUpdate?.Invoke(index, this.score);
      }
      else
      {
         TellServerOfClick();
      }
   }

   [Command]
   private void TellServerOfClick()
   {
      score += GameData.MetaData.ScorePerClick;
   }
   private void SetPlayerScore(int oldScore, int newScore)
   {
         GameEvents.OnPlayerScoreUpdate?.Invoke(index, newScore);
   }
    



   public override void OnStartClient()
   {
      base.OnStartClient();
      if(isServerOnly)
         StartCoroutine(Wait());
   }

   IEnumerator Wait()
   {
      yield return new WaitForSeconds(1);
      index = index;
   }
   
   private void SetPlayerIDClient(int old, int newV)
   {
      index = newV;

      if (isOwned)
         Board.localPlayerID = index;
   }

   [Command(requiresAuthority = false)]
   public void SetPlayerIDServer(int _index)
   {
       this.index = _index;
   }
    
    
}


