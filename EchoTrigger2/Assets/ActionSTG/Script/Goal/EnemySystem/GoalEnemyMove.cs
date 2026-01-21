using UnityEngine;
/// <summary>
/// ゴールムービに使う敵の動き
/// </summary>
public class GoalEnemyMove : MonoBehaviour
{
    [Header("移動スピード"), SerializeField]
    private float m_MoveSpeed = 2f;

    [Header("移動する方向"),SerializeField]
    private Vector3 m_MoveDirection=Vector3.right;
    [Header("アニメーションをアタッチ(自動)"), SerializeField]
    private Animator m_Animator;
    private void Start()
    {
        //ゲットコンポーネントの処理
        m_Animator = GetComponent<Animator>();
    }
    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //真っすぐ進む処理
        transform.position += m_MoveDirection.normalized * m_MoveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 敵が止まるアニメーションと移動を止める
    /// </summary>
    public void StopEnemy()
    {
        //移動速度を止める
        m_MoveSpeed = 0f;

        //アニメーションを切り替え
        m_Animator.SetBool("Stop",true);
    }
}
