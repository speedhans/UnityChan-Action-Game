using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static int MaximumFXCount = 15;

    static SoundManager single;
    static public SoundManager Instance
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("SoundManager");
                single = g.AddComponent<SoundManager>();
                DontDestroyOnLoad(g);
                single.Initialize();
            }
            return single;
        }
    }

    void Initialize()
    {
        GameObject g = new GameObject("BGM Audio");
        m_BGMPlayer = g.AddComponent<AudioSource>();
        m_BGMPlayer.transform.SetParent(this.transform);
        m_BGMPlayer.loop = false;
        m_BGMPlayer.playOnAwake = false;
        m_BGMPlayer.Stop();

        InitializeInstanceAudio();
        InitializeDefaultAudio();
    }

    GameObject m_3DAudioPrefab;
    GameObject m_DefaultAudioPrefab;
    List<AudioSource> m_3DAudioList = new List<AudioSource>();
    List<AudioSource> m_DefaultAudioList = new List<AudioSource>();
    List<AudioClip> m_BGMList = new List<AudioClip>();
    int m_BGMNumber;
    AudioSource m_BGMPlayer;
    Coroutine m_BGMCoroutine;

    float m_Volume = 1.0f;
    float m_BGMVolume = 1.0f;

    void InitializeInstanceAudio()
    {
        m_3DAudioPrefab = new GameObject("Audio");
        m_3DAudioPrefab.transform.SetParent(this.transform);
        AudioSource audio = m_3DAudioPrefab.AddComponent<AudioSource>();
        audio.spatialBlend = 1.0f;
        audio.spread = 360.0f;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 25.0f;
        m_3DAudioList.Add(audio);
        for (int i = 0; i < 9; ++i)
        {
            m_3DAudioList.Add(Create3DAudio());
        }
    }

    AudioSource Create3DAudio()
    {
        GameObject g = Instantiate(m_3DAudioPrefab, this.transform);
        return g.GetComponent<AudioSource>();
    }

    void InitializeDefaultAudio()
    {
        m_DefaultAudioPrefab = new GameObject("Audio");
        m_DefaultAudioPrefab.transform.SetParent(this.transform);
        AudioSource audio = m_DefaultAudioPrefab.AddComponent<AudioSource>();
        m_3DAudioList.Add(audio);
        for (int i = 0; i < 9; ++i)
        {
            m_DefaultAudioList.Add(CreateDefaultAudio());
        }
    }

    AudioSource CreateDefaultAudio()
    {
        GameObject g = Instantiate(m_DefaultAudioPrefab, this.transform);
        return g.GetComponent<AudioSource>();
    }

    public AudioSource Play3DSound(Vector3 _AudioPosition, AudioClip _Clip, float _Volume = 1.0f)
    {
        for (int i = 0; i < m_3DAudioList.Count; ++i)
        {
            if (!m_3DAudioList[i].isPlaying)
            {
                m_3DAudioList[i].transform.position = _AudioPosition;
                m_3DAudioList[i].volume = Get3DVolume() * _Volume;
                m_3DAudioList[i].PlayOneShot(_Clip);
                return m_3DAudioList[i];
            }
        }

        if (m_3DAudioList.Count >= MaximumFXCount)
        {
            m_3DAudioList[0].transform.position = _AudioPosition;
            m_3DAudioList[0].volume = Get3DVolume() * _Volume;
            m_3DAudioList[0].PlayOneShot(_Clip);
            return m_3DAudioList[0];
        }

        AudioSource a = Create3DAudio();
        a.transform.position = transform.position = _AudioPosition;
        a.volume = Get3DVolume() * _Volume;
        a.PlayOneShot(_Clip);
        m_3DAudioList.Add(a);
        return a;
    }

    public AudioSource PlayDefaultSound(AudioClip _Clip, float _Volume = 1.0f, bool _Loop = false)
    {
        for (int i = 0; i < m_DefaultAudioList.Count; ++i)
        {
            if (!m_DefaultAudioList[i].isPlaying)
            {
                if (_Loop)
                {
                    m_DefaultAudioList[i].clip = _Clip;
                    m_DefaultAudioList[i].volume = m_Volume * _Volume;
                    m_DefaultAudioList[i].Play();
                }
                else
                {
                    m_DefaultAudioList[i].volume = m_Volume * _Volume;
                    m_DefaultAudioList[i].PlayOneShot(_Clip);
                }
                return m_DefaultAudioList[i];
            }
        }

        if (m_DefaultAudioList.Count >= MaximumFXCount)
        {
            for (int i = 0; i < m_DefaultAudioList.Count; ++i)
            {
                if (m_DefaultAudioList[i].loop != _Loop)
                {
                    m_DefaultAudioList[i].volume = m_Volume * _Volume;
                    m_DefaultAudioList[i].PlayOneShot(_Clip);
                    return m_DefaultAudioList[i];
                }
            }
            return null;
        }

        AudioSource a = CreateDefaultAudio();
        a.PlayOneShot(_Clip);
        m_DefaultAudioList.Add(a);
        return a;
    }

    public void PlayBGM(List<AudioClip> _Clip)
    {
        m_BGMList = _Clip;
        m_BGMNumber = 0;

        if (m_BGMCoroutine != null) StopCoroutine(m_BGMCoroutine);
        m_BGMPlayer.Stop();
        m_BGMCoroutine = StartCoroutine(C_BGMPlay(3.0f));
    }

    IEnumerator C_BGMPlay(float _WaitTime)
    {
        float wait = 0.0f;
        while(true)
        {
            if (!m_BGMPlayer.isPlaying)
            {
                yield return new WaitForSeconds(wait);
                m_BGMPlayer.clip = m_BGMList[m_BGMNumber];
                m_BGMPlayer.Play();
                ++m_BGMNumber;
                if (m_BGMNumber >= m_BGMList.Count)
                    m_BGMNumber = 0;
                wait = _WaitTime;
            }

            yield return null;
        }
    }

    public void SetVolume(float _Volume)
    {
        m_Volume = Mathf.Clamp01(_Volume);
        foreach (AudioSource a in m_3DAudioList)
        {
            a.volume = m_Volume * 0.5f;
        }

        foreach (AudioSource a in m_DefaultAudioList)
        {
            a.volume = m_Volume;
        }
    }

    public void SetBGMVolume(float _Volume) 
    {
        m_BGMVolume = _Volume;
        m_BGMPlayer.volume = m_BGMVolume;
    }

    public float GetDefaultVolume() { return m_Volume; }
    public float Get3DVolume() { return m_Volume * 0.5f; }
    public float GetBGMVolume() { return m_BGMVolume; }
}
