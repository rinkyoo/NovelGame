using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

//シナリオを最初から始める際の処理
public class BeginningManager : MonoBehaviour
{
    const int dataNum = 8;
    private SaveDataManager sdManager;
    private PlaySaveDataHolder playSaveData;
    
    public GameObject beginPanel;
    private CanvasGroup beginGroup;
    public GameObject checkPanel;
    private CanvasGroup checkGroup;
    
    public Button[] DataButton = new Button[dataNum];
        
    void Awake()
    {
        sdManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        playSaveData = GameObject.Find("PlaySaveDataHolder").GetComponent<PlaySaveDataHolder>();
        beginGroup = beginPanel.GetComponent<CanvasGroup>();
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
    }
    
    public void BeginClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    beginPanel.SetActive(true);
                })
                .Append(beginGroup.DOFade(1f,0.5f));
        panelSeq.Play();
    }
    
    public void SaveData1Clicked()
    {
        ResetSaveData(0);
    }
    public void SaveData2Clicked()
    {
        ResetSaveData(1);
    }
    public void SaveData3Clicked()
    {
        ResetSaveData(2);
    }
    public void SaveData4Clicked()
    {
        ResetSaveData(3);
    }
    public void SaveData5Clicked()
    {
        ResetSaveData(4);
    }
    public void SaveData6Clicked()
    {
        ResetSaveData(5);
    }
    public void SaveData7Clicked()
    {
        ResetSaveData(6);
    }
    public void SaveData8Clicked()
    {
        ResetSaveData(7);
    }
    public void ResetSaveData(int dataNumber)
    {
        
        playSaveData.SetPlaySaveData(new SaveData(),dataNumber);
        
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    checkPanel.SetActive(true);
                })
                .Append(checkGroup.DOFade(0.8f,0.5f));
        panelSeq.Play();
    }
    
    public void BackButtonClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(beginGroup.DOFade(0f,0.5f))
                .AppendCallback(()=>
                {
                    beginPanel.SetActive(false);
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
        panelSeq.Append(checkGroup.DOFade(0f,0.5f))
                .AppendCallback(()=>
                {
                    checkPanel.SetActive(false);
                });
        panelSeq.Play();
    }
}
