
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void Gamestart()
    {
        SceneManager.LoadScene("場景");
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
