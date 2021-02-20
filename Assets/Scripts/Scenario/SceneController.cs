using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SceneController
{
    private GameController gc;
    public Actions Actions;

    private GUIManager gui;
    private AudioManager audio;
    private ParticleManager particle;
    private SceneHolder sh;
    private SceneReader sr;
    private string tempText;
    
    private float twoCameraDistance = 30f;
    
    private Sequence darkSeq = DOTween.Sequence();
    private Sequence textSeq = DOTween.Sequence();
    private Sequence imageSeq = DOTween.Sequence();
    private Sequence shakeSeq = DOTween.Sequence();
    Tweener tweener;
    private float messageSpeed = 0.05f;
    private float fadeSpeed = 2f;
    private float darknessSpeed = 1f;
    private bool isOptionsShowed;
    private bool nowDarkSeqFin = false;

    private Scene currentScene;
    public List<Character> Characters = new List<Character>();
    
    private CanvasGroup darknessGroup;
    
    public SceneController(GameController gc)
    {
        this.gc = gc;
        gui = GameObject.Find("GUI").GetComponent<GUIManager>();
        audio = GameObject.Find("Audio").GetComponent<AudioManager>();
        particle = GameObject.Find("Particle").GetComponent<ParticleManager>();
        Actions = new Actions(gc);
        sh = new SceneHolder(this);
        sr = new SceneReader(this);
        textSeq.Complete();
        darknessGroup = gui.ChangeSCImage.GetComponent<CanvasGroup>();
    }

    public void LoadChapter(string chapter)
    {
        gc.SetNowChapter(chapter);
        sh.LoadScenario(chapter);
    }
    
    public void WaitClick()
    {
        if (currentScene != null)
        {
            //暗転、明転時はクリック無視
            if (darkSeq.IsPlaying())
            {
                return;
            }
            
            if (nowDarkSeqFin)
            {
                nowDarkSeqFin = false;
                SetNextProcess();
                return;
            }
            //auto、skip機能の時は自動的に次のシナリオを読み込む
            if (gui.autoFlag || gui.skipFlag)
            {
                SetNextProcess();
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (gui.ScrollView.gameObject.activeSelf) return;
                //UI関係を非表示にしてるときは、全部表示させるだけ
                if (!gui.UIFlag())
                {
                    gui.UIAppear();
                    return;
                }
                
                //クリックを無視するオブジェクトの判定
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    tapPoint.x += twoCameraDistance;
                    Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
                    
                    if (collition2d != null)
                    {
                        var obj = collition2d.gameObject;;
                        if (obj.transform.tag == "SkipTap") return;
                    }
                }

                //クリックでシナリオを進める 
                if (!isOptionsShowed && !imageSeq.IsPlaying() && !gui.TouchSkipFlag())
                {
                    SetNextProcess();
                }
            }
        }
         
    }

    public void SetComponents()
    {
        gui.ButtonPanel.gameObject.SetActive(isOptionsShowed);
        gui.Tap.gameObject.SetActive
        (!textSeq.IsPlaying() && !isOptionsShowed && !imageSeq.IsPlaying() && !darkSeq.IsPlaying() && !gui.ScrollView.activeSelf);
    }

    public void SetNextProcess()
    {
        if (textSeq.IsPlaying())
        {
            if (gui.autoFlag)
            {
                return;
            }
            SetText(tempText);
        }
        else
        {
            sr.ReadLines(currentScene);
        }
    }
    
    public void SetBGM(string bgmName)
    {
        audio.SetBGM(bgmName);
    }

    public void SetScene(string id)
    {
        //現在のシーンをnowDataに保存*********
        gc.SetNowScene(id);
        
        if(gui.skipFlag) gui.skipFlag = false;
        currentScene = sh.Scenes.Find(s => s.ID == id);
        currentScene = currentScene.Clone();
        if (currentScene == null) Debug.LogError("scenario not found");
        SetNextProcess();
    }
    
    public void SetText(string text)
    {
        
        tempText = text;
        
        if (textSeq.IsPlaying())
        {
            textSeq.Complete();
        }
        else
        {
            string temp ="";
            gui.Text.text = "";
            gui.Text.maxVisibleCharacters = 0;
            temp = tempText;
            gui.AddLog(temp);
            
            if(gui.skipFlag) return;
            
            gui.Text.text = temp;
            textSeq = DOTween.Sequence();
            textSeq.AppendInterval(0.2f);
            textSeq.Append(gui.Text.DOMaxVisibleCharacters(
                            tempText.Length,
                            tempText.Length * messageSpeed));
            if (gui.autoFlag){
                textSeq.AppendInterval(1f);
            }
            textSeq.Play();
        }
    }
    
    public void SetOptionsPanel()
    {
        gui.ButtonPanel.gameObject.SetActive(isOptionsShowed);
    }

    public void SetSpeaker(string name = "")
    {
        gui.Speaker.text = name;
    }
    
    public void SetBackground(string name = "")
    {
        Image background = gui.Background.GetComponent<Image>();
        Sprite sprite = (Sprite)Resources.Load("Image/Background/"+name,typeof(Sprite));
        background.sprite = sprite;
    }
    //キャラ追加
    public void AddCharacter(string name,string imageID)
    {
        if (Characters.Exists(c => c.Name == name)) return;
        
        var prefab = Resources.Load("Charactor") as GameObject;
        var characterObject = UnityEngine.Object.Instantiate(prefab);
        characterObject.transform.parent = gui.BackCanvas.transform;
        var character = characterObject.GetComponent<Character>();

        character.transform.localPosition = new Vector3(0f,-60f,0f);
        character.Init(name);
        Characters.Add(character);
        SetImage(name,imageID);
        imageSeq = DOTween.Sequence();
        
        for (int i = 0; i < Characters.Count; i++)
        {
            var pos = gui.MainCamera.ScreenToWorldPoint(Vector3.zero);
            var pos2 = gui.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var posWidth = pos2.x - pos.x;
            var left = pos.x + ( posWidth / (Characters.Count+1) * (i+1) );
            var cpos = new Vector3(left, gui.MainCamera.transform.position.y, 0);
            //新しく登場する人
            if (i == Characters.Count - 1)
            {
                imageSeq.Append(Characters[i].transform.DOMoveX(cpos.x, 0f))
                    .OnComplete( () => character.Appear());
            }
            //一人目
            else if (i == 0)
            {
                imageSeq.Append(Characters[i].transform.DOMoveX(cpos.x, 0.3f)).SetEase(Ease.OutCubic);
            }
            //二人目以降
            else
            {
                cpos = new Vector3(left, cpos.y, 0);
                imageSeq.Join(Characters[i].transform.DOMoveX(cpos.x, 0.3f)).SetEase(Ease.OutCubic);
            }
            imageSeq.Play();
        }
    }
    //キャラ同時追加
    public void SetLumpChara(List<(string name, string imageID)> charas)
    {
        var prefab = Resources.Load("Charactor") as GameObject;
        
        foreach (var chara in charas)
        {
            if (Characters.Exists(c => c.Name == chara.name)) continue;
            var characterObject = UnityEngine.Object.Instantiate(prefab);
            characterObject.transform.parent = gui.BackCanvas.transform;
            var character = characterObject.GetComponent<Character>();
            character.Init(chara.name);
            Characters.Add(character);
            SetImage(chara.name,chara.imageID);
        }
        
        for (int i = 0; i < Characters.Count; i++)
        {
            var pos = gui.MainCamera.ScreenToWorldPoint(Vector3.zero);
            var pos2 = gui.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var posWidth = pos2.x - pos.x;
            var left = pos.x + (posWidth * (i + 1) / (Characters.Count + 1));
            var cpos = new Vector3(left, gui.MainCamera.transform.position.y, 0);
            Characters[i].transform.position = cpos;
            Characters[i].transform.localPosition -= new Vector3(0f,60f,0f);
        }
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].Appear();
        }
    }
    //キャラの入れ替え
    public void ReplaceChara(string name,string setName,string imageID)
    {
        if (Characters.Exists(c => c.Name == setName)) return;
        
        var prefab = Resources.Load("Charactor") as GameObject;
        var characterObject = UnityEngine.Object.Instantiate(prefab);
        characterObject.transform.parent = gui.BackCanvas.transform;
        var character = characterObject.GetComponent<Character>();

        character.transform.localPosition -= new Vector3(0f,60f,0f);
        character.Init(setName);
        for(int i=0;i<Characters.Count();i++)
        {
            if(Characters[i].Name == name)
            {
                character.transform.localPosition = Characters[i].transform.localPosition;
                Characters[i].Destroy();
                Characters[i] = character;
                SetImage(setName,imageID);
                Characters[i].Appear();
            }
        }
    }
    //キャラを消す
    public void RmCharacter(string name)
    {
        
        if (name == "all")
        {
            for(int i=Characters.Count-1;i>=0;i--)
            {
                Characters[i].Destroy();
                Characters.RemoveAt(i);
            }
            return;
        }
        
        if (!Characters.Exists(c => c.Name == name)) return;
        
        for(int i=Characters.Count-1;i>=0;i--)
        {
            if(Characters[i].Name == name)
            {
                Characters[i].Destroy();
                Characters.RemoveAt(i);
                break;
            }
        }
        
        for (int i = 0; i < Characters.Count; i++)
        {
            var pos = gui.MainCamera.ScreenToWorldPoint(Vector3.zero);
            var pos2 = gui.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var posWidth = pos2.x - pos.x;
            var left = pos.x + (posWidth * (i + 1) / (Characters.Count + 1));
            var cpos = new Vector3(left, gui.MainCamera.transform.position.y, 0);
            
            //一人目
            if (i == 0)
            {
                imageSeq.Append(Characters[i].transform.DOMoveX(cpos.x, 1.0f)).SetEase(Ease.OutCubic);
            }
            //二人目以降
            else
            {
                cpos = new Vector3(left, cpos.y, 0);
                imageSeq.Join(Characters[i].transform.DOMoveX(cpos.x, 1.0f)).SetEase(Ease.OutCubic);
            }
            imageSeq.Play();
        }
        
    }
    //キャラの画像変更
    public void SetImage(string name, string imageID)
    {
        var character = Characters.Find(c => c.Name == name);
        character.SetImage(imageID);
    }
    //アイコンのみの表示（現状はテキストウィンドウの左に小さく表示）
    public void SetSImage(string name, string imageID)
    {
        if(name == "none"){
            gui.SpeakerImage.gameObject.SetActive(false);
            return;
        }
        var temp = (Sprite)Resources.Load("Image/Charactor/"+name+"/"+imageID,typeof(Sprite));
        gui.SpeakerImage.gameObject.SetActive(true);
        gui.SpeakerImage.sprite = temp;
    }
    
    public void Shake(string name,string loop)
    {
        if(name == "text")
        {
            gui.Text.transform.DOShakePosition(0.8f,10f,10,60f,false,false);
            return;
        }
        var character = Characters.Find(c => c.Name == name);
        tweener = character.Shake(loop);
    }
    public void StopShake() {tweener.Pause();}
    
    public void CharaRelease(string name)
    {
        gc.CharaRelease(name);
    }

    //選択肢ボタンの表示
    public void SetOptions(List<(string text, string nextScene)> options)
    {
        isOptionsShowed = true;
        if(gui.skipFlag){
            gui.skipFlag = false;
            tempText = tempText.Replace("¥","\n");
            gui.Text.maxVisibleCharacters = tempText.Length;
            gui.Text.text = tempText;
        }
        foreach (var o in options)
        {
            Button b = UnityEngine.Object.Instantiate(gui.OptionButton) as Button;
            b.transform.DOScale(10f,0.5f).SetRelative(true).SetEase(Ease.OutQuart);
            TextMeshProUGUI text = b.GetComponentInChildren<TextMeshProUGUI>();
            text.text = o.text;
            b.onClick.AddListener(() => onClickedOption(o.nextScene));
            b.transform.SetParent(gui.ButtonPanel, false);
        }
    }

    public void onClickedOption(string nextID = "")
    {
        SetScene(nextID);
        isOptionsShowed = false;
        foreach (Transform t in gui.ButtonPanel)
        {
            UnityEngine.Object.Destroy(t.gameObject);
        }
    }
    
    public void DarkOn()
    {
        darkSeq = DOTween.Sequence();
        darkSeq.AppendCallback(()=>
                 {
                     gui.ChangeSCImage.SetActive(true);
                 })
                 .Append(darknessGroup.DOFade(1f,darknessSpeed))
                 .AppendInterval(0.1f)
                 .AppendCallback(()=>
                 {
                     gui.Text.text = "";
                     nowDarkSeqFin = true;
                 });
        darkSeq.Play();
    }
    public void DarkOff()
    {
        darkSeq = DOTween.Sequence();
        darkSeq.AppendInterval(0.2f)
                  .Append(darknessGroup.DOFade(0f,darknessSpeed))
                  .AppendCallback(()=>
                  {
                      gui.ChangeSCImage.SetActive(false);
                      nowDarkSeqFin = true;
                  });
        darkSeq.Play();
    }
    public bool DarkSeqPlaying()
    {
        return darkSeq.IsPlaying();
    }
    
    public void SetFallParticle(string material)
    {
        particle.SetFallParticle(material);
    }
    public void StopFallParticle()
    {
        particle.StopFallParticle();
    }
    
    public void NextChapter(string id)
    {
        DarkOn();
        gui.NextChapter(id);
    }

}
