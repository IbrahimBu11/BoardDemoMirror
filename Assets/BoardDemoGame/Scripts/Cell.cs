using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Cell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public Button button;
    
    public int x;
    public int y;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        int random = Random.Range(0, 10);
        text.SetText("Sup");
    }
    private void OnClick()
    {
        GameEvents.ClickUpdateLocal?.Invoke(x, y);
        GameEvents.OnPlayerScoreUpdateLocal.Invoke(0,10);
        button.interactable = false;
    }
}
