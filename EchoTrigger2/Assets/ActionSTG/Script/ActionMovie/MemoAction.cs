using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// メモを見終わったときのムービ設定
/// </summary>
public class MemoAction : MonoBehaviour
{
    [Header("Directorをアタッチ"),SerializeField]
    private PlayableDirector m_TimeLineDirector;

    [Header("ワープする位置"),SerializeField]
    private Transform m_WarpPoint;

    [Header("プレイヤー"), SerializeField]
    private GameObject m_PlayerObject;

    [Tooltip("MovieCameraアタッチ（ムービを流す方）"), SerializeField]
    private GameObject m_MovieCameraObject;

    [Header("Canvas全てアタッチ"), SerializeField]
    private GameObject m_CanvasObject;

    [Header("Memoのscriptをアタッチ"),SerializeField]
    private Memo m_MemoScript;

    //フラグ
    // エリア内にいるか
    private bool m_IsPlayerInArea = false;
    // メモを一度開いたか
    private bool m_IsMemoOpened = false;
    // ムービー再生済みか
    private bool m_HasPlayed = false;     

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //エリアに入ってないかまたはムービ済みじゃなければ処理しない
        if (!m_IsPlayerInArea || m_HasPlayed) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_IsMemoOpened = true;
            Debug.Log("MemoAction: Eキー検知 - メモを開いたと認識");
        }

        if (m_IsMemoOpened && Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("MemoAction: Tabキー検知 - ムービー開始します");
            m_HasPlayed = true;
            StartCoroutine(PlayMovieAndWarp());
        }
    }

    /// <summary>
    /// エリアに入ったらフラグオン
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPlayerInArea = true;
            m_IsMemoOpened = false; 
        }
    }

    /// <summary>
    /// エリアをでたらフラグオフ
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPlayerInArea = false;
            m_IsMemoOpened = false;
        }
    }

    /// <summary>
    /// ムービの処理、オブジェクトの表示、非表示の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayMovieAndWarp()
    {
        yield return null;

        // メモ消す
        var mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log("ムービー開始");

        //プレイヤーの見た目を消す（レンダラーを）
        Renderer[] playerRenderers = m_PlayerObject.GetComponentsInChildren<Renderer>();
        foreach (var r in playerRenderers)
        {
            r.enabled = false;
        }

        //UI（キャンバス）の見た目を消す
        Canvas canvasComp = null;
        if (m_CanvasObject != null)
        {
            canvasComp = m_CanvasObject.GetComponent<Canvas>();
            if (canvasComp != null)
            {
                canvasComp.enabled = false; 
            }
        }

        //ムービーカメラON
        if (m_MovieCameraObject != null)
        {
            m_MovieCameraObject.SetActive(true);
        }

        //Timeline再生
        if (m_TimeLineDirector != null)
        {
            bool isTimelineFinished = false;
            
            // タイムライン終了時のコールバックを設定
            m_TimeLineDirector.stopped += (director) => isTimelineFinished = true;
            
            m_TimeLineDirector.Play();
            
            // タイムラインが終了するまで待つ
            while (!isTimelineFinished)
            {
                yield return null;
            }
            
            m_TimeLineDirector.Stop();
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }

        // ムービーカメラOFF
        if (m_MovieCameraObject != null)
        {
            m_MovieCameraObject.SetActive(false);
        }

        // ワープ処理
        CharacterController cc = m_PlayerObject.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        m_PlayerObject.transform.position = m_WarpPoint.position;
        m_PlayerObject.transform.rotation = m_WarpPoint.rotation;

        if (cc != null) cc.enabled = true;

        // プレイヤーの見た目を元に戻す
        foreach (var r in playerRenderers)
        {
            r.enabled = true;
        }

        //UI（キャンバス）の見た目を元に戻す
        if (canvasComp != null)
        {
            canvasComp.enabled = true; 
        }

        // Memoスクリプトを無効化
        if (m_MemoScript != null)
        {
            m_MemoScript.enabled = false;
        }

        Debug.Log("ワープ完了・カメラ復帰");
    }
}