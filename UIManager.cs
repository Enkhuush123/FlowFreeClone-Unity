using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject nextButton;
public TMP_Text levelText;
public TMP_Text tipText;

    public void ShowNext()
    {
        nextButton.SetActive(true);
    }

    public void HideNext()
    {
        nextButton.SetActive(false);
    }

    public void SetLevelText(int levelIndex) {
        levelText.text = "Level" + (levelIndex + 1) + " / 5";
    }
    public void SetTipText(string tip) {
        tipText.text = "Tip: " + tip; 
    }
}