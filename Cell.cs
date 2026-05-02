using UnityEngine;

public class Cell : MonoBehaviour
{
    public int row;
    public int col;

    public bool hasDot;
    public string colorId = "";
    public Color dotColor;

    public string pathOwner = "";

    public void Init(int r, int c)
    {
        row = r;
        col = c;
        hasDot = false;
        colorId = "";
        pathOwner = "";
    }
}