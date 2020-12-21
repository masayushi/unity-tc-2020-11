
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    // public 公開，允許按鈕呼叫

    /// <summary>
    /// 延遲呼叫，等音效播完
    /// </summary>
    
    public void DelayStartGame()
    {
        // 延遲呼叫("方法名稱"，延遲秒數)
        // Invoke();
        Invoke("Gamestart", 0.8f);
    }

    /// <summary>
    /// 延遲呼叫，等音效播完
    /// </summary>
    
    public void DelayQuitgame()
    {
        Invoke("QuitGame", 0.8f);
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void Gamestart()
    {
        // 載入指定場景
        // 場景管理器 的 載入場景("場景名稱")
        // 場景管理器 的 載入場景(1)
        SceneManager.LoadScene("遊戲主畫面");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void Quitgame()
    {
        //退出遊戲
        //語法：應用程式.離開遊戲(); -> 應用程式 的 離開遊戲 
        Application.Quit();
    }
}