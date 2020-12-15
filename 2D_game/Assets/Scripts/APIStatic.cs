using UnityEngine;

/// <summary>
/// 認識靜態API
/// 靜態的英文是 Static
/// </summary>
public class APIStatic : MonoBehaviour
{
    /// <summary>
    /// 開始事件：播放後執行一次
    /// </summary>
   private void Start()
    {
        //靜態屬性的取得
        //語法：類別名稱.靜態屬性名稱
        print(Mathf.PI);
        print(Mathf.Infinity);

        //設定
        //如果有出現 Read Only 代表不能設定 ex:Static Properties
        //語法：類別名稱.靜態屬性名稱 = 相同屬性的值(記得，有浮點數要加f)
        Time.timeScale = 0.5f;

        //練習
        print("所有攝影機的數量" + Camera.allCamerasCount);
        Cursor.visible = false;


        //靜態方法 (Static Method) 的使用
        //語法：類別名稱.靜態方法名稱()<---括號裡放對應的參數
        int number = Mathf.Abs(-999);
        print("取得絕對值後的結果：" + number);

        //練習
        print("隨機範圍3 - 20.5：" + Random.Range(3.0f, 20.5f));
        print("隨機範圍1 - 100：" + Random.Range(1, 100));

        //Application.OpenURL("https://store.unity.com/");
        print("7.7去小數點：" + Mathf.Floor(7.7f));
    }

    /// <summary>
    /// 更新事件(Update)：一秒執行約60次(FPS)
    /// </summary>
    private void Update()
    {
        //練習
        //print("是否按任意鍵" + Input.anyKey);
        //print("遊戲時間" + Time.time);
        print("是否按下空白鍵" + Input.GetKeyDown("space"));
    }
}
