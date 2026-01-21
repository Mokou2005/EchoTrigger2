using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CallSpon : MonoBehaviour
{
    public GameObject m_a;
    public bool m_b=false;
    void Start()
    {

        
        //”ñ•\Ž¦
        m_a.SetActive(false);
    }
    private void Update()
    {
        if (m_b)
        {
            m_a.SetActive(true);
        }
    }

}
