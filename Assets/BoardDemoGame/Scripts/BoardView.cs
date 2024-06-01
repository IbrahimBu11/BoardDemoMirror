using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private GameObject CellFab;
    [SerializeField] private Transform GridParent;

    private List<List<Cell>> Cells = new();

    private void Awake()
    {
        GameEvents.ClickUpdateSync += OnCellClick;
        GameEvents.OnGameStart += GenerateGridStart;
        GameEvents.OnRoundCompleted += GenerateGrid;
    }

    private void GenerateGridStart()
    {
        GenerateGrid(0);
    }

    public bool IsGridFinished()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(Cells[i][j].button.interactable)
                    return false;
            }
        }

        return true;
    }

    private void OnDestroy()
    {
        GameEvents.ClickUpdateSync -= OnCellClick;
        GameEvents.OnGameStart -= GenerateGridStart;
        GameEvents.OnRoundCompleted -= GenerateGrid;
    }

    private void GenerateGrid(int x)
    {
        ClearView();
        
        for (int i = 0; i < 2; i++)
        {
            Cells.Add(new List<Cell>());
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Cells[i].Add(Instantiate(CellFab, GridParent).GetComponent<Cell>());
                Cells[i][j].x = i;
                Cells[i][j].y = j;
            }
        }

    }

    private void ClearView()
    {

        if (Cells.Count <= 0) return;
        
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Destroy(Cells[i][j].gameObject);
            }
        }
        
        Cells.Clear();
        print("Working");
    }

    private void OnCellClick(int x, int y)
    {
        Cells[x][y].button.interactable = false;
    }
}
