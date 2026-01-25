using UnityEngine;
/// <summary>
/// アイテムを知らせるための通知オンエフェクト
/// </summary>
public class ItemEffect : MonoBehaviour
{
    [Header("effect本体"), SerializeField]
    private ParticleSystem m_ItemEffect;

    [Header("アイテムを知らせるSE"), SerializeField]
    private AudioSource m_ItemEffectSE;

    [Header("SEを使用するか"), SerializeField]
    private bool m_UseSE = true;

    [Header("SEとエフェクトが消える距離"), SerializeField]
    private float m_EffectDistance = 1f;

    // プレイヤーのTransformを保持
    private Transform m_PlayerTransform;
    
    // トリガー内にいるかどうか
    private bool m_IsInTrigger = false;

    // エフェクト再生中かどうか
    private bool m_IsEffectPlaying = false;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        // エフェクトは最初は出さない
        m_ItemEffect.Stop();
        
        // SEをループ設定にする
        if (m_UseSE && m_ItemEffectSE != null)
        {
            m_ItemEffectSE.loop = true;
        }
    }

    /// <summary>
    /// プレイヤーがこのオブジェクトのコライダーに触れたら呼ばれる
    /// </summary>
    /// <param name="other">プレイヤー</param>
    private void OnTriggerEnter(Collider other)
    {
        // ぶつかったのが "Player" タグを持つオブジェクトなら
        if (other.CompareTag("Player"))
        {
            // プレイヤーのTransformを保持
            m_PlayerTransform = other.transform;
            m_IsInTrigger = true;
            
            // エフェクトを再生
            m_ItemEffect.Play();
            m_IsEffectPlaying = true;

            // SE再生（ループ）
            if (m_UseSE && m_ItemEffectSE != null)
            {
                m_ItemEffectSE.Play();
            }
        }
    }

    /// <summary>
    /// プレイヤーがこのオブジェクトのコライダーから出たら呼ばれる
    /// </summary>
    /// <param name="other">プレイヤー</param>
    private void OnTriggerExit(Collider other)
    {
        // ぶつかったのが "Player" タグを持つオブジェクトなら
        if (other.CompareTag("Player"))
        {
            // エフェクトを停止
            m_ItemEffect.Stop();
            m_IsEffectPlaying = false;

            // SE停止
            if (m_UseSE && m_ItemEffectSE != null)
            {
                m_ItemEffectSE.Stop();
            }
            
            m_IsInTrigger = false;
            m_PlayerTransform = null;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // トリガー内にいなければ何もしない
        if (!m_IsInTrigger || m_PlayerTransform == null) return;
        
        // プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, m_PlayerTransform.position);
        
        // 距離がm_EffectDistance以下ならエフェクトとSEを停止
        if (distance <= m_EffectDistance)
        {
            if (m_IsEffectPlaying)
            {
                m_ItemEffect.Stop();
                if (m_UseSE && m_ItemEffectSE != null)
                {
                    m_ItemEffectSE.Stop();
                }
                m_IsEffectPlaying = false;
            }
        }
        // 距離がm_EffectDistanceより大きければエフェクトとSEを再生
        else
        {
            if (!m_IsEffectPlaying)
            {
                m_ItemEffect.Play();
                if (m_UseSE && m_ItemEffectSE != null)
                {
                    m_ItemEffectSE.Play();
                }
                m_IsEffectPlaying = true;
            }
        }
    }
}
