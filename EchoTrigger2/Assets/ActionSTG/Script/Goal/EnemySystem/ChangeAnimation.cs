using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
/// <summary>
/// 敵のアニメーションを変える処理
/// </summary>
public class ChangeAnimation : MonoBehaviour
{
    [Header("GoalEnemyMoveのscriptをアタッチ(自動)"), SerializeField]
    private GoalEnemyMove m_GoalEnemyMove;

    [Header("Animationを変える時間"), SerializeField]
    private float m_CoroutineTime=3;
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //ゲットコンポーネント
        m_GoalEnemyMove = GetComponent<GoalEnemyMove>();

        StartCoroutine(ChangeEnemyAnimation());
    }

    //EnemyのAnimationを変える
    IEnumerator ChangeEnemyAnimation()
    {

        yield return new WaitForSeconds(m_CoroutineTime);

        m_GoalEnemyMove.StopEnemy();
    }
    
}
