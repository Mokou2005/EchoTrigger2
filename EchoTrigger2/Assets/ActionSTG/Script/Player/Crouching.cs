using UnityEngine;

public class Crouching : MonoBehaviour
{
    private Animator m_CrouchingAnimator;
    //しゃがみ中か
    private bool m_Down = false;
    //しゃがみ中は動きを停止（public staticは他のscriptに連動）
    public static bool m_Crouching = false;

    private void Start()
    {
        m_CrouchingAnimator = GetComponent<Animator>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_Down = !m_Down;//しゃがみ状態の切り替え（OnかOffか）
            m_CrouchingAnimator.SetBool("Down", m_Down);
            m_Crouching = true;
        }

    }
    //しゃがみアニメーションが終了したら動く
    public void CrouchingEnd2()
    {
   
        m_Crouching = false;
    }
}
