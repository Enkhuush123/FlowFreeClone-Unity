using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    public LineDrawer lineDrawer;
    public UIManager uiManager;

    private bool won = false;

    void Update()
    {
        if (won) return;

        if (CheckWin())
        {
            won = true;
            uiManager.ShowNext();
        }
    }

 bool CheckWin()
{
    Dot[] dots = FindObjectsOfType<Dot>();

    if (dots.Length == 0)
        return false;

    HashSet<string> colors = new HashSet<string>();

    foreach (Dot dot in dots)
        colors.Add(dot.colorId);

    foreach (string id in colors)
    {
        if (!lineDrawer.IsPathConnected(id))
            return false;
    }

    Cell[] cells = FindObjectsOfType<Cell>();

    foreach (Cell cell in cells)
    {
        if (cell.pathOwner == "")
            return false;
    }

    return true;
}

    public void ResetWin()
    {
        won = false;
    }
}