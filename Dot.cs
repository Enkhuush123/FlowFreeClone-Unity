using UnityEngine;

public class Dot : MonoBehaviour
{
    public string colorId;
    public Color dotColor;
    public Cell cell;

    public void Init(string id, Color color, Cell targetCell)
    {
        colorId = id;
        dotColor = color;
        cell = targetCell;

        cell.hasDot = true;
        cell.colorId = id;
        cell.dotColor = color;

        transform.position = cell.transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = color;
        sr.sortingOrder = 5;
    }
}