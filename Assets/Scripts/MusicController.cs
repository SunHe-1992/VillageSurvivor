using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    static MusicController Inst;
    private void Awake()
    {
        if (Inst != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicSource != null)
        {
            if (TBSPlayer.UserDetail.enableMusic)
                SetVolume(1f);
            else
                SetVolume(0f);
        }
    }
    public void SetVolume(float volume)
    {
        // Clamp volume between 0 and 1
        float music_volume = Mathf.Clamp01(volume);

        if (musicSource != null)
        {
            musicSource.volume = music_volume;
        }
    }
}
