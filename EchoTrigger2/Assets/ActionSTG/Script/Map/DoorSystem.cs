using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Animator m_Animator;       
    public AudioClip m_Doorsound;    
    private AudioSource m_audioSource;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // トリガーに入った瞬間
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Parameta>())
        {
            m_audioSource.PlayOneShot(m_Doorsound);
            OC_DoorAnimaier(true);
        }
    }

    // トリガーから出た瞬間
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Parameta>())
        {
            m_audioSource.PlayOneShot(m_Doorsound);
            OC_DoorAnimaier(false);
        }
    }
    //アニメーターを関数化
    public void OC_DoorAnimaier(bool Flag)
    {
        m_Animator.SetBool("Open", Flag);
    }
}
