using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;

    public GameObject cellPrefab;
    public GameObject dotPrefab;
    public Transform gridParent;

    private Cell[,] cells;

    private Color gridColor = new Color(0.85f, 0.85f, 0.85f);

    void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        cells = new Cell[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject obj = Instantiate(cellPrefab, gridParent);
                obj.transform.position = new Vector3(c, -r, 0);

                Cell cell = obj.GetComponent<Cell>();
                cell.Init(r, c);

                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                sr.color = gridColor;
                sr.sortingOrder = 0;

                cells[r, c] = cell;
            }
        }
    }

    public Cell GetCell(int r, int c)
    {
        return cells[r, c];
    }

    public void ResetGrid()
    {
        foreach (Cell cell in cells)
        {
            cell.hasDot = false;
            cell.colorId = "";
            cell.pathOwner = "";

            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
            sr.color = gridColor;
        }

        foreach (Dot dot in FindObjectsOfType<Dot>())
        {
            Destroy(dot.gameObject);
        }
    }

    public void CreateDot(string id, Color color, int r, int c)
    {
        GameObject obj = Instantiate(dotPrefab);
        Dot dot = obj.GetComponent<Dot>();
        dot.Init(id, color, GetCell(r, c));
    }

public void LoadLevel(int level)
{
    ResetGrid();

    if (level == 0)
    {


        CreateDot("Red", Color.red, 0, 0);
        CreateDot("Red", Color.red, 2, 0);

        CreateDot("Blue", Color.blue, 0, 1);
        CreateDot("Blue", Color.blue, 0, 4);

        CreateDot("Green", Color.green, 1, 1);
        CreateDot("Green", Color.green, 4, 1);

        CreateDot("Yellow", Color.yellow, 2, 2);
        CreateDot("Yellow", Color.yellow, 4, 2);

        CreateDot("Magenta", Color.magenta, 3, 3);
        CreateDot("Magenta", Color.magenta, 4, 4);
    }
    else if (level == 1)
    {


        CreateDot("Red", Color.red, 0, 0);
        CreateDot("Red", Color.red, 0, 3);

        CreateDot("Blue", Color.blue, 1, 0);
        CreateDot("Blue", Color.blue, 3, 0);

        CreateDot("Green", Color.green, 1, 2);
        CreateDot("Green", Color.green, 4, 2);

        CreateDot("Yellow", Color.yellow, 2, 3);
        CreateDot("Yellow", Color.yellow, 4, 3);

        CreateDot("Magenta", Color.magenta, 0, 4);
        CreateDot("Magenta", Color.magenta, 4, 4);
    }
    else if (level == 2)
    {


        CreateDot("Red", Color.red, 0, 0);
        CreateDot("Red", Color.red, 4, 0);

        CreateDot("Blue", Color.blue, 0, 1);
        CreateDot("Blue", Color.blue, 2, 1);

        CreateDot("Green", Color.green, 0, 2);
        CreateDot("Green", Color.green, 4, 2);

        CreateDot("Yellow", Color.yellow, 1, 3);
        CreateDot("Yellow", Color.yellow, 4, 3);

        CreateDot("Magenta", Color.magenta, 0, 4);
        CreateDot("Magenta", Color.magenta, 4, 4);
    }
    else if (level == 3)
    {
        CreateDot("Red", Color.red, 0, 0);
        CreateDot("Red", Color.red, 0, 2);

        CreateDot("Blue", Color.blue, 0, 3);
        CreateDot("Blue", Color.blue, 1, 4);

        CreateDot("Green", Color.green, 1, 0);
        CreateDot("Green", Color.green, 2, 4);

        CreateDot("Yellow", Color.yellow, 3, 0);
        CreateDot("Yellow", Color.yellow, 3, 4);

        CreateDot("Magenta", Color.magenta, 4, 0);
        CreateDot("Magenta", Color.magenta, 4, 4);
    }
    else if (level == 4)
    {
        CreateDot("Red", Color.red, 0, 0);
        CreateDot("Red", Color.red, 4, 0);

        CreateDot("Blue", Color.blue, 0, 1);
        CreateDot("Blue", Color.blue, 0, 4);

        CreateDot("Green", Color.green, 1, 1);
        CreateDot("Green", Color.green, 4, 1);

        CreateDot("Yellow", Color.yellow, 2, 2);
        CreateDot("Yellow", Color.yellow, 4, 2);

        CreateDot("Magenta", Color.magenta, 3, 3);
        CreateDot("Magenta", Color.magenta, 4, 4);
    }
}
}