using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// GameOverが行った時の処理
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("Parametaをアタッチ"),SerializeField]
    private Parameta m_Parameta;

    [Header("GameOverの背景"), SerializeField]
    private Image m_GameOverBackGround;

    [Header("GameOverのUI"), SerializeField]
    private Image m_GameOverUI;

    [Header("コンテニュー"), SerializeField]
    private Image m_ContinyeUI;

    [Header("ギブアップ"), SerializeField]
    private Image m_GiveUpUI;

    [Header("カーソル")]
    public Image m_CursorUI;

    [Header("AudioSouseをアタッチ（自動）"), SerializeField]
    private AudioSource m_AudioSource;

    [Header("GameOverSelectionをアタッチ（自動）"),SerializeField]
    private GameOverSelection m_GameOverSelection;

    [Header("一度だけの実行フラグ"),SerializeField]
    private bool m_IsGameOverProcessed=false;

    [Header("選択フラグ"),SerializeField]
    private bool m_IsSelection=false;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //コンポーネントを取得
        if (m_AudioSource==null)
        {
            m_AudioSource = GetComponent<AudioSource>();
        }
        if (m_GameOverSelection == null)
        {
            m_GameOverSelection = GetComponent<GameOverSelection>();
        }

        //最初は曲を停止
        m_AudioSource.Stop();

        //非表示
        m_GameOverBackGround.gameObject.SetActive(false);
        m_GameOverUI.gameObject.SetActive(false);
        m_GiveUpUI.gameObject.SetActive(false);
        m_ContinyeUI.gameObject.SetActive(false);
        m_CursorUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //プレイヤーが死んだらかつ一度だけ処理すること
        if (m_Parameta.m_IsDie&&!m_IsGameOverProcessed)
        {
            m_IsGameOverProcessed = true;
            GameOverUI();
        }

        //選択フラグがたったら
        if (m_IsSelection)
        {
            //カーソル選択処理に移行
            m_GameOverSelection.CursorSelection();
        }
    }

    /// <summary>
    /// GameOverのUIの処理
    /// </summary>
    void GameOverUI()
    {
        Debug.Log("GameOverのUIを表示");

        //BGMを流す
        m_AudioSource.loop = true;
        m_AudioSource.Play();

        //表示
        m_GameOverBackGround.gameObject.SetActive(true);
        m_GameOverUI.gameObject.SetActive(true);

        //UICorutineへ
        StartCoroutine(UICorutine());
    }

    /// <summary>
    /// 三秒後にUIを表示
    /// </summary>
    /// <returns>三秒後にUIを返す</returns>
    private IEnumerator UICorutine()
    {
        //三秒後
        yield return new WaitForSeconds(3f);

        //表示
        m_GiveUpUI.gameObject.SetActive(true);
        m_ContinyeUI.gameObject.SetActive(true);
        m_CursorUI.gameObject.SetActive(true);

        //選択可能
        m_IsSelection = true;
    }
}
