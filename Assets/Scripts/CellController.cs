using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    [Space]
    [SerializeField]
    Button button;
    [SerializeField]
    GameObject xText;

    public bool canCheckable = false;

    void OnDisable()
    {
        xText.SetActive(false);
        canCheckable = false;
        button.interactable = true;
    }

    public void OnCellClicked()
    {
        button.interactable = false;
        xText.SetActive(true);
        canCheckable = true;

        if (GridController.Instance.CheckForWin())
        {
            GridController.Instance.CreateCells();
        }
    }
}

        