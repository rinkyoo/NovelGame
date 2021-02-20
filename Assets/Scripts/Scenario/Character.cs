using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class Character : MonoBehaviour {

    private GameObject charactorObject;
    private SpriteRenderer charactorImage;
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private CanvasGroup canvasGroup;

    public string Name { get; private set; }
    
    private float fadeSpeed =  0.4f;
    
    Tweener tweener;
    
    void Start ()
    {
    }

    public void Init(string name)
    {
        this.Name = name;
        charactorObject = gameObject;
        charactorImage = charactorObject.GetComponent<SpriteRenderer>();
        charactorImage.transform.localScale = new Vector3(100f,100f,0);
        gameObject.SetActive(false);
        LoadImage();
    }

    public void LoadImage()
    {
        var temp = Resources.LoadAll<Sprite>("Image/Charactor/"+Name).ToList();

        foreach (Sprite s in temp)
        {
            sprites.Add(s.name, s);
        }
    }

    public void SetImage(string imageID)
    {
        charactorImage.sprite = sprites[imageID];
    }

    public void Appear()
    {
        charactorObject.SetActive(true);
        FadeIn();
    }

    public void FadeIn()
    {
        charactorImage.color = new Color(1f, 1f, 1f, 0);
        charactorImage.DOFade(1.0f, fadeSpeed);
    }
    
    public void FadeOut()
    {
        charactorImage.color = new Color(1f, 1f, 1f, 1f);
        charactorImage.DOFade(0f, fadeSpeed);
    }
    
    public Tweener Shake(string loop)
    {
        if(loop == "true"){
            return this.gameObject.transform.DOShakePosition(0.8f,10f,10,60f,false,false).SetLoops(-1,LoopType.Yoyo);
        }
        else{
            return this.gameObject.transform.DOShakePosition(0.8f,10f,10,60f,false,false);
        }
    }

    public void Destroy()
    {
        FadeOut();
        Destroy(this.gameObject,1f);
    }


}
