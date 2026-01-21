using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
/// <summary>
/// エンドロールの処理
/// </summary>
public class EndRoll : MonoBehaviour
{
    [Header("テキストのスクロールのスピード"), SerializeField]
    private float m_TextScrollSpeed = 30f;

    [Header("テキストの制限位置"), SerializeField]
    private float m_TextLimitPosition = 730f;

    [Header("エンドロールが終了したかどうか"), SerializeField]
    private bool m_IsStopEndRoll;

    [Header("シーンに移動用のコルーチン"), SerializeField]
    private Coroutine m_EndRollCoroutine;

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //　エンドロールが終了したら
        if (m_IsStopEndRoll)
        {
            m_EndRollCoroutine = StartCoroutine(GoToNextScene());
        }
        else
        {
            //エンドロールのテキストがリミットを超えるまで動かす
            if (transform.position.y <= m_TextLimitPosition)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + m_TextScrollSpeed * Time.deltaTime);
            }
            else
            {
                //リミットに達したらエンドロールのフラグをオンに
                m_IsStopEndRoll = true;
            }
        }
    }

    /// <summary>
    /// シーンに移動する処理
    /// </summary>
    /// <returns></returns>
    IEnumerator GoToNextScene()
    {

        if (Input.GetKeyDown("space"))
        {
            //このコルーチンをストップする
            StopCoroutine(m_EndRollCoroutine);
            //別のシーンに移動
            SceneManager.LoadScene("Title");
        }

        yield return null;
    }
}
