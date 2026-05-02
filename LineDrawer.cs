using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public Camera cam;

    private bool isDrawing;
    private string currentColorId = "";
    private Color currentColor;
    private Cell lastCell;

    private Color gridColor = new Color(0.85f, 0.85f, 0.85f);

    private List<Cell> currentPath = new List<Cell>();
    private Dictionary<string, List<Cell>> savedPaths = new Dictionary<string, List<Cell>>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartDraw();

        if (Input.GetMouseButton(0) && isDrawing)
            ContinueDraw();

        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }

    void StartDraw()
    {
        Cell cell = GetCellUnderMouse();

        if (cell == null) return;
        if (!cell.hasDot) return;

        currentColorId = cell.colorId;
        currentColor = cell.dotColor;

        ClearPath(currentColorId);

        currentPath.Clear();
        currentPath.Add(cell);

        cell.pathOwner = currentColorId;
        Paint(cell, currentColor);

        lastCell = cell;
        isDrawing = true;
    }

    void ContinueDraw()
    {
        Cell cell = GetCellUnderMouse();

        if (cell == null) return;
        if (cell == lastCell) return;

        if (!IsAdjacent(lastCell, cell))
            return;

        if (currentPath.Contains(cell))
        {
            UndoTo(cell);
            return;
        }

        if (cell.hasDot && cell.colorId != currentColorId)
            return;

        if (cell.pathOwner != "" && cell.pathOwner != currentColorId)
            ClearPath(cell.pathOwner);

        currentPath.Add(cell);
        cell.pathOwner = currentColorId;

        Paint(cell, currentColor);
        lastCell = cell;
    }

    void EndDraw()
    {
        if (!isDrawing) return;

        savedPaths[currentColorId] = new List<Cell>(currentPath);

        isDrawing = false;
        currentColorId = "";
        lastCell = null;
        currentPath.Clear();
    }

    Cell GetCellUnderMouse()
    {
        if (cam == null)
            cam = Camera.main;

        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        Collider2D hit = Physics2D.OverlapPoint(pos);

        if (hit == null) return null;

        return hit.GetComponent<Cell>();
    }

    bool IsAdjacent(Cell a, Cell b)
    {
        int distance = Mathf.Abs(a.row - b.row) + Mathf.Abs(a.col - b.col);
        return distance == 1;
    }

    void UndoTo(Cell target)
    {
        int index = currentPath.IndexOf(target);

        for (int i = currentPath.Count - 1; i > index; i--)
        {
            Cell cell = currentPath[i];
            cell.pathOwner = "";
            ResetCell(cell);
            currentPath.RemoveAt(i);
        }

        lastCell = target;
    }

    public void ClearPath(string id)
    {
        if (!savedPaths.ContainsKey(id)) return;

        foreach (Cell cell in savedPaths[id])
        {
            cell.pathOwner = "";
            ResetCell(cell);
        }

        savedPaths[id].Clear();
    }

    public void ClearAllPaths()
    {
        foreach (var path in savedPaths.Values)
        {
            foreach (Cell cell in path)
            {
                if (cell != null)
                {
                    cell.pathOwner = "";
                    ResetCell(cell);
                }
            }
        }

        savedPaths.Clear();
        currentPath.Clear();
        isDrawing = false;
        currentColorId = "";
        lastCell = null;
    }

    void ResetCell(Cell cell)
    {
        if (cell.hasDot)
            Paint(cell, cell.dotColor);
        else
            Paint(cell, gridColor);
    }

    void Paint(Cell cell, Color color)
    {
        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
        sr.color = color;
    }

    public bool IsPathConnected(string id)
    {
        if (!savedPaths.ContainsKey(id)) return false;

        int dotCount = 0;

        foreach (Cell cell in savedPaths[id])
        {
            if (cell.hasDot && cell.colorId == id)
                dotCount++;
        }

        return dotCount >= 2;
    }
}