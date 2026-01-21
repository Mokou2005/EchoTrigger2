using System;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// タイトルのメインメニューの選択
/// </summary>
public class TitleSelection : MonoBehaviour
{
    [Header("タイトルのメニュー一覧"), SerializeField]
    private Text[] m_MenuText;

    [Header("カーソル"), SerializeField]
    private Image m_TitleCursor;

    [Header("カーソルのスピード"), SerializeField]
    private float m_TitleCursorSpeed = 5;

    [Header("現在の選択番号"), SerializeReference]
    private int m_TitleIndex = 0;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //最初の所にカーソルを合わせる
        if (m_MenuText.Length > 0)
        {
            m_TitleCursor.transform.position = m_MenuText[m_TitleIndex].transform.position;
        }
    }

    private void Update()
    {
        TitleCursorSelection();
    }

    /// <summary>
    /// カーソルの処理選択
    /// </summary>
    private void TitleCursorSelection()
    {
        //上を押すとカーソルが上に
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            m_TitleIndex++;

            //m_TitleIndexがm_MenuText.Lengthより同じか大きかった戻す
            if (m_TitleIndex >= m_MenuText.Length)
            {
                m_TitleIndex = 0;
            }
        }

        //下を押すとカーソルが下に行き一番下に達したら上に戻る
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            m_TitleIndex--;

            if (m_TitleIndex < 0)
            {
                m_TitleIndex = m_MenuText.Length - 1;
            }
        }

        //カーソルの移動処理
        if (m_MenuText.Length > 0 && m_TitleCursor != null)
        {
            //今選択している位置を取得
            Vector3 targetPos = m_MenuText[m_TitleIndex].transform.position;

            //カーソルを滑らかに移動する
            m_TitleCursor.transform.position = Vector3.Lerp(
                m_TitleCursor.transform.position,
                targetPos,
                Time.deltaTime * m_TitleCursorSpeed
            );

        }

        //エンターで選択
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (m_TitleIndex)
            {
                case 0:
                    Debug.Log("ゲーム開始へ選択");
                    SceneManager.LoadScene("Opening");
                    break;
                case 1:
                    Debug.Log("ゲームを終了を選択");
                    Application.Quit();
                    break;
            }
        }
    }
}
