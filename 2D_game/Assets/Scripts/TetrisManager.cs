using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    #region 註釋、範圍、修飾詞 類型 欄位名稱...等初階練習 1 
    [Header("這是掉落時間"), Range(0.1f,3)]
    public float droptime = 1.5f;

    [Header("這是目前分數")]
    public int nscore;

    [Header("這是最高分數")]
    public int hscore;

    [Header("這是等級")]
    public int level = 1;

    [Header("結束畫面")]
    public GameObject end;

    [Header("這是掉落音效")]
    public AudioClip dropsound;

    [Header("這是移動及旋轉音效")]
    public AudioClip spinound;

    [Header("這是消除音效")]
    public AudioClip removesound;

    [Header("這是結束音效")]
    public AudioClip oversound;

    #endregion



    #region 方法語法練習 練習 2

    /// <summary>
    /// 生成俄羅斯方塊
    /// </summary>
    private void BLOCK()
    {
       
    }

    /// <summary>
    /// 添加分數
    /// </summary>
    /// <param name="score">整數</param>
    public void Pluscore(int score)
    {
   
    }

    /// <summary>
    /// 遊戲時間
    /// </summary>
    private void Gametime()
    {
    
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    private void Gameover()
    {
      
    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void Restart()
    {

    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Nextgame()
    {
 
    }
    #endregion
}
