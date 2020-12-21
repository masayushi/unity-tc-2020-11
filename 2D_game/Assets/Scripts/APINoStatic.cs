using UnityEngine;

public class APINoStatic : MonoBehaviour
{
    // 一般屬性的使用方式
    // 類型  欄位屬性
    public Transform traA;
    public Transform traB;

    public GameObject myObject;

    public Transform myTra;

    private void Start()
    {
        // 一般屬性的取得
        // 語法如下：
        // 欄位屬性名稱 .(的) 一般屬性
        print("物件A的座標" + traA.position);

        // 一般屬性的設定
        // 語法如下：
        // 欄位屬性名稱 .(的) 一般屬性 =(指定) 對應的值;
        traB.position = new Vector3(1, 2, 3);

        print("我的物件座標為：" + myObject.layer);

        myObject.layer = 4;
    }

        // Update：一秒執行60次事件
        private void Update()
        {
            // 一般方法的使用，語法如下
            // 語法：類型欄位名稱 的 方法(對應的參數)
            myTra.Rotate(0, 0, 1);
            myTra.Translate(1, 0, 0);
        }
}
