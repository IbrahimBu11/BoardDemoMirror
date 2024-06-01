
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private GameObject highlight;
    [SerializeField] private TextMeshProUGUI score;

    private void Awake()
    {
        GameEvents.OnPlayerScoreUpdate += OnScoreUpdate;
        GameEvents.OnTurnViewUpdate += OnTurnViewUpdate;
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerScoreUpdate -= OnScoreUpdate;
        GameEvents.OnTurnViewUpdate -= OnTurnViewUpdate;
    }

    private void OnTurnViewUpdate(bool val)
    {
        bool isEven = index == 2;
        highlight.SetActive(isEven != val);
    }


    private void OnScoreUpdate(int _index,int _score)
    {
        if(index != _index)
            return;
        
        this.score.SetText(_score.ToString());
    }
}
