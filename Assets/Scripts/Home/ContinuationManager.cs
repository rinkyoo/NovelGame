using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class ContinuationManager : MonoBehaviour
{
    const int dataNum = 8;
    private SaveDataManager sdManager;
    private PlaySaveDataHolder playSaveData;
    
    public GameObject contiPanel;
    private CanvasGroup contiGroup;
    public GameObject checkPanel;
    private CanvasGroup checkGroup;
    
    public Button[] DataButton = new Button[dataNum];
    
    void Awake()
    {
        sdManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        playSaveData = GameObject.Find("PlaySaveDataHolder").GetComponent<PlaySaveDataHolder>();
        contiGroup = contiPanel.GetComponent<CanvasGroup>();
        checkGroup = checkPanel.GetComponent<CanvasGroup>();    
    }
    
    void Start()
    {
        for (int i=0;i<dataNum;i++)
        {
            string temp = "セーブデータ" + (i+1).ToString() + "\n";
            temp += sdManager.GetChapter(i);
            temp += "章/シーン";
            temp += sdManager.GetScene(i);
            DataButton[i].GetComponentInChildren<Text>().text = temp;
        }
        
        playSaveData.SetCommonSaveData(sdManager.GetCommonSaveData());
    }
    
    public void ContiClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    contiPanel.SetActive(true);
                })
                .Append(contiGroup.DOFade(1f,0.5f));
        panelSeq.Play();
    }
    
    public void SaveData1Clicked()
    {
        SaveDataButton(0);
    }
    
    public void SaveData2Clicked()
    {
        SaveDataButton(1);
    }
    public void SaveData3Clicked()
    {
        SaveDataButton(2);
        
    }
    public void SaveData4Clicked()
    {
        SaveDataButton(3);
        
    }
    public void SaveData5Clicked()
    {
        SaveDataButton(4);
        
    }
    public void SaveData6Clicked()
    {
        SaveDataButton(5);
        
    }
    public void SaveData7Clicked()
    {
        SaveDataButton(6);
        
    }
    public void SaveData8Clicked()
    {
        SaveDataButton(7);
        
    }
    
    public void SaveDataButton(int dataNumber)
    {
        playSaveData.SetPlaySaveData(sdManager.GetSaveData(dataNumber),dataNumber);
        
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    checkPanel.SetActive(true);
                })
                .Append(checkGroup.DOFade(0.8f,0.3f));
        panelSeq.Play();
    }
    public void BackButtonClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(contiGroup.DOFade(0f,0.3f))
                .AppendCallback(()=>
                {
                    contiPanel.SetActive(false);
                });
        panelSeq.Play();
    }
    
    public void YesClicked()
    {
        SceneManager.LoadScene("Scenario");
    }
    public void NoClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(checkGroup.DOFade(0f,0.3f))
                .AppendCallback(()=>
                {
                    checkPanel.SetActive(false);
                });
        panelSeq.Play();
    }
}
