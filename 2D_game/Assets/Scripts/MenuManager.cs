
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    /// public 公開，允許按鈕呼叫
    public void DelayStartGame()
    {
        ///延遲呼叫("方法名稱"，延遲秒數
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void Gamestart()
    {
        SceneManager.LoadScene("遊戲主畫面");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Quitgame()
    {
        //退出遊戲
        //語法：應用程式.離開遊戲()
        Application.Quit();
    }
}
