using UnityEngine;
using UnityEngine.UI;     // 引用介面   API
using UnityEngine.SceneManagement; 
using System.Linq;        // 查詢語言
using System.Collections; // 引用  系統.集合 API - 協同程序

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
    public AudioClip soundRotate;
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
    /// 遊戲是否結束
    /// </summary>
    private bool gameover;

    private AudioSource aud;

    // 開始事件：開始時執行一次
    
    private void Start()
    {
        aud = GetComponent<AudioSource>(); 

        BLOCK();
    }

    /// <summary>
    /// 更新事件：每秒執行約60次
    /// </summary>
    private void Update()
    {

        if (gameover) return; // 如果 遊戲結束 就跳出

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
            if (!tetris.wallRight && !tetris.smallRight)
            {
                {
                    // 按下 D 往右 40 或者 按下 -> 往右40
                    // KeyCode 列舉 (下拉式選單)
                    // || 或者
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        aud.PlayOneShot(soundMove, Random.Range(0.8f, 1.2f));
                        currentTeteris.anchoredPosition += new Vector2(30, 0);
                    }
                }
            }

            // 如果 X座標 大於 -210 才能向左移動
            // if (currentTeteris.anchoredPosition.x > -210) 左為另一個方法，是以座標來定
            if (!tetris.wallLeft && !tetris.smallLeft)
            {
                // 按下 A 往左 -40 或者 按下 <- 往左40
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    aud.PlayOneShot(soundMove, Random.Range(0.8f, 1.2f));
                    currentTeteris.anchoredPosition -= new Vector2(30, 0);
                }
            }

            // 如果  俄羅斯方塊 可以旋轉
            // 按下 W 逆時針轉90度
            if (tetris.canRotate)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    aud.PlayOneShot(soundRotate, Random.Range(0.8f, 1.2f));
                    // 屬性面板上面的 rotation 必須要用 eulerAngles 來做控制
                    currentTeteris.eulerAngles += new Vector3(0, 0, 90);

                    tetris.Offset();
                }
            }

            // 如果按下 S 或者 下 時，方塊落下速度會加速 
            if (!Droptime)
            {
                //aud.PlayOneShot(soundMove, Random.Range(0.8f, 1.2f));

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    droptime = 0.2f;
                }

                //否則 恢復速度
                else
                {
                    droptime = DroptimeMax;
                }
            }
            #endregion

            //如果 目前俄羅斯方塊 Y軸 等於-295 時就 叫下一顆方塊
            //if (currentTeteris.anchoredPosition.y == -295)
            if (tetris.wallDown || tetris.smallBottom)
            {
                SetGround();                     // 設定為地板
                StartCoroutine(CheckTetris());   //檢查並開始消除判定
                StartGame();                    // 生成下顆方塊
                StartCoroutine(shakeEffect());  // 啟動協同程序(協同方法()) - 晃動效果
            }
        }
    }

    /// <summary>
    /// 設定掉落後變為地板
    /// </summary>
    private void SetGround()
    {
        /*
         * 解釋案例
           // 迴圈 for
           // (初始值 ； 條件 ； 迭代器)
           for (int i = 0; i < 10; i++)
           {
               print("迴圈：" + i);
           }
       */

        aud.PlayOneShot(soundfall, Random.Range(0.8f, 1.2f));

        int count = currentTeteris.childCount;                   //取得  目前 方塊  的 子物件數量

        for (int i = 0; i < count; i++)                          //迴圈 執行 子物件數量次數
        {
            currentTeteris.GetChild(i).name = "方塊";            //名稱改為方塊
            currentTeteris.GetChild(i).gameObject.layer = 10;     //圖層改為方塊
        }

        // 將俄羅斯方塊的小方塊搬到 分數區域
        for (int i = 0; i < count; i++)
        {
            currentTeteris.GetChild(0).SetParent(traScoreArea);
        }
        // 刪除父物件
        Destroy(currentTeteris.gameObject);
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
                droptime = 0.025f;
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

    [Header("目前分數")]
    public Text textCurrent;

    [Header("最高分數")]
    public Text textHeight;

    /// <summary>
    /// 遊戲結束
    /// </summary>
    private void Gameover()
    {
        if (!gameover)
        {
            aud.PlayOneShot(soundLose, Random.Range(0.8f, 1.2f));

            gameover = true;            // 遊戲結束
            StopAllCoroutines();        // 停止所有協程
            end.SetActive(true);        // 顯示結束畫面

            textCurrent.text = "目前分數" + nscore;

            // 如果分數 > 本機端紀錄的 最高分數
            if (nscore > PlayerPrefs.GetInt("最高分數"))
            {
                // 更新 本機端紀錄的 最高分數 與 介面
                PlayerPrefs.GetInt("最高分數", nscore);
                textHeight.text = "最高分數" + nscore;
            }

            // 否則  更新最高分數為 本機端紀錄的 最高分數
            else
            {
                textHeight.text = "最高分數" + PlayerPrefs.GetInt("最高分數");
            }
        }
    }

    /// <summary>
    /// 重新遊戲
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("遊戲主畫面");
    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void Nextgame()
    {
        Application.Quit();
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

    /// <summary>
    /// 是否快速落下
    /// </summary>
    private bool Droptime;
    /// <summary>
    /// 分數判定與消除
    /// </summary>
    [Header("分數判定區域")]
    public Transform traScoreArea;

    public RectTransform[] rectSmall;

    /// <summary>
    /// 要刪除的列數
    /// </summary>
    public bool[] destoryRow = new bool[14];

    /// <summary>
    /// 剩下的方塊快要掉落的高度
    /// </summary>
    public float[] downHeight;

    /// <summary>
    /// 檢查方塊是否連線
    /// </summary>
    private IEnumerator CheckTetris()
    {
        // 指定數量與子物件相同
        rectSmall = new RectTransform[traScoreArea.childCount];

        // 利用迴圈將子物件儲存
        for (int i = 0; i < traScoreArea.childCount; i++)
        {
            rectSmall[i] = traScoreArea.GetChild(i).GetComponent<RectTransform>();
            // 取得方塊 Y 軸 檢查是否失敗
            float y = rectSmall[i].anchoredPosition.y;
            // 檢查 Y軸 是否介於 -55 ~ -45 之間
            if (y >= 195 - 5 && y <= 195 + 5) Gameover();
      
        }
        
        int row = 14;                       //總共有的列數(直的)

        for (int i = 0; i < row; i++)
        {
            float bottom = -195;            // 最底層的位置
            float step = 30;                // 每層的間距

            // 查詢有幾顆位置在 -195 +- 5的範圍  - 避免誤差值
            var small = rectSmall.Where(x => x.anchoredPosition.y >= bottom + step * i - 5 && x.anchoredPosition.y <= bottom + step * i + 5);
            // print(small.ToArray().Length);

            // 消除
            if (small.ToArray().Length == 16)
            {
                aud.PlayOneShot(soundfall, Random.Range(0.8f, 1.2f));

                yield return StartCoroutine(shine(small.ToArray()));     // 開始閃爍
                destoryRow[i] = true;                                    // 紀錄要刪除的列數
                AddScore(1000);
            }
        }

        downHeight = new float[traScoreArea.childCount];                // 紀錄有幾顆刪除後剩下的方塊
        for (int i = 0; i < downHeight.Length; i++) downHeight[i] = 0;  // 先將掉落高度歸零

        // 計算每顆剩下方塊要掉落的高度
        for (int i = 0; i < destoryRow.Length; i++)
        {
            if (!destoryRow[i]) continue;                               // 如果 此列 沒有要刪除 就跳過繼續下一列

            for (int x = 0; x < rectSmall.Length; x++)                  // 迴圈 執行 每一顆剩下的方塊
            {
                if (rectSmall[x].anchoredPosition.y > -195 + 30 * i - 5)    // 如果 此方塊 Y 大於 要刪除的行
                {
                    downHeight[x] -= 30;                                // 座標 遞減 30
                }
            }

            destoryRow[i] = false;                                       // 恢復為不消除
        }

        // 更新方塊的高度：往下掉
        for (int i = 0; i < rectSmall.Length; i++)
        {
            rectSmall[i].anchoredPosition += Vector2.up * downHeight[i];
        }
    }

    /// <summary>
    /// 閃爍、消除效果、閃爍後刪除
    /// </summary>
    /// <param name="smalls">要閃爍與消除的列</param>
    /// <returns></returns>

    private IEnumerator shine(RectTransform[] smalls)
    {
        // 閃爍
        float Interval = 0.08f;
        for (int i = 0; i < 16; i++) smalls[i].GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(Interval);
        for (int i = 0; i < 16; i++) smalls[i].GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(Interval);
        for (int i = 0; i < 16; i++) smalls[i].GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(Interval);
        for (int i = 0; i < 16; i++) smalls[i].GetComponent<Image>().enabled = true;

        // 消除
        yield return new WaitForSeconds(Interval);
        for (int i = 0; i < 16; i++) Destroy(smalls[i].gameObject);

        // 重新取得小方塊：避免 Missing 導致的錯誤
        yield return new WaitForSeconds(Interval);

        // 指定數量與子物件相同
        rectSmall = new RectTransform[traScoreArea.childCount];

        // 利用迴圈將子物件儲存
        for (int i = 0; i < traScoreArea.childCount; i++)
        {
            rectSmall[i] = traScoreArea.GetChild(i).GetComponent<RectTransform>();
        }
    }
    [Header("分數文字")]
    public Text textScore;
    [Header("等級文字")]
    public Text textLv;

    private float DroptimeMax = 1.5f;


    /// <summary>
    /// 添加分數
    /// </summary>
    /// <param name="add"></param>
    public void AddScore(int add)
    {
        nscore += add;                           // 分數疊加
        textScore.text = "分數：" + nscore;      // 更新介面

        level = 1 + nscore / 1000;             // 等級公式
        textLv.text = "等級：" + level;      // 更新介面

        DroptimeMax = 1.5f - level / 2;            // 速度提升公式

        DroptimeMax = Mathf.Clamp(DroptimeMax, 0.1f, 99f);

        droptime = DroptimeMax;
    }
}
