using UnityEngine;
using System.Collections; //引用  系統.集合 API - 協同程序

public class TetrisManager : MonoBehaviour
{
    #region 註釋、範圍、修飾詞 類型 欄位名稱...等初階練習 1 
    [Header("這是掉落時間"), Range(0.1f, 3)]
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

    [Header("生成俄羅斯方塊的父物件")]
    public Transform traTetrisPaPa;

    [Header("生成的起始位置")]
    public Vector2[] posSpawn =
    {
        new Vector2(15, 240),
        new Vector2(0, 225),
        new Vector2(0, 225),
        new Vector2(15 , 210),
        new Vector2(15 , 210),
        new Vector2(15, 210),
        new Vector2(0, 210)
    };
    #endregion

    /// <summary>
    /// 下一顆俄羅斯方塊編號
    /// </summary>
    private int indexNext;

    /// <summary>
    /// 目前俄羅斯方塊
    /// </summary>
    private RectTransform currentTeteris;

    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;

    /// <summary>
    /// 開始事件：開始時執行一次
    /// </summary>
    private void Start()
    {
        BLOCK();
    }

    /// <summary>
    /// 更新事件：每秒執行約60次
    /// </summary>
    private void Update()
    {
        ControlTertis();

        FastDown();
    }

    /// <summary>
    /// 控制俄羅斯方塊
    /// </summary>
    private void ControlTertis()
    {
        //如果 已經有 目前的俄羅斯方塊
        if (currentTeteris)
        {//計時器 累加 一幀的時間 - 累加時間
            timer += Time.deltaTime;

            if (timer >= droptime)
            {
                timer = 0;
                currentTeteris.anchoredPosition -= new Vector2(0, 30);
            }
            #region 控制俄羅斯方塊的 左右、旋轉與加速

            Tetris tetris = currentTeteris.GetComponent<Tetris>();

            // 如果 X座標 小於 230 才能向右移動
            // if (currentTeteris.anchoredPosition.x < 230) 左為另一個方法，是以座標來定
            //如果 X 座標 沒有碰到右邊牆壁
            if (!tetris.wallRight)
            {
                {
                    // 按下 D 往右 40 或者 按下 -> 往右40
                    // KeyCode 列舉 (下拉式選單)
                    // || 或者
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        currentTeteris.anchoredPosition += new Vector2(30, 0);
                    }
                }
            }

            // 如果 X座標 大於 -210 才能向左移動
            // if (currentTeteris.anchoredPosition.x > -210) 左為另一個方法，是以座標來定
            if (!tetris.wallLeft)
            {
                // 按下 A 往左 -40 或者 按下 <- 往左40
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentTeteris.anchoredPosition -= new Vector2(30, 0);
                }
            }

            // 如果  俄羅斯方塊 可以旋轉
            // 按下 W 逆時針轉90度
            if (tetris.canRotate)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    // 屬性面板上面的 rotation 必須要用 eulerAngles 來做控制
                    currentTeteris.eulerAngles += new Vector3(0, 0, 90);

                    tetris.Offset();
                }
            }

            // 如果按下 S 或者 下 時，方塊落下速度會加速 
            if (!Droptime)
            {
                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    droptime = 0.2f;
                }

                //否則 恢復速度
                else
                {
                    droptime = 1.0f;
                }
            }
            #endregion

            //如果 目前俄羅斯方塊 Y軸 等於-295 時就 叫下一顆方塊
            //if (currentTeteris.anchoredPosition.y == -295)
            if (tetris.wallDown)
            {
                SetGround();                     // 設定為地板
                StartGame();                    // 生成下顆方塊
                StartCoroutine(shakeEffect());  // 啟動協同程序(協同方法())
            }
        }
    }

    /// <summary>
    /// 設定掉落後變為地板
    /// </summary>
    private void SetGround()
    {
        /*
           // 迴圈 for
           // (初始值 ； 條件 ； 迭代器)
           for (int i = 0; i < 10; i++)
           {
               print("迴圈：" + i);
           }
       */

        int count = currentTeteris.childCount;                   //取得  目前 方塊  的 子物件數量

        for (int i = 0; i < count; i++)                          //迴圈 執行 子物件數量次數
        {
            currentTeteris.GetChild(i).name = "地板";            //名稱改為地板
            currentTeteris.GetChild(i).gameObject.layer = 9;     //圖層改為地板
        }

    }

    #region 方法語法練習 練習 2

    /// <summary>
    /// 生成俄羅斯方塊
    /// 1.隨機顯示下一顆俄羅斯方塊 0 - 6
    /// </summary>
    private void BLOCK()
    {
        // 下一顆編號 = 隨機 的 範圍(最小,最大) - 整數不會等於最大值(最大值+1)
        indexNext = Random.Range(0, 7);

        // 測試
        // indexNext = 0;

        //下一個俄羅斯方塊區域 . 取得子物件(子物件編號) . 轉為遊戲物件 . 啟動設定為(顯示)
        traNaxtAreas.GetChild(indexNext).gameObject.SetActive(true);
    }

    /// <summary>
    /// 開始遊戲
    /// 1. 生成的俄羅斯方塊要放在正確位置
    /// 2.上一次俄羅斯方塊要被隱藏
    /// 3.隨機取得下一個
    /// </summary>
    public void StartGame()
    {
        //碰到地板後，停止快速落下
        Droptime = false;

        // 1. 生成的俄羅斯方塊要放在正確位置
        // 保存上一次的俄羅斯方塊
        GameObject tetris = traNaxtAreas.GetChild(indexNext).gameObject;
        // 生成物件(物件,父物件)
        // 目前俄羅斯方塊 = 生成物件(物件，父物件)
        GameObject current = Instantiate(tetris, traTetrisPaPa);
        // GetComponent<任何元件>()
        // <T>泛型 - 意為所有類型
        // 目前俄羅斯方塊 . 取得元件<介面變形>() . 座標 = 生成座標陣列[編號]
        current.GetComponent<RectTransform>().anchoredPosition = posSpawn[indexNext];

        // 2.上一次俄羅斯方塊要被隱藏
        tetris.SetActive(false);

        // 3.隨機取得下一個
        BLOCK();

        currentTeteris = current.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 是否快速落下
    /// </summary>
    private bool Droptime;

    /// <summary>
    /// 快速落下功能
    /// </summary>
    private void FastDown()
    {
        // 如果有方塊存在於遊戲區
        if (currentTeteris && !Droptime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Droptime = true;
                // 快速掉落的掉落時間
                droptime = 0.018f;
            }
        }
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

    //協同程序
    // IEnumerator 為一個時間的傳回類型 - 暫停某區塊(程式)的設定時間，而非所有程式停止
    private IEnumerator shakeEffect()
    {

        //範本
        // 下面這幾段這段是能夠重複寫的
        //print("協同程序一開始");
        // yield 讓步 - 等待(時間)
        //yield return new WaitForSeconds(1f);
        //print("等待一秒鐘過後...");
        //yield return new WaitForSeconds(2f);
        //print("又過了兩秒！");

        // 取得震動物件效果的 Rect 
        RectTransform rect = traTetrisPaPa.GetComponent<RectTransform>();

        // 晃動 向上 30 > 0 > 20 > 0
        // 等待時間為0.05秒

        float interval = 0.05f;

        rect.anchoredPosition += Vector2.up * 30;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition += Vector2.up * 20;
        yield return new WaitForSeconds(interval);
        rect.anchoredPosition = Vector2.zero;
        yield return new WaitForSeconds(interval);
    }
}
