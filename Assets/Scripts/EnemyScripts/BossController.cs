using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public static bool winTriggered = false;

    void Start()
    {
        winTriggered = false;
    }

    public void TriggerWin()
    {
        if (winTriggered) return;
        winTriggered = true;
        WinScreenController winScreen = FindFirstObjectByType<WinScreenController>();
        if (winScreen != null)
            winScreen.StartCoroutine(Win(winScreen));
        else
            Debug.LogWarning("WinScreenController not found in scene.");
    }

    IEnumerator Win(WinScreenController winScreen)
    {
        yield return new WaitForSeconds(2f);
        winScreen.ShowWinPanel();
    }
}