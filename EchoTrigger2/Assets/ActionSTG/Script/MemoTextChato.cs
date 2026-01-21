using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro; // TextMeshProを使用

/// <summary>
/// ムービを流しながらトーク機能（タイプライター演出付き）
/// 複数セリフ対応：「|」で区切ると連続して表示、すべて終わったらタイムライン再開
/// </summary>
public class MemoTextChato : MonoBehaviour
{
    [Header("Timeline本体"), SerializeField]
    private PlayableDirector m_Director;

    [Header("--- 会話UI設定 ---")]
    [SerializeField] private GameObject chatCanvas;        // 会話UIの親（表示/非表示用）
    [SerializeField] private TextMeshProUGUI m_TextComponent; // 文字を出すTMP

    [Header("--- 演出設定 ---")]
    [SerializeField] private float m_TypeSpeed = 0.05f;    // 1文字の表示速度

    [Header("--- 音響設定 ---")]
    [SerializeField] private AudioSource m_AudioSource;    // SEを鳴らすスピーカー
    [SerializeField] private AudioClip m_TypeSE;           // カタカタ音

    // 内部変数
    private bool m_IsPaused = false;      // 停止中フラグ
    private Coroutine m_TextCoroutine = null; // 文字送りの処理保存用
    private Queue<string> m_TextQueue = new Queue<string>(); // セリフのキュー

    private void Start()
    {
        // 最初は会話ウィンドウを隠しておく
        if (chatCanvas != null) chatCanvas.SetActive(false);
    }

    void Update()
    {
        // ムービが止まっていて、エンターキー（またはクリック）を押したら
        if (m_IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                // まだ文字が流れている途中なら、スキップして全文表示
                if (m_TextComponent.maxVisibleCharacters < m_TextComponent.text.Length)
                {
                    ShowFullText();
                }
                else
                {
                    // 文字が表示し終わっていたら、次のセリフへ
                    ShowNextText();
                }
            }
        }
    }

    /// <summary>
    /// シグナルから呼ばれる：ムービーを止めてテキストを表示する
    /// ※複数セリフは「|」で区切る（例：「こんにちは|元気？|さようなら」）
    /// </summary>
    public void PauseMovie(string text)
    {
        // 「|」で区切って複数セリフに分割
        string[] texts = text.Split('|');
        
        // キューをクリアして新しいセリフを追加
        m_TextQueue.Clear();
        foreach (string t in texts)
        {
            string trimmed = t.Trim(); // 前後の空白を除去
            if (!string.IsNullOrEmpty(trimmed))
            {
                m_TextQueue.Enqueue(trimmed);
            }
        }

        // UIを表示
        if (chatCanvas != null) chatCanvas.SetActive(true);

        // Timelineを止める
        if (m_Director != null)
        {
            m_Director.Pause();
            m_IsPaused = true;
        }

        // 最初のセリフを表示
        ShowNextText();
    }

    /// <summary>
    /// 次のセリフを表示する。キューが空ならタイムライン再開
    /// </summary>
    private void ShowNextText()
    {
        if (m_TextQueue.Count > 0)
        {
            // まだセリフがある → 次のセリフを表示
            string nextText = m_TextQueue.Dequeue();
            StartTypewriter(nextText);
        }
        else
        {
            // セリフがすべて終わった → タイムライン再開
            ResumeMovie();
        }
    }

    /// <summary>
    /// 再開する処理
    /// </summary>
    private void ResumeMovie()
    {
        // UIを隠す
        if (chatCanvas != null) chatCanvas.SetActive(false);

        // Timelineを再開する
        if (m_Director != null)
        {
            m_Director.Play();
            m_IsPaused = false;
        }
    }

    // --- 以下、TypewriterEffectから移植した演出用コード ---

    /// <summary>
    /// タイプライター演出の開始
    /// </summary>
    private void StartTypewriter(string text)
    {
        // 実行中のコルーチンがあれば止める
        if (m_TextCoroutine != null)
        {
            StopCoroutine(m_TextCoroutine);
            StopTypeSE();
        }
        m_TextCoroutine = StartCoroutine(PlayText(text));
    }

    /// <summary>
    /// 1文字ずつ表示するコルーチン
    /// </summary>
    private IEnumerator PlayText(string targetText)
    {
        m_TextComponent.text = targetText;
        m_TextComponent.maxVisibleCharacters = 0; // まず0文字表示

        PlayTypeSE(); // 音再生

        // 1文字ずつ増やしていく
        for (int i = 0; i <= targetText.Length; i++)
        {
            m_TextComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(m_TypeSpeed);
        }

        StopTypeSE(); // 音停止
    }

    /// <summary>
    /// 文字送りをスキップして一気に表示する処理
    /// </summary>
    private void ShowFullText()
    {
        if (m_TextCoroutine != null) StopCoroutine(m_TextCoroutine);
        m_TextComponent.maxVisibleCharacters = m_TextComponent.text.Length;
        StopTypeSE();
    }

    private void PlayTypeSE()
    {
        if (m_AudioSource != null && m_TypeSE != null)
        {
            m_AudioSource.clip = m_TypeSE;
            m_AudioSource.loop = true; // ループ再生にする
            m_AudioSource.Play();
        }
    }

    private void StopTypeSE()
    {
        if (m_AudioSource != null && m_AudioSource.isPlaying)
        {
            m_AudioSource.Stop();
        }
    }
}