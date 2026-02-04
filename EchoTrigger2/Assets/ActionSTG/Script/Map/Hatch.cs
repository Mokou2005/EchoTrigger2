using UnityEngine;
/// <summary>
/// ハッチの処理
/// </summary>
public class Hatch : MonoBehaviour
{
    [Header("テレポートする場所"), SerializeField]
    private Transform m_TPPosition;

    [Header("Playerをアタッチ"), SerializeField]
    private GameObject m_PlayerTP;

    [Header("エリアに入ったかのフラグ"), SerializeField]
    private bool m_InAria = false;

    [Header("ゴールのオブジェクト"), SerializeField]
    private GameObject m_GoalObject;

    [Header("Loadingのscriptをアタッチ"),SerializeField]
    private Loading m_Loading;

    [Header("Key表示の画像"),SerializeField]
    private GameObject m_KeyImage;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        m_KeyImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// エリアに入ったらテレポートの処理へ
    /// </summary>
    /// <param name="other">プレイヤー</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //エリアを検知
            m_InAria = true;
            //Key表示
            if (m_KeyImage != null)
            {
                m_KeyImage.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //エリアから抜けた
            m_InAria = false;
            //Key非表示
            if (m_KeyImage != null)
            {
                m_KeyImage.SetActive(false);
            }
        }
    }

    /// <summary>
    ///更新
    /// </summary>
    private void Update()
    {
        //エリアに入っていてEキーを押したらTP
        if (Input.GetKeyDown(KeyCode.E) &&　m_InAria)
        {
            //ローディングする
            m_Loading.gameObject.SetActive(true);
            m_PlayerTP.transform.position = m_TPPosition.position;
            m_GoalObject.SetActive(true);
        }
    }
}
