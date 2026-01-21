using UnityEngine;


public class CMRSet : MonoBehaviour
{
    [Header("カメラの基準となるオブジェクト（プレイヤーなど）")]
    public Transform m_CMRBase;

   
    [Header("回転設定")]
    public float pitchSpeed = 60f;   // 視線の上下スピード
    public float minPitch = -30f;    // 下を向く限界角度
    public float maxPitch = 45f;     // 上を向く限界角度

    [Header("壁衝突設定")]
    public float m_SafetyMargin = 0.2f;      // 壁との余白
    public LayerMask m_CollisionLayers = ~0; // 衝突検出するレイヤー

    private float m_HeightOffset = 0f; // 現在の上下オフセット値
    private float m_Pitch = 0f;        // 現在の上下回転角度
    [Header("Parametaをアタッチ")]
    public Parameta m_Parameta;


    void Update()
    {
        // Parameta があって、死んでいたらこのスクリプトを無効化
        if (m_Parameta != null && m_Parameta.m_IsDie)
        {
            //CMRSet が完全に停止する
            this.enabled = false; 
        }
    }
    void LateUpdate()
    {

        if (m_CMRBase)
        {
            
            //マウス入力で上下移動
            float mouseY = Input.GetAxis("Mouse Y");
 

            // 同時にカメラの向きも調整
            m_Pitch -= mouseY * pitchSpeed * Time.deltaTime;
            m_Pitch = Mathf.Clamp(m_Pitch, minPitch, maxPitch);

            //新しい位置（ベースの位置 + 高さオフセット）
            Vector3 newPos = m_CMRBase.position + new Vector3(0, m_HeightOffset, 0);

            // === 壁衝突検出（Linecast） ===
            // カメラの向いている方向にLinecastして、壁があれば手前に移動
            RaycastHit hit;
            Vector3 lookDirection = transform.forward;
            float checkDistance = 1.0f; // 前方チェック距離

            // 後方の壁をチェック（カメラがプレイヤーの後ろに壁があるか）
            if (Physics.Linecast(m_CMRBase.position, newPos, out hit, m_CollisionLayers))
            {
                // 壁があれば、壁の手前に配置
                newPos = hit.point + hit.normal * m_SafetyMargin;
            }

            transform.position = newPos;

            //ベースの回転を継承しつつ、上下角度を加える
            Quaternion baseRot = m_CMRBase.rotation;
            Quaternion pitchRot = Quaternion.Euler(m_Pitch, 0f, 0f);
            transform.rotation = baseRot * pitchRot;
        }
    }
}




