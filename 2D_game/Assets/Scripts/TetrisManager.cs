using UnityEngine;

public class TetrisManager : MonoBehaviour
{
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


    

    private void BLOCK()
    {
        print("生成俄羅斯方塊");
    }



    public int Pluscore(int score)
    {
        score += 10;
        return 50;
        
    }



}
