using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// オプション処理
/// </summary>
public class Options : MonoBehaviour
{
    [Header("オプションのオブジェクト"), SerializeField]
    private GameObject m_OptionsObj;

    [Header("Soundをアタッチ"), SerializeField]
    private AudioSource m_TVSE;

    /// <summary>
    /// オプションが開いているかどうか（他のスクリプトから参照用）
    /// </summary>
    public static bool m_IsOptionsOpen { get; private set; } = false;

    /// <summary>
    /// 開始処理
    /// </summary>
    private void Start()
    {
        //非表示
        m_OptionsObj.SetActive(false);
        m_IsOptionsOpen = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //特定のKeyを押したらキャンバス表示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_OptionsObj.activeSelf)
            {
                //表示
                m_OptionsObj.SetActive(true);
                //音を再生
                m_TVSE.Play();
                //オプションが開いている状態
                m_IsOptionsOpen = true;
                //カーソルを表示
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                //非表示
                m_OptionsObj.SetActive(false);
                //オプションが閉じている状態
                m_IsOptionsOpen = false;
                //カーソルを非表示に戻す
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
