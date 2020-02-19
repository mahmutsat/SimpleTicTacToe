using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [Space]
    [SerializeField]
    int rowCount = 3;
    [SerializeField]
    TextMeshProUGUI rowCountText;

    public static GridController Instance;

    GridLayoutGroup gridLayoutGroup;
    RectTransform rectTransform;

    List<CellController> cells = new List<CellController>();

    const int MAX_ROW_COUNT = 10;
    const int MIN_ROW_COUNT = 3;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        CreateCells();
        rowCountText.text = rowCount.ToString();
    }

    public void CreateCells()
    {
        var cellSize = rectTransform.rect.width / rowCount;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

        ClearCells();

        for (int i = 0; i < Mathf.Pow(rowCount, 2); i++)
        {
            var cell = ObjectManager.Instance.Spawn("Cell", this.transform,Vector3.zero,Quaternion.identity);
            var cellController = cell.GetComponent<CellController>();
            cells.Add(cellController);
        }
    }

    public void IncreaseRowCount()
    {
        if (rowCount >= MAX_ROW_COUNT)
            return;

        rowCount++;
        rowCountText.text = rowCount.ToString();
    }

    public void DecreaseRowCount()
    {
        if (rowCount <= MIN_ROW_COUNT)
            return;

        rowCount--;
        rowCountText.text = rowCount.ToString();
    }

    public bool CheckForWin()
    {
        for (int firstObjectIndexInRow = 0; firstObjectIndexInRow <= rowCount * (rowCount - 1); firstObjectIndexInRow += rowCount)
        {
            // Horizontal Check
            for (int column = 0; column < rowCount - 2; column++)
            {
                var index = firstObjectIndexInRow + column;

                if (!CheckValues(index, index + 1) || !CheckValues(index, index + 2))
                    continue;

                return true;
            }

            // Vertical Check
            if (firstObjectIndexInRow < rowCount * (rowCount - 2))
            {
                for (int column = 0; column < rowCount; column++)
                {
                    var index = firstObjectIndexInRow + column;

                    if (!CheckValues(index, index + rowCount) || !CheckValues(index, index + rowCount * 2))
                        continue;

                    return true;
                }
            }

            // L Check
            if (firstObjectIndexInRow < rowCount * (rowCount - 1))
            {
                for (int column = 0; column < rowCount; column++)
                {
                    var index = firstObjectIndexInRow + column;

                    if ((CheckValues(index, index + rowCount) && CheckValues(index, index + rowCount + 1)) 
                        || (CheckValues(index, index + rowCount - 1) && CheckValues(index, index + rowCount))
                        || (CheckValues(index, index + 1) && CheckValues(index, index + rowCount)) 
                        || (CheckValues(index, index + 1) && CheckValues(index, index + rowCount + 1)))
                        return true;
                }
            }
        }

        return false;
    }

    bool CheckValues(int firstIndex, int secondIndex)
    {
        if (firstIndex < cells.Count && secondIndex < cells.Count)
            return cells[firstIndex].canCheckable && cells[secondIndex].canCheckable;
        else
            return false;
    }

    void ClearCells()
    {
        if (cells.Count == 0)
            return;

        foreach (var item in cells)
            ObjectManager.Instance.Destroy(item.gameObject, "Cell");

        cells.Clear();
    }
}
