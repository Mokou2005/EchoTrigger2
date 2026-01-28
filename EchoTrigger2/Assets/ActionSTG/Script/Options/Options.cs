using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// オプション処理
/// </summary>
public class Options : MonoBehaviour
{
    [Header("オプションのオブジェクト"), SerializeField]
    private GameObject m_OptionsObj;

    [Header("選択オブジェクト"), SerializeField]
    private GameObject m_SelectionObj;

    [Header("操作説明のオブジェクト"),SerializeField]
    private GameObject m_OperationObject;

    [Header("操作説明の矢印のimage"), SerializeField]
    private Image[] m_OperationArrowImage;

    [Header("矢印を選択されてない時のスプライト"), SerializeField]
    private Sprite[] m_OperationArrowOffSprite;

    [Header("矢印を選択された時のスプライト"), SerializeField]
    private Sprite[] m_OperationArrowOnSprite;

    [Header("操作説明の画像"), SerializeField]
    private Image m_OperationImage;

    [Header("操作説明のスプライト"),SerializeField]
    private Sprite[] m_OperationImageSprite;

    [Header("選択オブジェクトのimage"), SerializeField]
    private Image[] m_SelectionImage;

    [Header("選択されてない時のスプライト"), SerializeField]
    private Sprite[] m_SelectionOffSprite;

    [Header("選択された時のスプライト"), SerializeField]
    private Sprite[] m_SelectionOnSprite;

    [Header("Soundをアタッチ")]
    [SerializeField] private AudioSource m_TVSE;
    [SerializeField] private AudioSource m_SelectionOpenSE;
    [SerializeField] private AudioSource m_ExitSE;
    [SerializeField] private AudioSource m_PushSelectionSE;
    [SerializeField] private AudioSource m_SelectionUpDownSE;

    [Header("選択が表示されたかフラグ"), SerializeField]
    private bool m_SelectionOnImage = false;

    [Header("現在の選択番号"), SerializeField]
    private int m_SelectionBox = 0;

    [Header("説明画面表示中フラグ"), SerializeField]
    private bool m_IsDescriptionOpen = false;

    [Header("操作説明の矢印選択番号"), SerializeField]
    private int m_OperationArrowIndex = 0;

    [Header("操作説明画像の現在のインデックス"), SerializeField]
    private int m_OperationImageIndex = 0;

    //スワイプアニメーション中かどうか
    private bool m_IsSwipeAnimating = false;

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
        m_SelectionObj.SetActive(false);
        m_OperationObject.SetActive(false);

        //最初はオプションは開いてない
        m_IsOptionsOpen = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //選択オブジェクトが表示されていたら
        if (m_SelectionObj.activeSelf)
        {
            //上キーが押されたら
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //選択番号を減らす（上に移動）
                m_SelectionBox--;

                //範囲外になったら最後の項目に戻す
                if (m_SelectionBox < 0)
                {
                    m_SelectionBox = m_SelectionImage.Length - 1;
                }

                //スプライトを更新
                UpdateSelectionSprites();

                //上下Sound再生
                m_SelectionUpDownSE.Play();
            }

            //下キーが押されたら
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //選択番号を増やす（下に移動）
                m_SelectionBox++;

                //範囲外になったら最初の項目に戻す
                if (m_SelectionBox >= m_SelectionImage.Length)
                {
                    m_SelectionBox = 0;
                }

                //スプライトを更新
                UpdateSelectionSprites();

                //上下Sound再生
                m_SelectionUpDownSE.Play();
            }
        }

        //説明画面が表示中にBackSpaceで戻る
        if (m_IsDescriptionOpen && Input.GetKeyDown(KeyCode.Backspace))
        {
            //説明画面フラグをOFF
            m_IsDescriptionOpen = false;

            //操作説明オブジェクトを非表示
            m_OperationObject.SetActive(false);

            //選択画像を再表示
            ShowSelectionImages();

            //Sound再生
            m_ExitSE.Play();
        }

        //操作説明画面が表示中の左右キー操作
        if (m_IsDescriptionOpen && !m_IsSwipeAnimating)
        {
            //左キーが押されたら（前の画像へ）
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //左矢印のインデックス（0番目と仮定）
                StartCoroutine(FlashArrowSprite(0));

                //前の画像へスワイプ（右方向にスワイプ）
                if (m_OperationImageIndex > 0)
                {
                    StartCoroutine(SwipeOperationImage(1)); //右方向にスワイプ
                    m_OperationImageIndex--;
                }

                //Sound再生
                m_SelectionUpDownSE.Play();
            }

            //右キーが押されたら（次の画像へ）
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //右矢印のインデックス（1番目と仮定）
                StartCoroutine(FlashArrowSprite(1));

                //次の画像へスワイプ（左方向にスワイプ）
                if (m_OperationImageIndex < m_OperationImageSprite.Length - 1)
                {
                    StartCoroutine(SwipeOperationImage(-1)); //左方向にスワイプ
                    m_OperationImageIndex++;
                }

                //Sound再生
                m_SelectionUpDownSE.Play();
            }
        }

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

                //選択画面をコルーチンで制御
                StartCoroutine(SelectionImageOpen());
            }
            else
            {
                //選択画像を再表示状態にリセット（次回開いた時のため）
                ShowSelectionImages();
                
                //非表示
                m_OptionsObj.SetActive(false);
                m_SelectionObj.SetActive(false);
                m_OperationObject.SetActive(false);

                //オプションが閉じている状態
                m_IsOptionsOpen = false;
                //説明画面フラグをリセット
                m_IsDescriptionOpen = false;
                //選択フラグをリセット
                m_SelectionOnImage = false;

                //カーソルを非表示に戻す
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //Sound再生
                m_ExitSE.Play();
            }
        }

        //選択処理
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //選択Sound再生
            m_PushSelectionSE.Play();

            switch (m_SelectionBox)
            {
                case 0:
                    //ゲーム画面へ戻る
                    m_OptionsObj.SetActive(false);
                    m_SelectionObj.SetActive(false);
                    m_IsOptionsOpen = false;
                    m_SelectionOnImage = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case 1:
                    //説明へ
                    //選択画像を非表示
                    HideSelectionImages();
                    //操作説明オブジェクトを表示
                    m_OperationObject.SetActive(true);
                    //矢印スプライトを全てOFFで初期化
                    ResetOperationArrowSprites();
                    //操作説明画像のインデックスを初期化
                    m_OperationImageIndex = 0;
                    //最初の画像を設定
                    if (m_OperationImageSprite.Length > 0)
                    {
                        m_OperationImage.sprite = m_OperationImageSprite[0];
                    }
                    //説明画面フラグをON
                    m_IsDescriptionOpen = true;
                    break;
                case 2:
                    //タイトルへ
                    SceneManager.LoadScene("Title");
                    break;
                case 3:
                    //終了
                    Application.Quit();
                    break;

            }
        }
    }

    /// <summary>
    /// コルーチンで選択オブジェクトを表示
    /// </summary>
    /// <returns>一秒待つ</returns>
    private IEnumerator SelectionImageOpen()
    {
        Debug.Log("処理開始");

        // 1秒待つ
        yield return new WaitForSeconds(1f);
        //Sound再生
        m_SelectionOpenSE.Play();
        //表示
        m_SelectionObj.SetActive(true);

        //表示フラグON
        m_SelectionOnImage = true;

        //初期選択状態を更新
        UpdateSelectionSprites();
    }

    /// <summary>
    /// 選択スプライトを更新する
    /// </summary>
    private void UpdateSelectionSprites()
    {
        //全てのImageを処理
        for (int i = 0; i < m_SelectionImage.Length; i++)
        {
            if (i == m_SelectionBox)
            {
                //選択されている場合はONスプライト
                m_SelectionImage[i].sprite = m_SelectionOnSprite[i];
            }
            else
            {
                //選択されていない場合はOFFスプライト
                m_SelectionImage[i].sprite = m_SelectionOffSprite[i];
            }
        }
    }

    /// <summary>
    /// 選択画像を非表示にする
    /// </summary>
    private void HideSelectionImages()
    {
        foreach (Image img in m_SelectionImage)
        {
            img.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 選択画像を表示する
    /// </summary>
    private void ShowSelectionImages()
    {
        foreach (Image img in m_SelectionImage)
        {
            img.gameObject.SetActive(true);
        }
        //スプライトを更新
        UpdateSelectionSprites();
    }

    /// <summary>
    /// 操作説明の矢印スプライトを更新する
    /// </summary>
    private void UpdateOperationArrowSprites()
    {
        //まず全ての矢印をOFFスプライトにする
        for (int i = 0; i < m_OperationArrowImage.Length; i++)
        {
            m_OperationArrowImage[i].sprite = m_OperationArrowOffSprite[i];
        }

        //選択されている矢印だけONスプライトにする
        if (m_OperationArrowIndex >= 0 && m_OperationArrowIndex < m_OperationArrowImage.Length)
        {
            m_OperationArrowImage[m_OperationArrowIndex].sprite = m_OperationArrowOnSprite[m_OperationArrowIndex];
        }
    }

    /// <summary>
    /// 全ての矢印スプライトをOFFにリセットする
    /// </summary>
    private void ResetOperationArrowSprites()
    {
        for (int i = 0; i < m_OperationArrowImage.Length; i++)
        {
            m_OperationArrowImage[i].sprite = m_OperationArrowOffSprite[i];
        }
    }

    /// <summary>
    /// 矢印スプライトを一時的にONにし0.5秒後にOFFに戻す
    /// </summary>
    /// <param name="arrowIndex">変化させる矢印のインデックス</param>
    private IEnumerator FlashArrowSprite(int arrowIndex)
    {
        //範囲チェック
        if (arrowIndex < 0 || arrowIndex >= m_OperationArrowImage.Length)
        {
            yield break;
        }
        
        //ONスプライトに変更
        m_OperationArrowImage[arrowIndex].sprite = m_OperationArrowOnSprite[arrowIndex];

        //0.5秒待つ
        yield return new WaitForSecondsRealtime(0.5f);

        //OFFスプライトに戻す
        m_OperationArrowImage[arrowIndex].sprite = m_OperationArrowOffSprite[arrowIndex];
    }

    /// <summary>
    /// 操作説明画像をスワイプアニメーションで切り替える
    /// </summary>
    /// <param name="direction">スワイプ方向（1: 右へ, -1: 左へ）</param>
    private IEnumerator SwipeOperationImage(int direction)
    {
        m_IsSwipeAnimating = true;

        RectTransform rectTransform = m_OperationImage.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = m_OperationImage.GetComponent<CanvasGroup>();
        
        //CanvasGroupがなければ追加（透明度制御用）
        if (canvasGroup == null)
        {
            canvasGroup = m_OperationImage.gameObject.AddComponent<CanvasGroup>();
        }

        Vector2 startPos = rectTransform.anchoredPosition;
        float swipeDistance = 40f; //スワイプ距離
        float duration = 0.25f; //アニメーション時間
        float elapsed = 0f;

        //スワイプアウト（現在の画像を移動しながらフェードアウト）
        Vector2 endPos = startPos + new Vector2(swipeDistance * direction, 0);
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            canvasGroup.alpha = 1f - t; //フェードアウト
            yield return null;
        }

        //完全に透明にする
        canvasGroup.alpha = 0f;

        //次の画像に切り替え
        int newIndex = m_OperationImageIndex;
        if (newIndex >= 0 && newIndex < m_OperationImageSprite.Length)
        {
            m_OperationImage.sprite = m_OperationImageSprite[newIndex];
        }

        //反対側から入ってくる位置に設定
        rectTransform.anchoredPosition = startPos + new Vector2(-swipeDistance * direction, 0);

        //スワイプイン（新しい画像を元の位置へ移動しながらフェードイン）
        elapsed = 0f;
        Vector2 swipeInStart = rectTransform.anchoredPosition;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(swipeInStart, startPos, t);
            canvasGroup.alpha = t; //フェードイン
            yield return null;
        }

        //最終状態を正確に設定
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 1f;

        m_IsSwipeAnimating = false;
    }
}
