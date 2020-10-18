using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private AudioClip BGM;
    private string nowBGM ="";
    private AudioSource audioSource;
    
    private float fadeSpeed = 1f;
    
    private void Start()
    {
        audioSource = GameObject.Find("Audio").GetComponent<AudioSource>();
    }
 
    public void SetBGM(string bgmName)
    {
        //同じBGMの場合は何せずに関数を抜ける
        if(nowBGM == bgmName)
        {
            return;
        }
        else
        {
            nowBGM = bgmName;
        }
        BGM = (AudioClip)Resources.Load("Audio/BGM/"+bgmName,typeof(AudioClip));
        Sequence bgmSeq = DOTween.Sequence();
        
        //if(audioSource.volume > 0f){
        bgmSeq.AppendCallback(()=>
        {
            FadeOut();
            
        });
        //}
        bgmSeq.AppendCallback(()=>
        {
            audioSource.clip = BGM;
            audioSource.Play();
            FadeIn();
        });
        bgmSeq.Play();
    }
    
    public void FadeIn()
    {
        audioSource.DOFade(1f,fadeSpeed);
    }
    
    public void FadeOut()
    {
        audioSource.DOFade(0f,fadeSpeed);
    }

}
