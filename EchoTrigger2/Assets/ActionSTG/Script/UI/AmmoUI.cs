using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 残り弾数のUI処理
/// </summary>
public class AmmoUI : MonoBehaviour
{
    [Header("今銃に入ってる弾の数"), SerializeField]
    private Text m_GunAmmoText;

    [Header("プレイヤーが今持ってるストックの数"), SerializeField]
    private Text m_GunAmmoStockText;

    [Header("PlayerAttackへ参照"),SerializeField]
    private PlayerAttack m_PlayerAttack;

    // 元の色を保存する変数
    private Color m_OriginalColor;

    /// <summary>
    /// 最初は白色のTextに保存
    /// </summary>
    void Start()
    {
        if (m_GunAmmoText != null)
        {
            m_OriginalColor = m_GunAmmoText.color;
        }
    }

    /// <summary>
    /// 残りの弾を更新
    /// </summary>
    void Update()
    {
        if (m_PlayerAttack == null) return;

        // 現在の弾数（マガジン）を表示
        if (m_GunAmmoText != null)
        {
            m_GunAmmoText.text = m_PlayerAttack.m_BulletCount.ToString();

            // 弾数が3以下の場合は赤くする
            if (m_PlayerAttack.m_BulletCount <= 3)
            {
                m_GunAmmoText.color = Color.red;
            }
            else
            {
                m_GunAmmoText.color = m_OriginalColor;
            }
        }

        // 残り弾数（ストック）を表示
        if (m_GunAmmoStockText != null)
        {
            m_GunAmmoStockText.text = m_PlayerAttack.m_BulletIndex.ToString();

            //ストックが10以下の場合は赤くする
            if (m_PlayerAttack.m_BulletIndex<=10)
            {
                m_GunAmmoStockText.color=Color.red;
            }
            else
            {
                m_GunAmmoStockText.color = m_OriginalColor;
            }
        }
    }
}
