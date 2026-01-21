using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// ローディングの処理
/// </summary>
public class Loading : MonoBehaviour
{
    [Header("Sponのオーナーをアタッチ"), SerializeField]
    private MasterEnemySystem m_LoadingMasterEnemySystem;

    [Header("バーが伸びる方のImage"), SerializeField]
    private Image m_LoadingBar;

    [Header("消したいオブジェクト"), SerializeField]
    private List<GameObject> m_DestroyObjects = new List<GameObject>();

    [Header("1フレームに消す数（多いほど速くなる）")]
    [SerializeField] private int m_DestroyPerFrame = 1;

    [Header("進歩をまとめて管理する"), SerializeField]
    private float m_Progress = 0f;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        StartCoroutine(LoadingProcess());
    }

    /// <summary>
    /// オブジェクトを少しずつ消しながらローディングバーを更新する処理
    /// </summary>
    /// <returns>コルーチン</returns>
    private IEnumerator LoadingProcess()
    {
        //2段階のロード※ロードする数によって数を帰る
        float stepCount = 2f;

        //建物を消す
        yield return StartCoroutine(DestroyObjectsStep(0f, 1f / stepCount));

        //敵の出現
        yield return StartCoroutine(EnemyRespon(1f / stepCount, 1f));

        // 完了
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 建物を消す処理
    /// </summary>
    /// <param name="start">ローディングバーの開始値</param>
    /// <param name="end">ローディングバーの終了値</param>
    /// <returns></returns>
    private IEnumerator DestroyObjectsStep(float start, float end)
    {
        //消す対象となるオブジェクトの数
        int total = m_DestroyObjects.Count;
        //これまで削除した数
        int destroyed = 0;

        //すべてのオブジェクトを消すまで繰り返す
        while (destroyed < total)
        {
            for (int i = 0; i < m_DestroyPerFrame; i++)
            {
                //全て消したらループを抜ける
                if (destroyed >= total) break;
                //一つずつ消す
                Destroy(m_DestroyObjects[destroyed]);
                //消したものを加算
                destroyed++;
            }
            //このステップ内の進行度を計算
            float localProgress = (float)destroyed / total;

            // 全体ローディングの指定範囲（start〜end）に合わせて
            // ローディングバーの表示量を更新
            m_LoadingBar.fillAmount =
                Mathf.Lerp(start, end, localProgress);

            // 次のフレームまで待機し、処理を分割する
            yield return null;
        }
    }

    /// <summary>
    /// 敵をスポーンする処理
    /// </summary>
    /// <param name="start">ローディングバーの開始値</param>
    /// <param name="end">ローディングバーの終了値</param>
    /// <returns></returns>
    private IEnumerator EnemyRespon(float start, float end)
    {
        float timer = 0f;
        float duration = 1.5f;

        //敵を初期化して消す
        m_LoadingMasterEnemySystem.EnemyAllDestroy();
        //敵をスポーン
        m_LoadingMasterEnemySystem.EnemySpawn();
        while (timer < duration)
        {
            //バーを滑らかに動かす処理
            timer += Time.deltaTime;
            float t = timer / duration;

            //ローディングバーの更新
            m_LoadingBar.fillAmount =
                Mathf.Lerp(start, end, t);

            yield return null;
        }
        
    }
}
