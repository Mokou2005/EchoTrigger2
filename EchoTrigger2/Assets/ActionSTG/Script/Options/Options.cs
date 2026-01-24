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
                //非表示
                m_OptionsObj.SetActive(false);
                m_SelectionObj.SetActive(false);

                //オプションが閉じている状態
                m_IsOptionsOpen = false;

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
                    //タイトルへ
                    SceneManager.LoadScene("Title");
                    break;
                case 2:
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
}
