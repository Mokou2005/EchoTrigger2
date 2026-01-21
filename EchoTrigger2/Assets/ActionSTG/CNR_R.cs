using UnityEngine;
using UnityEngine.UIElements;

public class CNR_R : MonoBehaviour
{
    public Transform m_Player;
    public Transform m_Cmmera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit hit;
        Vector3 direction = (m_Cmmera.position - m_Player.position).normalized;
        if (Physics.Raycast(m_Player.position, direction, out hit, 999))
        {
            m_Cmmera.position= hit.point;
        }
        else
        {

        }
    }
}
