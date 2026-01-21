using UnityEngine;

public class PlayerDetectionState : MonoBehaviour
{
    //プレイヤーが見えなくなっているかどうか
    private bool isInvisible = false;

    //外部から今見えなくなっている状態か知らせる
    public bool IsInvisible()
    {
        return isInvisible;
    }

    public void SetInvisible(bool value)
    {
        isInvisible = value;
        Debug.Log("検知不可状態: " + isInvisible);
    }
}
