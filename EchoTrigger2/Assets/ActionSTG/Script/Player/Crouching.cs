using UnityEngine;

public class Crouching : MonoBehaviour
{
    private Animator m_CrouchingAnimator;
    //しゃがみ中
    private bool m_Down = false;
    //しゃがみ中は動き停止（public staticは他のscriptに連動）
    public static bool m_Crouching = false;

    private void Start()
    {
        m_CrouchingAnimator = GetComponent<Animator>();
    }
    public void Update()
    {
        //オプション中、メモ中、またはキーパッド操作中は操作禁止
        if (Options.m_IsOptionsOpen || Memo.m_IsMemoOpen || KeyPadRock.m_IsKeyPadOpen)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            m_Down = !m_Down;//しゃがみ状態の切り替え（On〜Off）
            m_CrouchingAnimator.SetBool("Down", m_Down);
            m_Crouching = true;
        }

    }
    //しゃがみアニメーション終了時に動く
    public void CrouchingEnd2()
    {
   
        m_Crouching = false;
    }
}
