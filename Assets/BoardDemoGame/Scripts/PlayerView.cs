
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] public int id;

    private void Awake()
    {
        GameEvents.OnPlayerScoreUpdate += OnScoreUpdate;
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerScoreUpdate -= OnScoreUpdate;
    }
    


    private void OnScoreUpdate(int _index,int _score)
    {
        if(index != _index)
            return;
        
        this.score.SetText(_score.ToString());
    }
}
