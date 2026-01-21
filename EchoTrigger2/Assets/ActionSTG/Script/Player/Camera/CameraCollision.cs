using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("カメラの注視ターゲット（プレイヤー）")]
    public Transform m_Target;

    [Header("カメラの理想距離")]
    public float m_Distance = 4.0f;

    [Header("高さの補正（プレイヤーの背中より少し上）")]
    public Vector3 m_Offset = new Vector3(0, 2, 0);

    [Header("壁との余白距離")]
    public float m_SafetyMargin = 0.3f;

    [Header("追従スピード")]
    public float m_MoveSpeed = 10f;

    [Header("カメラの衝突半径")]
    public float m_CameraRadius = 0.4f;

    // デフォルトは全レイヤー
    [Header("衝突検出するレイヤー（壁・床など）")]
    public LayerMask m_CollisionLayers = ~0;

    [Header("上下回転設定")]
    public float pitchSpeed = 60f;   // 視線の上下スピード
    public float minPitch = -30f;    // 下を向く限界角度
    public float maxPitch = 45f;     // 上を向く限界角度

    [Header("Parametaをアタッチ")]
    public Parameta m_Parameta;

    private float m_Pitch = 0f;      // 現在の上下回転角度
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
        
        if (m_Target == null) return;

        // 初期位置をターゲットの後方に設定
        Vector3 targetPosition = m_Target.position + m_Offset;
        Vector3 initialPos = targetPosition - m_Target.forward * m_Distance;
        transform.position = initialPos;
        transform.LookAt(targetPosition);
    }

    private void Update()
    {
        // Parameta があって、死んでいたらこのスクリプトを無効化
        if (m_Parameta != null && m_Parameta.m_IsDie)
        {
            this.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (m_Target == null) return;

        // マウス入力で上下角度を調整
        float mouseY = Input.GetAxis("Mouse Y");
        m_Pitch -= mouseY * pitchSpeed * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, minPitch, maxPitch);

        // 注視点（プレイヤーの頭位置）
        Vector3 targetPosition = m_Target.position + m_Offset;

        // カメラの回転を計算（プレイヤーの回転 + 上下角度）
        Quaternion baseRot = m_Target.rotation;
        Quaternion pitchRot = Quaternion.Euler(m_Pitch, 0f, 0f);
        Quaternion finalRot = baseRot * pitchRot;

        // カメラの後方方向（回転を考慮）
        Vector3 backDirection = finalRot * Vector3.back;

        // 理想的なカメラ位置
        Vector3 desiredPos = targetPosition + backDirection * m_Distance;

        // === 壁衝突検出 ===
        float allowedDistance = m_Distance;
        
        // プレイヤーからカメラ方向にRaycastして壁を検出
        RaycastHit hit;
        if (Physics.SphereCast(targetPosition, m_CameraRadius, backDirection, out hit, m_Distance, m_CollisionLayers))
        {
            // 壁があれば、その手前で止める
            allowedDistance = hit.distance - m_SafetyMargin;
            allowedDistance = Mathf.Max(allowedDistance, 0.5f);
        }

        // 新しいカメラ位置
        Vector3 newCameraPos = targetPosition + backDirection * allowedDistance;

        // === 現在位置から新しい位置への移動をチェック ===
        Vector3 currentPos = transform.position;
        Vector3 moveDirection = newCameraPos - currentPos;
        float moveDistance = moveDirection.magnitude;

        if (moveDistance > 0.01f)
        {
            // 移動方向に壁があるかチェック
            if (Physics.SphereCast(currentPos, m_CameraRadius, moveDirection.normalized, out hit, moveDistance, m_CollisionLayers))
            {
                // 壁がある場合、移動を制限
                float safeMove = hit.distance - m_SafetyMargin;
                if (safeMove > 0)
                {
                    newCameraPos = currentPos + moveDirection.normalized * safeMove;
                }
                else
                {
                    // 壁が近すぎる場合は現在位置を維持
                    newCameraPos = currentPos;
                }
            }
        }

        // === 壁の中にいる場合は押し出す ===
        Collider[] colliders = Physics.OverlapSphere(newCameraPos, m_CameraRadius, m_CollisionLayers);
        foreach (Collider col in colliders)
        {
            Vector3 closestPoint = col.ClosestPoint(newCameraPos);
            Vector3 pushDir = newCameraPos - closestPoint;
            float depth = m_CameraRadius - pushDir.magnitude;
            
            if (depth > 0 && pushDir.magnitude > 0.001f)
            {
                // 壁から押し出す
                newCameraPos += pushDir.normalized * (depth + m_SafetyMargin);
            }
            else if (pushDir.magnitude <= 0.001f)
            {
                // 完全に壁の中にいる場合はプレイヤー方向に移動
                newCameraPos = targetPosition + backDirection * 0.5f;
                break;
            }
        }

        // 最終確認：プレイヤーが見えるか
        if (Physics.Linecast(targetPosition, newCameraPos, out hit, m_CollisionLayers))
        {
            Vector3 toCamera = (newCameraPos - targetPosition).normalized;
            newCameraPos = hit.point - toCamera * m_SafetyMargin;
        }

        // カメラ位置を適用
        transform.position = newCameraPos;

        // カメラの回転を適用
        transform.rotation = finalRot;
    }
}


