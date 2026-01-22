using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// 敵方向UI表示機能
/// プレイヤーが指定キーを押している間だけ敵の方向を表示
/// </summary>
public class EnemyIndicator : MonoBehaviour
{
    /// <summary>
    /// インジケーターデータ（矢印UIと距離テキスト）
    /// </summary>
    private struct IndicatorData
    {
        public RectTransform ArrowRect;
        public TextMeshProUGUI DistanceText;
    }

    [Header("矢印Prefab"), SerializeField]
    GameObject m_ArrowPrefab;

    [Header("アビリティの発動中のPrefab"), SerializeField]
    GameObject m_ActivationPrefab;

    [Header("クールタイムText"), SerializeField]
    TextMeshProUGUI m_CoolingTimeText;

    [Header("クールタイムのフレーム"), SerializeField]
    Image m_CoolingTimeframe;

    [Header("中心基準点となる親オブジェクト"), SerializeField]
    RectTransform m_CenterScreen;

    [Header("プレイヤーのTransform"), SerializeField]
    Transform m_Player;

    [Header("UIを配置する半径"), SerializeField]
    float m_Radius = 350;

    [Header("インジケーター表示キー"), SerializeField]
    KeyCode m_ShowIndicatorKey = KeyCode.Tab;

    [Header("アビリティのクールタイム"), SerializeField]
    private float m_LookAbilityCoolingTime = 60f;

    [Header("アビリティの持ち時間"), SerializeField]
    private float m_LookAbilityNowTime = 10f;

    [Tooltip("敵とUIのペア辞書")]
    Dictionary<Transform, IndicatorData> m_Indicators = new Dictionary<Transform, IndicatorData>();

    //タイマーを設定
    [SerializeField] private float m_AbilityNowTime;
    [SerializeField] private float m_AbilityCoolingTime;

    /// <summary>
    /// インジケーターが表示中かどうか（アビリティ発動中か）
    /// </summary>
    bool m_IsActive = false;

    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //最初はクールタイム完了状態（0 = 発動可能）
        m_AbilityCoolingTime = 0f;

        //初期値に設定
        m_AbilityNowTime = 0f;

        //テキストに値を代入（0なら発動可能表示）
        UpdateCoolingTimeText();

        //非表示
        m_IsActive = false;
        m_ActivationPrefab.gameObject.SetActive(false);

        //クールタイムフレームも非表示（最初は発動可能なので）
        if (m_CoolingTimeframe != null)
            m_CoolingTimeframe.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //表示なら発動時間へ
        if (m_IsActive)
        {
            NowTime();
        }
        else
        {
            //クールタイムへ
            CoolingTime();
            // 発動していないときは隠す
            HideAllIndicators(); 
        }

    }

    /// <summary>
    /// クールタイムの時間を計測
    /// </summary>
    void CoolingTime()
    {
        // クールタイムがまだ残っているなら減算する（60→0）
        if (m_AbilityCoolingTime > 0f)
        {
            m_AbilityCoolingTime -= Time.deltaTime;
            
            // 0以下にならないようにクランプ
            if (m_AbilityCoolingTime < 0f)
                m_AbilityCoolingTime = 0f;

            // テキストを更新
            UpdateCoolingTimeText();

            // クールタイムが0になったらフレームを非表示
            if (m_AbilityCoolingTime <= 0f && m_CoolingTimeframe != null)
            {
                m_CoolingTimeframe.gameObject.SetActive(false);
            }
        }
        else
        {
            // クールタイム完了済み（0）。キー入力を待つ
            if (Input.GetKeyDown(m_ShowIndicatorKey))
            {
                Debug.Log("アビリティ発動！");
                m_IsActive = true;
                m_AbilityNowTime = 0f;
            }
        }
    }

    /// <summary>
    /// アビリティが発動中の時間
    /// </summary>
    void NowTime()
    {
        // 時間を加算
        m_AbilityNowTime += Time.deltaTime;

        //表示
        m_ActivationPrefab.gameObject.SetActive(true);

        // インジケーターを更新
        UpdateIndicators();

        // 制限時間を超えたら終了
        if (m_AbilityNowTime >= m_LookAbilityNowTime)
        {
            Debug.Log("アビリティ終了！クールダウンに入ります。");

            //非表示
            m_ActivationPrefab.gameObject.SetActive(false);

            // 状態をリセット
            m_AbilityNowTime = 0f;
            m_IsActive = false;

            // クールタイムを最大値からスタートさせる（60→0へカウントダウン）
            m_AbilityCoolingTime = m_LookAbilityCoolingTime;
            UpdateCoolingTimeText();

            // クールタイムフレームを表示
            if (m_CoolingTimeframe != null)
                m_CoolingTimeframe.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// クールタイムテキストを更新する
    /// </summary>
    void UpdateCoolingTimeText()
    {
        if (m_CoolingTimeText != null)
        {
            // 残り秒数を整数で表示
            m_CoolingTimeText.text = $"{Mathf.CeilToInt(m_AbilityCoolingTime)}";
        }
    }

    /// <summary>
    /// 全てのインジケーターを非表示にする
    /// </summary>
    void HideAllIndicators()
    {
        foreach (var pair in m_Indicators)
        {
            if (pair.Value.ArrowRect != null)
            {
                pair.Value.ArrowRect.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 敵がカメラ外にいる場合、方向矢印を計算
    /// </summary>
    void UpdateIndicators()
    {
        ManagePool();

        // カメラの前方向（0度）を基準として定義
        Vector3 camForward = Camera.main.transform.forward;
        // 水平方向のみ考慮
        camForward.y = 0f;

        foreach (var pair in m_Indicators)
        {
            // 敵の座標（角度計算に使用）
            Transform enemy = pair.Key;
            // 矢印UIの座標
            IndicatorData indicator = pair.Value;
            RectTransform arrow = indicator.ArrowRect;

            // 敵が既に破棄されている場合はスキップ
            if (enemy == null || arrow == null) continue;

            // 3D空間の敵座標をスクリーン座標（ピクセル）に変換
            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position);

            // 敵が画面外にいるかどうか判定
            bool isOffScreen = screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

            // プレイヤーと敵の距離を計算（メートル単位）
            float distance = Vector3.Distance(m_Player.position, enemy.position);

            // 画面外なら表示
            if (isOffScreen)
            {
                arrow.gameObject.SetActive(true);

                // カメラから敵への方向を計算
                Vector3 direction = enemy.position - m_Player.position;
                // 水平方向のみ
                direction.y = 0;

                // 相対角度を計算
                float angle = Vector3.SignedAngle(camForward, direction, Vector3.up);

                // 度数をラジアンに変換
                float rad = angle * Mathf.Deg2Rad;

                // 中心からの円周上の座標を計算
                arrow.anchoredPosition = new Vector2(Mathf.Sin(rad) * m_Radius, Mathf.Cos(rad) * m_Radius);

                // 矢印を敵の方向に回転
                arrow.localRotation = Quaternion.Euler(0, 0, -angle);

                // 距離テキストを更新
                if (indicator.DistanceText != null)
                {
                    indicator.DistanceText.text = $"{distance:F0}m";
                    // テキストの回転をリセット（常に読みやすい向きに）
                    indicator.DistanceText.transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                // 画面内の敵は非表示
                arrow.gameObject.SetActive(false);
            }
        }
    }

    void ManagePool()
    {
        // BattleManagerが存在しない場合は処理しない
        if (BattleManager.m_BattleInstance == null) return;

        // リストにあるがUIがない敵を追加
        foreach (var enemy in BattleManager.m_BattleInstance.m_ActiveEnemies)
        {
            // 敵が未登録なら追加
            if (!m_Indicators.ContainsKey(enemy))
            {
                // 生成
                GameObject newArrow = Instantiate(m_ArrowPrefab, m_CenterScreen);

                // インジケーターデータを作成
                IndicatorData indicatorData = new IndicatorData
                {
                    ArrowRect = newArrow.GetComponent<RectTransform>(),
                    DistanceText = newArrow.GetComponentInChildren<TextMeshProUGUI>()
                };

                // 敵を登録
                m_Indicators.Add(enemy, indicatorData);
            }
        }

        // 削除対象を一時リストに保存
        List<Transform> toRemove = new List<Transform>();

        foreach (var key in m_Indicators.Keys)
        {
            // keyがnullか、辞書の敵がリストにいない（死亡、倒した等）
            if (key == null || !BattleManager.m_BattleInstance.m_ActiveEnemies.Contains(key))
            {
                // 削除リストに追加
                toRemove.Add(key);
            }
        }

        foreach (var key in toRemove)
        {
            // 削除リストにある敵のUIを破棄
            if (m_Indicators[key].ArrowRect != null)
            {
                Destroy(m_Indicators[key].ArrowRect.gameObject);
            }

            // ペアリストから削除
            m_Indicators.Remove(key);
        }
    }
}
