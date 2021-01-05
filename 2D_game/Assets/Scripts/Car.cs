using UnityEngine;

public class Car : MonoBehaviour
{
    #region 欄位
    //: 單行註解
    // 數值 = 欄位(Field) 
    // 語法如下:
    // 修飾詞 類別 名稱 指定 值(指定跟值可省略); 


    //Unity 常見的4大類型如下:

    //整數(int) : 沒有小數點的數值。ex:1、0、-99等
    //浮點數(float) : 有小數點的數值。ex:1.5、3.14、-1.9等
    //字串(string) : 文字(包含中、英、日文)、特殊字型。ex:BMW、賓士、@#$等
    //布林值(bool) : 有(true)、沒有(false)。用來下指令使用

    //指定符號 「=」 並非數學的等於，應用方式下述

    //修飾詞：
    // 私人(private)：不會顯示在屬性面板上，如果沒有特別輸入就會預設為私人
    // 公開(public) ：會顯示在屬性面板上

    // 符號必定成對，如：()、[]、{}、''、""、<>

    //欄位的屬性：
    // 屬性會寫在類別前一段，後述
    // 標題：Header - 字串
    // 提示：Tooltip - 字串
    // 範圍：Range - 數值，即整數與浮點數，記法為：(最小值,最大值)


    //對應如下:


    [Header("這是車子的尺寸"), Range(50, 150)]
    public int size = 100;
    [Tooltip("這是車子的重量，單位是噸"), Range(0.5f, 3.5f)]
    public float weight = 1.9F; //浮點數後綴需要加F(Float)，大小寫不拘
    [Header("品牌")]
    [Tooltip("這是車子的品牌")]
    public string brand = "BMW"; //字串需要加""包裹住
    [Header("是否有天窗"), Tooltip("這台車有沒有天窗")]
    public bool haswindow = true;

    //其他常用類型：

    // 1. 顏色(Color)

    public Color colorA = Color.blue; //使用內建顏色
    public Color colorB = new Color(0.5f, 0.3f, 0); // 自訂顏色 R，G，B (值必定為0-1)
    public Color colorC = new Color(1, 0, 0, 0.5f); // 自訂顏色 R，G，B，A (值必定為0-1)

    // 2. 向量(Vector) 2維-4維 (2、3維最常使用)

    public Vector2 v2A = Vector2.zero;
    public Vector2 v2B = Vector2.one;
    public Vector2 v2C = new Vector2(1.5f, 99.9f);

    public Vector3 v3A = new Vector3(1, 2, 3);
    public Vector4 v4A = new Vector4(1, 2, 3, 4);

    // 3. 音效片段 AudioClip
    public AudioClip soundExplosion;

    // 4. 圖片 Sprite - 改變 2D 圖片與 介面圖片
    public Sprite spriteA;

    // 5. 遊戲物件與預制物 GameOjbect
    public GameObject goA;
    public GameObject goB;

    // 6. 元件：屬性面板上的粗體字
    public Transform tra;
    public Camera cam;
    #endregion

    [Header("每一顆小方塊的射線長度"), Range(0f, 2f)]
    public float smallcube = 0.5f;
    #region 事件
    //事件：開始事件 - 撥放後執行一次
    #region 

        /// <summary>
        /// 繪製圖示
        /// </summary>
    private void Start()
    {
        print("Hello,world!");

        //取得 欄位 (抓出資料)
        print(size);
        print(brand);

        //設定 欄位 (修改資料)
        weight = 1.3f;
        haswindow = false;

        // 呼叫自訂方法
        // 呼叫方法語法：自訂方法名稱();
        MethodA();
        MethodA();

        // 區域變數
        // 類型 區域變數名稱 指定 值
        // 僅限於此區塊(大括號)中使用
        int intA = MethodB();
        print("傳回整數：" + intA);

        float pi = PI();
        print("拍：" + pi);

        Vector3 v371 = V371();
        print("三維向量 371" + v371);

        MethodC(7);
        MethodC(100);

        float b = BMI(80, 1.75f);
        print("我的BMI" + b);

        Drive(150);
        Drive(90);
        Drive(10, "後方");
    }
    #endregion

    private void OnDrawGizmos()
    {

        #region  每一顆判定

        #endregion
    }
    #endregion


    #region 方法
    // 欄位語法：
    // 修飾詞 類型 欄位名稱 指定 值

    // 方法語法：
    // 修飾詞 傳回類型 方法名稱 () {}
    //無傳回類型 void - 沒有任何傳回資料
    //自訂方法
    // ※ 必須被呼叫才會執行

    private void MethodA()
    {
        print("hello~ I'am MethodA");
    }

    // 如果不是無傳回，必須使用關鍵字「return」傳回，且必須在 return 後加上傳回類型的資料
    private int MethodB()
    {
        return 123;
    }

    private float PI()
    {
        return 3.14f;
    }

    private Vector3 V371()
    {
        return new Vector3(1, 2, 3);
    }

    private void MethodC(int number)
    {
        number += 10;
        print("累加後的數字" + number);
    }

    //參數數量無上限(但一般不會用到太多，參數10份就算很多了)
    private float BMI(float w, float h)
    {
        float bmi = w / (h * h);
        return bmi;
    }

    // 參數的預設值
    // 語法：參數類型 參數名稱 指定 值
    //必須放在右邊

    private void Drive(int speed, string direction = "前方")
    {
        print("時速" + speed);
        print("方向" + direction);
    }

    #endregion
}
