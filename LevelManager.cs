using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GridManager gridManager;
    public LineDrawer lineDrawer;
    public UIManager uiManager;
    public WinChecker winChecker;
   private string[] tips =
{
    "Хамгийн ойр цэгүүдээс эхэл.",
    "Эхлээд захын замуудаа холбо.",
    "Зөвхөн холбох биш бүх нүдийг дүүргэх хэрэгтэй.",
    "Хэрвээ замууд хаагдвал дахин зур.",
    "Бүх нүдийг ашиглах ёстойг сана."
};

    private int currentLevel = 0;
    private int failCount = 0;

    IEnumerator Start()
    {
        yield return null;
        LoadCurrentLevel();
    }

    void LoadCurrentLevel()
    {
        uiManager.HideNext();
        uiManager.SetLevelText(currentLevel);
        uiManager.SetTipText(tips[currentLevel]);
        winChecker.ResetWin();
        lineDrawer.ClearAllPaths();
        gridManager.LoadLevel(currentLevel);
    }

    public void NextLevel()
    {
        currentLevel++;

        if (currentLevel >= 5)
            currentLevel = 0;

        LoadCurrentLevel();
    }

    public void ClearLevel()
    {
        LoadCurrentLevel();
    }
}