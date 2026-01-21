using UnityEngine;
/*
public class CamCtrl : MonoBehaviour
{
    [Header("向きのベース")]
    public Transform m_MukiBase;
    [Header("カメラのベース")]
    public Transform m_CameraBase;
    [Header("初期カメラ向き")]
    public float m_Muki = 0.0f;
    [Header("最大カメラ向き")]
    public float m_MaxMuki = 30.0f;

    private void Start()
    {
        m_CameraBase.rotation = m_MukiBase.rotation;
    }
    private void Update()
    {
        m_Muki -= Input.GetAxis("Mouse Y");
        if (m_Muki > m_MaxMuki) m_Muki = m_MaxMuki;
        if (m_Muki < -m_MaxMuki) m_Muki = -m_MaxMuki;
        m_CameraBase.rotation = m_MukiBase.rotation;
        m_CameraBase.Rotate(new Vector3(m_Muki, 0, 0));
    }


}
*/
