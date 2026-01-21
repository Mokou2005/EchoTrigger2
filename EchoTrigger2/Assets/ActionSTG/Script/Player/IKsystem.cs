using UnityEngine;

public class IKsystem : MonoBehaviour
{
    [Header("Animationリンク")]
    public Animator m_Animator;
    [Header("左手の位置")]
    public Transform HandTarget;
    [Header("左手のIKの重さ")]
    public float HandIKWeight;
    private void OnAnimatorIK(int layerIndex)
    {
        if (m_Animator == null) return;

        if (HandTarget != null)
        {
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKWeight);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKWeight);


            // 実際の位置と回転をターゲットに一致させる
            m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, HandTarget.position);
            m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, HandTarget.rotation);

        }
    }
}
