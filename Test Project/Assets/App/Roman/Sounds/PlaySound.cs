using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    public static PlaySound Instance { get; private set; }

    [Header("UI & Gameplay Sounds")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip buildSound;
    [SerializeField] private AudioClip errorSound;

    private AudioSource audioSourceSOUND;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSourceSOUND = GetComponent<AudioSource>();
        audioSourceSOUND.playOnAwake = false;

        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        if (clickSound != null)
            audioSourceSOUND.PlayOneShot(clickSound);
    }

    public void PlayBuild()
    {
        if (buildSound != null)
            audioSourceSOUND.PlayOneShot(buildSound);
    }
    
    public void PlayError()
    {
        if (errorSound != null)
            audioSourceSOUND.PlayOneShot(errorSound);
    }
}