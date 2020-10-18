using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResetManager : MonoBehaviour
{
    private SaveDataManager sdManager;
    const int dataNum = 8;
    
    public GameObject checkPanel;
    private CanvasGroup checkGroup;
    
    void Awake()
    {
        sdManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        checkGroup = checkPanel.GetComponent<CanvasGroup>();
    }
    
    public void ResetClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    checkPanel.SetActive(true);
                })
                .Append(checkGroup.DOFade(0.8f,0.3f));
        panelSeq.Play();
    }
    
    public void YesClicked()
    {
        sdManager.SetCommonSaveData(new CommonSaveData());
        for(int i=0;i<dataNum;i++)
        {
            sdManager.SetSaveData(new SaveData(),i);
        }
        SceneManager.LoadScene("Home");
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
