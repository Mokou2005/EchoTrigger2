using System.Collections;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [Header("テキスト"), SerializeField]
    private TextMeshProUGUI m_TextComponent;

    [Header("画像を表示する場所"),SerializeField]
     private Image m_DisplayImage;

    [Header("切り替える絵"), SerializeField]
    private Sprite[] m_SpriteImage;

    [Header("一文字ごとの速さ")]
    private float m_TypeSpeed = 0.05f; 

    [Header("フェードの速さ（秒）"), SerializeField]
    private float m_FadeDuration = 0.3f;

    [Header("テキスト表示中のSE"), SerializeField]
    private AudioSource m_AudioSource;

    [Header("タイプ音"), SerializeField]
    private AudioClip m_TypeSE;

    [Header("テスト用（実行時に表示したい文章）")]
    [TextArea(3, 10)]
    public string m_storyText;

    ///何番目のTextか
    [SerializeField] private int m_IndexText = 0;

    /// フェード中かどうか
    private bool m_IsFading = false;

    /// 最初の表示かどうか
    private bool m_IsFirstDisplay = true;

    /// テキスト用のコルーチン
    private Coroutine m_TextCoroutine = null;

    /// ストーリーが完了したかどうか
    private bool m_IsStoryComplete = false;

    [Header("遷移先のシーン名"), SerializeField]
    private string m_NextSceneName = "Lab";

    private void Start()
    {
        // ゲーム開始時に実行
        if (!string.IsNullOrEmpty(m_storyText))
        {
            TextChange();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !m_IsFading)
        {
            // ストーリー完了後はシーン遷移
            if (m_IsStoryComplete)
            {
                LoadNextScene();
            }
            else
            {
                TextChange();
            }
        }
    }

    /// <summary>
    /// テキストと画像を変える処理
    /// </summary>
    void TextChange()
    {
        switch (m_IndexText)
        {
            case 0:
                //画像替え
                StartCoroutine(ChangeSpriteWithFade(m_SpriteImage[0]));
                m_storyText = "時は20XX年――。";
                StartTypewriter(m_storyText);
                break;
            case 1:
                m_storyText = "街は今日もまた、変わらぬ活気に満ち溢れていた。\r\n誰もがこの国を愛し、平穏な日々を謳歌している。";
                StartTypewriter(m_storyText);
                break;
            case 2:
                m_storyText = "そこは、世界中の誰からも愛される、美しく賑やかな国だった。";
                StartTypewriter(m_storyText);
                break;
            case 3:
                m_storyText = "しかし、光ある場所には必ず影が落ちる。";
                StartTypewriter(m_storyText);
                break;
            case 4:
                m_storyText = "平和な日々の裏側で、静かに動き出した者たちがいた。";
                StartTypewriter(m_storyText);
                break;
            case 5:
                m_storyText = "この美しき国を、根底から滅ぼそうと企む「影」が……。";
                StartTypewriter(m_storyText);
                break;
            case 6:
                //画像替え
                StartCoroutine(ChangeSpriteWithFade(m_SpriteImage[1]));
                m_storyText = "空を切り裂くような轟音と共に現れたのは、無数のヘリコプター。";
                StartTypewriter(m_storyText);
                break;
            case 7:
                m_storyText = "彼らが空から撒き散らしたのは、救助物資などではない。";
                StartTypewriter(m_storyText);
                break;
            case 8:
                m_storyText = "それは、この街を死に至らしめる「謎のウィルス」だった。";
                StartTypewriter(m_storyText);
                break;
            case 9:
                m_storyText = "そのウィルスは、人々の体から「力」を奪い去った。";
                StartTypewriter(m_storyText);
                break;
            case 10:
                //画像替え
                StartCoroutine(ChangeSpriteWithFade(m_SpriteImage[2]));
                m_storyText = "昨日まで笑い合っていた人々が、痩せ細り、次々と路地に崩れ落ちていく。\r\n住人を失った街は、急速にその色を失った。";
                StartTypewriter(m_storyText);
                break;
            case 11:
                m_storyText = "ガラスは割れ、建物は朽ち、かつての美しい国は見る影もなく崩壊していった。";
                StartTypewriter(m_storyText);
                break;
            case 12:
                //画像替え
                StartCoroutine(ChangeSpriteWithFade(m_SpriteImage[3]));
                m_storyText = "だが、このまま滅びを待つつもりはない。";
                StartTypewriter(m_storyText);
                break;
            case 13:
                m_storyText = "私は瓦礫の中で誓った。必ずこの世界を救ってみせると。";
                StartTypewriter(m_storyText);
                break;
            case 14:
                m_storyText = "全ての元凶を断つため、私は最後の希望を背負い、\r\n奴らの根城である「研究所」への潜入を決意した。";
                StartTypewriter(m_storyText);
                break;
            case 15:
                //画像替え
                StartCoroutine(ChangeSpriteWithFade(m_SpriteImage[4]));
                m_storyText = "私は止まらない、みんなを救うため、\r\n国をまた賑やかな世界に戻すため。";
                StartTypewriter(m_storyText);
                break;
            case 16:
                m_storyText = "さぁいこう、ワクチンを取りに行くのだ、";
                StartTypewriter(m_storyText);
                // ストーリー完了フラグを立てる
                m_IsStoryComplete = true;
                break;
        }

    }

    /// <summary>
    /// スプライトをフェードアウト→変更→フェードインで切り替える
    /// </summary>
    /// <param name="newSprite">新しいスプライト</param>
    /// <returns></returns>
    private IEnumerator ChangeSpriteWithFade(Sprite newSprite)
    {
        m_IsFading = true;

        // 現在の色を取得
        Color originalColor = m_DisplayImage.color;

        // 最初の表示の場合はフェードアウトをスキップ
        if (!m_IsFirstDisplay)
        {
            // フェードアウト（透明にする）
            float elapsed = 0f;
            while (elapsed < m_FadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / m_FadeDuration);
                m_DisplayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            m_DisplayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
        else
        {
            // 最初は透明から始める
            m_DisplayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            m_IsFirstDisplay = false;
        }

        // スプライトを変更
        m_DisplayImage.sprite = newSprite;

        // フェードイン（元の色に戻す）
        float elapsedIn = 0f;
        while (elapsedIn < m_FadeDuration)
        {
            elapsedIn += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedIn / m_FadeDuration);
            m_DisplayImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        m_DisplayImage.color = originalColor;

        m_IsFading = false;
    }

    /// <summary>
    /// 言葉を更新する時用の関数
    /// </summary>
    /// <param name="text"></param>
    public void StartTypewriter(string text)
    {
        // テキストコルーチンのみを停止（フェードは止めない）
        if (m_TextCoroutine != null)
        {
            StopCoroutine(m_TextCoroutine);
            // SEも停止
            StopTypeSE();
        }
        m_TextCoroutine = StartCoroutine(PlayText(text));
    }

    /// <summary>
    /// 文字をひとつずつ更新（言葉の処理）
    /// </summary>
    /// <param name="targetText">自分で設定した言葉</param>
    /// <returns></returns>
    private IEnumerator PlayText(string targetText)
    {
        m_IndexText++;
        //自分が書いた言葉を代入
        m_TextComponent.text = targetText;
        //文字をまずリセット
        m_TextComponent.maxVisibleCharacters = 0;

        // SE再生開始
        PlayTypeSE();

        // 1文字ずつ表示文字数を増やしていく
        for (int i = 0; i <= targetText.Length; i++)
        {
            m_TextComponent.maxVisibleCharacters = i;
            //文字スピードで更新
            yield return new WaitForSeconds(m_TypeSpeed);
        }

        // テキスト表示完了時にSE停止
        StopTypeSE();
    }

    /// <summary>
    /// タイプSEを再生する
    /// </summary>
    private void PlayTypeSE()
    {
        if (m_AudioSource != null && m_TypeSE != null)
        {
            m_AudioSource.clip = m_TypeSE;
            m_AudioSource.loop = true;
            m_AudioSource.Play();
        }
    }

    /// <summary>
    /// タイプSEを停止する
    /// </summary>
    private void StopTypeSE()
    {
        if (m_AudioSource != null && m_AudioSource.isPlaying)
        {
            m_AudioSource.Stop();
        }
    }

    /// <summary>
    /// 次のシーンに遷移する
    /// </summary>
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(m_NextSceneName))
        {
            SceneManager.LoadScene(m_NextSceneName);
        }
    }
}