using UnityEngine;
// 引用  系統  查詢語言的 API - 偵測陣列與清單內的資料
using System.Linq;

public class Tetris : MonoBehaviour
{
    #region 欄位
    [Header("角度為0時，輔助線的長度")]
    public float cube0;

    [Header("角度為90時，輔助線的長度")]
    public float cube90;

    [Header("旋轉位移左右")]
    public int offsetX;

    [Header("旋轉位移上下")]
    public int offsetY;

    [Header("偵測水平是否能旋轉")]
    public float cubeRotate0r;
    public float cubeRotate0l;
    [Header("偵測垂直是否能旋轉")]
    public float cubeRotate90r;
    public float cubeRotate90l;
    #endregion 
    [Header("每一顆小方塊的射線長度"), Range(0f, 2f)]
    public float smallcube = 0.5f;

    #region 事件
    /// <summary>
    /// 紀錄目前射線長度
    /// </summary>
    private float cube;
    private float cubeDown;
    private float cubeRotateR;
    private float cubeRotateL;

    /// <summary>
    /// 繪製圖示(感應邊界的輔助線)
    /// </summary>
    private void OnDrawGizmos()
    {
        #region 判定牆壁與地板
        //圖示輔助線  的  顏色
        Gizmos.color = Color.red;

        //將浮點數 轉變為 整數 - 要把小數點去掉，就在類型前加(int)
        int z = (int)transform.eulerAngles.z;

        if (z == 0 || z == 180)
        {
            //儲存目前長度
            cube = cube0;

            // 圖示  的  繪製射線(起點, 方向)
            //右側如下
            Gizmos.DrawRay(transform.position, Vector3.right * cube0);

            //左側如下
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, -Vector3.right * cube0);

            //下側線條
            cubeDown = cube90;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, -Vector3.up * cube90);

            //垂直時偵測感應牆壁的線條
            cubeRotateR = cubeRotate0r;
            cubeRotateL = cubeRotate0l;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Vector3.right * cubeRotate0r);
            Gizmos.DrawRay(transform.position, -Vector3.right * cubeRotate0l);
        }
        else if (z == 90 || z == 270)
        {
            // 儲存目前長度
            //右側如下
            cube = cube90;

            Gizmos.DrawRay(transform.position, Vector3.right * cube90);

            //左側如下
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, -Vector3.right * cube90);

            //下側線條
            cubeDown = cube0;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, -Vector3.up * cube0);

            // 水平時偵測牆壁感應的線條
            cubeRotateR = cubeRotate90r;
            cubeRotateL = cubeRotate90l;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Vector3.right * cubeRotate90r);
            Gizmos.DrawRay(transform.position, -Vector3.right * cubeRotate90l);
        }
        #endregion
        #region 每一顆方塊判定
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.down * smallcube);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.right * smallcube);
            Gizmos.color = Color.gray;
            Gizmos.DrawRay(transform.GetChild(i).position, Vector2.left * smallcube);
        }
        #endregion
    }


    private void Start()
    {
        // 儲存遊戲開始的角度
        cube = cube0;
        //UI一律是用RectTransform
        rect = GetComponent<RectTransform>();

        //偵測到幾個子物件(小方塊)，就新增幾個陣列
        smallRightAll = new bool[transform.childCount];
        smallLeftAll = new bool[transform.childCount];
    }

    private void Update()
    {
        CheckWall();
        CheckBottom();
        CheckLeftAndRight();
    }
    #endregion

    /// <summary>
    /// 小方塊底部碰撞
    /// </summary>
    public bool smallBottom;

    /// <summary>
    /// 右邊是否有方塊
    /// </summary>
    public bool smallRight;


    /// <summary>
    /// 所有小方塊右邊是否有其他方塊
    /// </summary>
    public bool[] smallRightAll;

    /// <summary>
    /// 左邊是否有方塊
    /// </summary>
    public bool smallLeft;

    /// <summary>
    /// 所有小方塊左邊是否有方塊
    /// </summary>
    public bool[] smallLeftAll;

    private void CheckLeftAndRight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RaycastHit2D hitR = Physics2D.Raycast(transform.GetChild(i).position, Vector3.right, smallcube, 1 << 10);
            // 如果   右邊有方塊  就  勾選(判定)與陣列對應的格子
            if (hitR && hitR.collider.name == "方塊") smallRightAll[i] = true;
            else smallRightAll[i] = false;

            RaycastHit2D hitL = Physics2D.Raycast(transform.GetChild(i).position, Vector3.left, smallcube, 1 << 10);
            // 如果   左邊有方塊  就  勾選(判定)與陣列對應的格子
            if (hitL && hitL.collider.name == "方塊") smallLeftAll[i] = true;
            else smallLeftAll[i] = false;
        }

        // 檢查陣列內 等於 true 的資料
        // 陣列.哪裡(代名詞 => 條件)
        // var 為 無類型
        var allRight = smallRightAll.Where(x => x == true);
        smallRight = allRight.ToArray().Length > 0;
        //轉為陣列.數量
        //print(smallRightAll.ToArray().Length);
        var allLeft = smallLeftAll.Where(x => x == true);
        smallLeft = allLeft.ToArray().Length > 0;
    }
    /// <summary>
    /// 檢查底部是否有其他方塊
    /// </summary>
    private void CheckBottom()
    {
        //迴圈執行每一顆小方塊
        for (int i = 0; i < transform.childCount; i++)
        {
            //每一個小方塊 射線(每一顆小方塊的中心點，向下，長度，圖層)
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(i).position, Vector3.down, smallcube, 1 << 10);
            if (hit && hit.collider.name == "方塊") smallBottom = true;
        }
    }
    #region 方法
    /// <summary>
    /// 是否碰到牆壁的右邊
    /// </summary>
    public bool wallRight;

    /// <summary>
    /// 是否碰到牆壁的左邊
    /// </summary>
    public bool wallLeft;

    /// <summary>
    /// 是否碰到地板
    /// </summary>
    public bool wallDown;

    /// <summary>
    /// 是否能旋轉
    /// </summary>
    public bool canRotate = true;

    //
    private RectTransform rect;

    /// <summary>
    /// 檢查射線是否碰到牆壁
    /// </summary>
    private void CheckWall()
    {
        //2D 射線碰撞資訊 區域變數名稱 = 2D 物理.射線碰撞(起點，方向，長度，圖層)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, cube, 1 << 8);

        if (hit && hit.transform.name == "牆壁(右)")
        {
            wallRight = true;
        }
        else
        {
            wallRight = false;
        }

        //旋轉射線的判定
        RaycastHit2D hitRotateR = Physics2D.Raycast(transform.position, Vector3.right, cubeRotateR, 1 << 8);
        RaycastHit2D hitRotateL = Physics2D.Raycast(transform.position, -Vector3.right, cubeRotateL, 1 << 8);
        if (hitRotateR && hitRotateR.transform.name == "牆壁(右)" || hitRotateL && hitRotateL.transform.name == "牆壁(左)")
        {
            canRotate = false;
        }
        else
        {
            canRotate = true;
        }

        //2D 射線碰撞資訊 區域變數名稱 = 2D 物理.射線碰撞(起點，方向，長度，圖層)
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, -Vector3.right, cube, 1 << 8);

        if (hitL && hitL.transform.name == "牆壁(左)")
        {
            wallLeft = true;
        }
        else
        {
            wallLeft = false;
        }

        //2D 射線碰撞資訊 區域變數名稱 = 2D 物理.射線碰撞(起點，方向，長度，圖層)
        RaycastHit2D hitD = Physics2D.Raycast(transform.position, -Vector3.up, cubeDown, 1 << 9);
        if (hitD && hitD.transform.name == "地板")
        {
            wallDown = true;
        }
        else
        {
            wallDown = false;
        }
    }

    /// <summary>
    /// 旋轉後位移處理
    /// </summary>
    public void Offset()
    {
        // 將浮點數角度 轉為 整數 - 去小數點
        int z = (int)transform.eulerAngles.z;

        if (z == 90 || z == 270)
        {
            rect.anchoredPosition -= new Vector2(offsetX, offsetY);
        }

        else if (z == 0 || z == 180)
        {
            rect.anchoredPosition += new Vector2(offsetX, offsetY);
        }
    }
    #endregion
}
