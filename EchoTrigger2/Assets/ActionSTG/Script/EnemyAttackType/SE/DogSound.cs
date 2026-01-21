using UnityEngine;

public class DogSound : MonoBehaviour
{
    [Header("‰“–i‚¦")]
    public AudioClip m_Howling;
    [Header("Šš‚Ý‚Â‚«‰¹")]
    public AudioClip m_BiteSound;
    public AudioSource m_Source;

    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }
    public void Sound(AudioClip clip)
    {

        m_Source.PlayOneShot(clip);
    }

    public void Howl()
    {
        Sound(m_Howling);
        Sound(m_BiteSound);

    }
}
