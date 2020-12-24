using UnityEngine;

public class Tetris : MonoBehaviour
{
    [Header("角度為0時，輔助線的長度")]
    public float cube0;

    [Header("角度為90時，輔助線的長度")]
    public float cube90;

    /// <summary>
    /// 紀錄目前射線長度
    /// </summary>
    public float cube;

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
            Gizmos.DrawRay(transform.position, Vector3.right * cube0);
        }
        else if (z == 90 || z == 270)
        {
            // 儲存目前長度
            cube = cube90;

            Gizmos.DrawRay(transform.position, Vector3.right * cube90);
        }
    }
    private void Start()
    {
        // 儲存遊戲開始的角度
        cube = cube0;
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
    }
}
