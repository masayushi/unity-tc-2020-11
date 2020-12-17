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
    public AudioClip soundfall;
    public AudioClip soundMove;
    public AudioClip soundClear;
    public AudioClip soundLose;

    [Header("下一顆俄羅斯方塊區域")]
    public Transform traNaxtAreas;
    #endregion

    /// <summary>
    /// 下一顆俄羅斯方塊編號
    /// </summary>
    public int indexNext;

    private void Start()
    {
        BLOCK();
    }

    #region 方法語法練習 練習 2

    /// <summary>
    /// 生成俄羅斯方塊
    /// 1.隨機顯示下一顆俄羅斯方塊 0 - 6
    /// </summary>
    private void BLOCK()
    {
        //下一顆編號 = 隨機 的 範圍(最小,最大) - 整數不會等於最大值(最大值+1)
        indexNext = Random.Range(0, 7);

        //下一個俄羅斯方塊區域 . 取得子物件(子物件編號) . 轉為遊戲物件 . 啟動設定為(顯示)
        traNaxtAreas.GetChild(indexNext).gameObject.SetActive(true);
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartGame()
    {

        //保存上一次的俄羅斯方塊
        GameObject tetris = traNaxtAreas.GetChild(indexNext).gameObject;
        //生成物件(物件)
        Instantiate(tetris);
    }


    /// <summary>
    /// 添加分數
    /// </summary>
    /// <param name="score">要添加的分數</param>
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
