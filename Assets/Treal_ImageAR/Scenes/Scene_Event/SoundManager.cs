using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    public AudioClip Yes;
    public AudioClip No;
    public AudioClip Clear;
    public AudioClip NextQ;
    public AudioClip Button;
    public AudioClip Boomb;
    public AudioClip popup;
    public AudioClip Done;
    AudioSource MySource;
    public AudioSource BGM;
   double t = 0;

    public enum SoundName
    {
        Answer = 0,
        NoAnswer,
        NextQuiz,
        ButtonPush,
        GameClear,
            Boomb,
            popup,
            Done,
    }

	// Use this for initialization
	void Start () {
        MySource = GetComponent<AudioSource>();
        if (MySource == null)
            Debug.Log("오디소소스 찾기실패");
	}

    public void StartBGM()
    {
        BGM.Play();
    }
    public void OutSound()
    {
        StartCoroutine(SoundFade());
        Debug.Log("fadeOutBgm  ");
    }
    IEnumerator SoundFade()
    {
        float MaxVolume = BGM.volume;
        if (BGM.volume != 0)
        {
            while (t <= 1)
            {
                t += 0.02;
                BGM.volume = (float)(MaxVolume - t);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void SetSoundVolume(float nValue)
    {
        BGM.volume = nValue;
    }

    public bool AudioSourceCheck()
    {
        return MySource.isPlaying;
    }


    public void AudioPlay(SoundName num)
    {
        switch(num)
        {
            case SoundName.Answer:
                {
                    MySource.PlayOneShot(Yes);
                }
                break;
            case SoundName.NoAnswer:
                {
                    MySource.PlayOneShot(No);
                }
                break;
            case SoundName.GameClear:
                {
                    MySource.PlayOneShot(Clear);
                }
                break;
            case SoundName.NextQuiz:
                {
                    MySource.PlayOneShot(NextQ);
                }
                break;
            case SoundName.ButtonPush:
                {
                    MySource.PlayOneShot(Button);
                }
                break;
            case SoundName.Boomb:
                {
                    MySource.PlayOneShot(Boomb);
                }
                break;
            case SoundName.popup:
                {
                    MySource.PlayOneShot(popup);
                }
                break;
            case SoundName.Done:
                {
                    MySource.PlayOneShot(Done);
                }
                break;
        }
        
    }
}
