using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public Camera cam;
    public GameObject linePrefab;

    private bool isDrawing = false;
    private string currentColorId = "";
    private Color currentColor;

    private Cell startCell;
    private Cell lastCell;

    private LineRenderer currentLine;
    private List<Cell> currentPath = new List<Cell>();

    private Dictionary<string, List<Cell>> savedPaths = new Dictionary<string, List<Cell>>();
    private Dictionary<string, LineRenderer> savedLines = new Dictionary<string, LineRenderer>();

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
        startCell = cell;
        lastCell = cell;

        ClearPath(currentColorId);

        currentPath.Clear();
        currentPath.Add(cell);

        cell.pathOwner = currentColorId;

        GameObject lineObj = Instantiate(linePrefab);
        currentLine = lineObj.GetComponent<LineRenderer>();
        currentLine.startColor = currentColor;
        currentLine.endColor = currentColor;
        currentLine.positionCount = 1;
        currentLine.SetPosition(0, cell.transform.position);

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

        AddPointToLine(cell.transform.position);

        lastCell = cell;
    }

    void EndDraw()
    {
        if (!isDrawing) return;

        bool completed =
            lastCell != null &&
            lastCell.hasDot &&
            lastCell.colorId == currentColorId &&
            lastCell != startCell;

        if (completed)
        {
            savedPaths[currentColorId] = new List<Cell>(currentPath);
            savedLines[currentColorId] = currentLine;
        }
        else
        {
            ClearCurrentDrawing();
        }

        isDrawing = false;
        currentColorId = "";
        startCell = null;
        lastCell = null;
        currentPath.Clear();
        currentLine = null;
    }

    void ClearCurrentDrawing()
    {
        foreach (Cell cell in currentPath)
        {
            if (cell != null && cell.pathOwner == currentColorId)
                cell.pathOwner = "";
        }

        if (currentLine != null)
            Destroy(currentLine.gameObject);
    }

    void AddPointToLine(Vector3 pos)
    {
        currentLine.positionCount++;
        currentLine.SetPosition(currentLine.positionCount - 1, pos);
    }

    void RebuildCurrentLine()
    {
        if (currentLine == null) return;

        currentLine.positionCount = currentPath.Count;

        for (int i = 0; i < currentPath.Count; i++)
        {
            currentLine.SetPosition(i, currentPath[i].transform.position);
        }
    }

    void UndoTo(Cell target)
    {
        int index = currentPath.IndexOf(target);

        for (int i = currentPath.Count - 1; i > index; i--)
        {
            Cell cell = currentPath[i];

            if (cell.pathOwner == currentColorId)
                cell.pathOwner = "";

            currentPath.RemoveAt(i);
        }

        lastCell = target;
        RebuildCurrentLine();
    }

    public void ClearPath(string id)
    {
        if (savedPaths.ContainsKey(id))
        {
            foreach (Cell cell in savedPaths[id])
            {
                if (cell != null && cell.pathOwner == id)
                    cell.pathOwner = "";
            }

            savedPaths[id].Clear();
        }

        if (savedLines.ContainsKey(id))
        {
            if (savedLines[id] != null)
                Destroy(savedLines[id].gameObject);

            savedLines.Remove(id);
        }
    }

    public void ClearAllPaths()
    {
        foreach (string id in new List<string>(savedLines.Keys))
        {
            if (savedLines[id] != null)
                Destroy(savedLines[id].gameObject);
        }

        savedLines.Clear();
        savedPaths.Clear();

        if (currentLine != null)
            Destroy(currentLine.gameObject);

        currentPath.Clear();
        isDrawing = false;
        currentColorId = "";
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
        return Mathf.Abs(a.row - b.row) + Mathf.Abs(a.col - b.col) == 1;
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