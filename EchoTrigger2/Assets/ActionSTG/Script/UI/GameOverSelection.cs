using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// GameOverになった際のKey選択処理
/// </summary>
public class GameOverSelection : MonoBehaviour
{
    [Header("コンテニューとギブアップ"), SerializeField]
    private Image[] m_Selections;

    [Header("カーソルスピード"), SerializeField]
    private float m_CursorSpeed = 5f;

    [Header("GameOverのscriptをアタッチ（自動）"), SerializeField]
    private GameOver m_GameOver;

    [Header("現在の選択番号"), SerializeField]
    private int m_Index = 0;

    [Header("アニメーター"), SerializeField]
    private Animator[] m_Animator;
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //コンポーネントを取得
        if (m_GameOver == null)
        {
            m_GameOver = GetComponent<GameOver>();
        }

        //開始時に最初のところにカーソルを合わせる
        if (m_Selections.Length > 0)
        {
            m_GameOver.m_CursorUI.transform.position = m_Selections[m_Index].transform.position;
        }

    }

    /// <summary>
    /// カーソルの選択処理
    /// </summary>
    public void CursorSelection()
    {
        //上を押すとカーソルが上に
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_Index++;
            //Indexがm_Selections.Lengthより同じか大きかった戻す
            if (m_Index >= m_Selections.Length)
            {
                m_Index = 0;
            }
        }

        //カーソルを下に押すと下に行き一番下に達したら上に戻る
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_Index--;
            if (m_Index < 0)
            {
                m_Index = m_Selections.Length - 1;
            }
        }

        //カーソルの移動処理
        if (m_Selections.Length > 0 && m_GameOver.m_CursorUI != null)
        {
            //今選択している位置を取得
            Vector3 targetPos = m_Selections[m_Index].transform.position;

            //カーソルを滑らかに移動する
            m_GameOver.m_CursorUI.transform.position = Vector3.Lerp(
                m_GameOver.m_CursorUI.transform.position,
                targetPos,
                Time.deltaTime * m_CursorSpeed
            );

        }

        //カーソルがコンテニューかギブアップに重なるたびアニメーション
        switch (m_Index)
        {
            case 0:
                m_Animator[1].SetBool("GiveUpPlay", false);
                m_Animator[0].SetBool("ContinuePlay", true);
                break;
            case 1:
                m_Animator[0].SetBool("ContinuePlay", false);
                m_Animator[1].SetBool("GiveUpPlay", true);
                break;

        }

        //エンターで選択
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (m_Index)
            {
                case 0:
                    Debug.Log("コンテニューを選択");
                    // 現在のシーンを再読み込み
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case 1:
                    Debug.Log("タイトルに戻る");
                    SceneManager.LoadScene("Title");
                    break;

            }

        }
    }
}
