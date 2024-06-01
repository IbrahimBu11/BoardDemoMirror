
using System;
using UnityEngine;
using UnityEngine.UI;

public class RoundsView : MonoBehaviour
{
    [SerializeField] private Image[] images;
    private void Awake()
    {
        GameEvents.OnRoundCompleted += OnViewUpdate;
    }

    private void OnViewUpdate(int index)
    {
        foreach (var v in images)
        {
            v.color = Color.white;
        }
        
        for (int i = 0; i < index; i++)
        {
            images[i].color = Color.green;
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnRoundCompleted -= OnViewUpdate;
    }
}
