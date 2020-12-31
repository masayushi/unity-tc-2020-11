using UnityEngine;

public class Tetris : MonoBehaviour
{
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

    /// <summary>
    /// 紀錄目前射線長度
    /// </summary>
    private float cube;
    private float cubeDown;

    /// <summary>
    /// 繪製圖示(感應邊界的輔助線)
    /// </summary>
    private void OnDrawGizmos()
    {
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
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Vector3.right * cubeRotate90r);
            Gizmos.DrawRay(transform.position, -Vector3.right * cubeRotate90l);
        }
    }
    private void Start()
    {
        // 儲存遊戲開始的角度
        cube = cube0;

        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        CheckWall();
    }

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
}
