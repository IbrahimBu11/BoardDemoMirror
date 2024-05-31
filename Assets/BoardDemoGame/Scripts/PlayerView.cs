
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] public int id;

    private void Awake()
    {
        GameEvents.OnPlayerScoreUpdateSync += OnScoreUpdate;  
        GameEvents.OnPlayerJoinIndex += OnPlayerJoin;  
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerScoreUpdateSync -= OnScoreUpdate;
        GameEvents.OnPlayerJoinIndex -= OnPlayerJoin;  
    }

    private void OnPlayerJoin(int _id, int _index)
    {
        if(index != _index)
            return;
        
        id = _id;
    }


    private void OnScoreUpdate(int _id,int _score)
    {
        //if(_id != id)
           // return;
        
        this.score.SetText(_score.ToString());
    }
}
