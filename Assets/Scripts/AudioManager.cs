using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX")]
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioClip blockSfx;
    [SerializeField] AudioClip knockOutSfx;
    [SerializeField] AudioClip roundStartSfx;
    [SerializeField] AudioClip roundEndSfx;

    [Header("Musica")]
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] float musicVolume = 0.5f;

    AudioSource sfxSource;
    AudioSource musicSource;

    void Awake()
    {
        Instance = this;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
    }

    void Start()
    {
        if (backgroundMusic == null) return;
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlayHit() => PlaySfx(hitSfx);
    public void PlayBlock() => PlaySfx(blockSfx);
    public void PlayKnockOut() => PlaySfx(knockOutSfx);
    public void PlayRoundStart() => PlaySfx(roundStartSfx);
    public void PlayRoundEnd() => PlaySfx(roundEndSfx);

    void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
