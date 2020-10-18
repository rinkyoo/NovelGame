using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaInfoManager : MonoBehaviour
{
    const int charaNum = 8;
    
    private SaveDataManager sdManager;
    CommonSaveData commonSaveData;
    
    public GameObject charaInfoPanel;
    private CanvasGroup charaInfoGroup;
    public GameObject oneCharaInfoPanel;
    
    public Image[] lockedImage = new Image[charaNum];
    public Button[] button = new Button[charaNum];
    private CanvasGroup buttonGroup;
    
    private CanvasGroup oneCharaInfoGroup;
    public Image oneCharaInfoImage;
    
    private Sprite sprite;
    
    void Awake()
    {
        sdManager = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        charaInfoGroup = charaInfoPanel.GetComponent<CanvasGroup>();
        oneCharaInfoGroup = oneCharaInfoPanel.GetComponent<CanvasGroup>();
    }
    
    void Start()
    {
        commonSaveData = sdManager.GetCommonSaveData();
        
        for(int i=0;i<charaNum;i++)
        {
            int j = i;
            button[i].onClick.AddListener(()=>OneCharaInfoClicked(j+1));
            if(!commonSaveData.charaReleased[i])
            {
                sprite = (Sprite)Resources.Load("Image/CharaInfo/Chara" +(i+1).ToString()+"Locked",typeof(Sprite));
                lockedImage[i].sprite = sprite;
                button[i].interactable = false;
                buttonGroup = button[i].GetComponent<CanvasGroup>();
                buttonGroup.alpha = 0.3f;
            }
        }
    }
    
    public void CharaInfoClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    charaInfoPanel.SetActive(true);
                })
                .Append(charaInfoGroup.DOFade(1f,0.3f));
        panelSeq.Play();
    }
    public void BackButtonClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(charaInfoGroup.DOFade(0f,0.3f))
                .AppendCallback(()=>
                {
                    charaInfoPanel.SetActive(false);
                });
        panelSeq.Play();
    }
    
    void OneCharaInfoClicked(int i)
    {
        sprite = (Sprite)Resources.Load("Image/CharaInfo/Chara" +(i).ToString()+"Info",typeof(Sprite));
        oneCharaInfoImage.sprite = sprite;
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.AppendCallback(()=>
                {
                    oneCharaInfoPanel.SetActive(true);
                })
                .Append(oneCharaInfoGroup.DOFade(1f,0.3f));
        panelSeq.Play();
    }
    public void OneCharaBackButtonClicked()
    {
        Sequence panelSeq = DOTween.Sequence();
        panelSeq.Append(oneCharaInfoGroup.DOFade(0f,0.3f))
                .AppendCallback(()=>
                {
                    oneCharaInfoPanel.SetActive(false);
                });
        panelSeq.Play();
    }
}
