using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GUIManager : MonoBehaviour
{
    
    private GameController gc;
    
    public Camera MainCamera;
    public Transform ButtonPanel;
    public Button OptionButton;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI Speaker;
    public Image SpeakerImage;
    public GameObject Background;
    public GameObject Tap;
    public GameObject ChangeSCImage;
    public GameObject BackCanvas;
    public GameObject UIPanel;
    
    //For Text Log
    public GameObject ScrollView;
    public GameObject Content;
    public GameObject textPrefab;
    
    //chapter変更する際のCanvasGroup
    public GameObject changeChapPanel;
    private CanvasGroup changeChapGroup;
    
    //セーブ関連
    const int dataNum = 8;
    int tempDataNum;
    public GameObject savePanel;
    private CanvasGroup saveGroup;
    public GameObject saveCheckPanel;
    private CanvasGroup saveCheckGroup;
    public Button[] saveDataButton = new Button[dataNum];
    //その他オプションボタン
    public GameObject homePanel;
    private CanvasGroup homeGroup;
    public Button autoButton;
    private CanvasGroup autoGroup;
    public bool autoFlag = false;
    public bool saveFlag = false;
    public bool skipFlag = false;
    
    private CanvasGroup uiGroup;
    private float fadeSpeed = 1f;
    
    private string nextChapter ="";
    
    private void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        saveGroup = savePanel.GetComponent<CanvasGroup>();
        saveCheckGroup = saveCheckPanel.GetComponent<CanvasGroup>();
        homeGroup = homePanel.GetComponent<CanvasGroup>();
        changeChapGroup = changeChapPanel.GetComponent<CanvasGroup>();
        uiGroup = UIPanel.GetComponent<CanvasGroup>();
        autoGroup = autoButton.GetComponent<CanvasGroup>();
        
        Tap.transform.DOScale(new Vector3(1.3f,1.3f),0.7f).SetLoops(-1);
            
    }
    
    public void HideClicked()
    {
        if (autoFlag)
        {
            autoFlag = false;
        }
        uiGroup.DOFade(0f,fadeSpeed);
    }
    public void UIAppear()
    {
        uiGroup.DOFade(1f,fadeSpeed);
    }
    public bool UIFlag()
    {
        return (uiGroup.alpha == 1);
    }
    public void AutoClicked()
    {
        if(autoFlag) autoGroup.alpha = 1.0f;
        else autoGroup.alpha = 0.3f;
        autoFlag = !autoFlag;
    }
    public void SkipClicked()
    {
        skipFlag = true;
    }
    
    public void HomeClicked()
    {
        //SceneManager.LoadScene("Home");
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    homePanel.SetActive(true);
                })
                .Append(homeGroup.DOFade(0.8f,0.5f));
        panelSeq.Play();
    }
    public void YesHomeClicked()
    {
        SceneManager.LoadScene("Home");
        //NoHomeClicked();
    }
    public void NoHomeClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(homeGroup.DOFade(0f,0.5f))
                .AppendCallback(()=>
                {
                    homePanel.SetActive(false);
                });
        panelSeq.Play();
    }
    
    public void SaveClicked()
    {
        gc.UpdateSaveData();
        UpdateSaveDataButton();
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    saveFlag = true;
                    savePanel.SetActive(true);
                })
                .Append(saveGroup.DOFade(0.9f,0.5f));
        panelSeq.Play();
    }
    void UpdateSaveDataButton()
    {
        for(int i=0;i<dataNum;i++)
        {
            string temp = "セーブデータ" + (i+1).ToString() + " : ";
            temp += gc.GetChapter(i);
            temp += "章/シーン";
            temp += gc.GetScene(i);
            saveDataButton[i].GetComponentInChildren<TextMeshProUGUI>().text = temp;
        }
    }
    public void SaveBackClicke()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(saveGroup.DOFade(0f,0.5f))
                .AppendCallback(()=>
                {
                    savePanel.SetActive(false);
                    saveFlag = false;
                });
        panelSeq.Play();
    }
    public void Data1Clicked(){tempDataNum = 0;SetSaveCheckPanel();}
    public void Data2Clicked(){tempDataNum = 1;SetSaveCheckPanel();}
    public void Data3Clicked(){tempDataNum = 2;SetSaveCheckPanel();}
    public void Data4Clicked(){tempDataNum = 3;SetSaveCheckPanel();}
    public void Data5Clicked(){tempDataNum = 4;SetSaveCheckPanel();}
    public void Data6Clicked(){tempDataNum = 5;SetSaveCheckPanel();}
    public void Data7Clicked(){tempDataNum = 6;SetSaveCheckPanel();}
    public void Data8Clicked(){tempDataNum = 7;SetSaveCheckPanel();}
    
    void SetSaveCheckPanel()
    {
        Sequence panelSeq = DOTween.Sequence();
            panelSeq.AppendCallback(()=>
            {
                //saveFlag = true;
                saveCheckPanel.SetActive(true);
            })
            .Append(saveCheckGroup.DOFade(1f,0.5f));
            panelSeq.Play();
    }
    public void YesSaveClicked()
    {
        /*
        gc.SaveNowData();
        gc.SaveNowCommonData();
         */
        gc.SetSaveData(tempDataNum);
        NoSaveClicked();
        SaveBackClicke();
    }
    public void NoSaveClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(saveCheckGroup.DOFade(0f,0.5f))
                .AppendCallback(()=>
                {
                    saveCheckPanel.SetActive(false);
                    saveFlag = false;
                });
        panelSeq.Play();
    }
    
    public bool TouchSkipFlag()
    {
        return saveFlag;
    }
    
    public void NextChapter(string id)
    {
        nextChapter = id;
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    changeChapPanel.SetActive(true);
                })
                .Append(changeChapGroup.DOFade(1.0f,0.5f));
        panelSeq.Play();
    }
    public void ChangeChapter()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(changeChapGroup.DOFade(0.0f,0.5f))
                .AppendCallback(()=>
                {
                    changeChapPanel.SetActive(false);
                })
                .AppendInterval(0.5f)
                .AppendCallback(()=>
                {
                  gc.ChangeChapter(nextChapter);
                });
        panelSeq.Play();
    }
    /*
    public void SaveClicked2()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    saveFlag = true;
                    savePanel.SetActive(true);
                    saveGroup.alpha = 0.8f;
                })
                .Append(changeChapGroup.DOFade(0.0f,0.5f))
                .AppendCallback(()=>
                {
                    changeChapPanel.SetActive(false);
                });
        panelSeq.Play();
    }
     */
    public void LogClicked()
    {
        ScrollView.gameObject.SetActive(true);
        Content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        ScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
    public void BackLogClicked()
    {
        ScrollView.gameObject.SetActive(false);
    }
    public void AddLog(string Logtext)
    {
        GameObject newLog = Instantiate(textPrefab) as GameObject;
        newLog.GetComponent<TextMeshProUGUI>().text = "[ "+Speaker.text+" ]\n"+Logtext;
        newLog.transform.SetParent(Content.transform);
        newLog.transform.SetAsLastSibling();
        newLog.transform.localScale = Vector3.one;
        newLog.transform.localPosition = Vector3.zero;
    }
}
