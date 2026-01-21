using UnityEngine;
/// <summary>
/// コンテニューの処理
/// </summary>
public class Continue : MonoBehaviour
{
    [Header("復活するPlayer"), SerializeField]
    private GameObject m_ResurrectionPlayer;

    [Header("Playerが復活する場所"),SerializeField]
    private Transform m_ResurrectionPosition;

    /// <summary>
    /// プレイヤーが復活する処理
    /// </summary>
    public void ContinuePlayer()
    {
        //プレイヤーをスポーン
        Instantiate(m_ResurrectionPlayer, m_ResurrectionPosition.position, Quaternion.identity);
    }
}
