using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class StageManager : MonoBehaviour
{
    public static int stageLv;
    /// <summary>
    /// 方框預錄物件
    /// </summary>
    public GameObject AnsPrefabs;
    /// <summary>
    /// 產生方框UI的父物件
    /// </summary>
    public Transform AnserUI;
    public TextMeshProUGUI LlevelText;
    public List<StageData> stageDatas;
    [HideInInspector]
    public GameObject ImageGroup;
    [HideInInspector]
    public TextMeshProUGUI TopicText , AnserText;
    public static StageManager stageManager;
    public static StageData NowStageData;
    [Header("新手教學專用")]
    public Sprite TutorialSprite;
    // Start is called before the first frame update
    void Start()
    {
        stageManager = this;
        ImageGroup = GameObject.Find("ImageGroup");
        TopicText = GameObject.Find("TopicText").GetComponent<TextMeshProUGUI>();
        AnserText = GameObject.Find("AnserText").GetComponent<TextMeshProUGUI>();       
        LoadStage(0);

    }
    [Header("動畫設定相關")]
    public bool Shake;
    public GameObject obj;
    public float time1 , time2 , SizeY , SizeX  ;
    public Vector3 vector3;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Shake)
        {
            Shake = false;
            obj.transform.localScale = new Vector3(1, 0, 1);
            obj.transform.DOScaleY(SizeY, time1).OnComplete(() => { obj.transform.DOScaleY(1, time2); });
            obj.transform.DOScaleX(SizeX, time1).OnComplete(() => { obj.transform.DOScaleX(1, time2); });
            obj.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            obj.transform.GetComponent<Image>().DOFade(1, time1);            
        }   
    }
    /// <summary>
    /// 執行動畫 Scale變形
    /// </summary>
    /// <param name="obj"></param>
    public void ScaleAni(GameObject obj)
    {
        obj.transform.localScale = new Vector3(1, 0, 1);
        obj.transform.DOScaleY(SizeY, time1).OnComplete(() => { obj.transform.DOScaleY(1, time2); });
        obj.transform.DOScaleX(SizeX, time1).OnComplete(() => { obj.transform.DOScaleX(1, time2); });
        obj.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        obj.transform.GetComponent<Image>().DOFade(1, time1);
    }
    
    /// <summary>
    /// 讀取關卡資料
    /// </summary>
    /// <param name="lv">傳入第幾個關卡</param>
    public void LoadStage(int lv)
    {
        LlevelText.text = $"Level {lv + 1}";
        var stageData = stageDatas[lv];
        NowStageData = stageData;
        for (int i = 0; i < AnserUI.childCount; i++)
        {
            Destroy(AnserUI.GetChild(i).gameObject);
        }

        #region 生成上方UI

        if (NowStageData.Mode == StageData.StageMode.Mode0 || NowStageData.Mode == StageData.StageMode.Mode1)
        {
            for (int i = 0; i < stageData.spriteAns.Count; i++)
            {
                var obj = Instantiate(AnsPrefabs, AnserUI);
                //如果是新手教學
                if (NowStageData.Mode == StageData.StageMode.Mode0)
                {
                    Debug.Log("新手教學觸發");

                    var obj_ = Instantiate(ImageGroup.transform.GetChild(0).gameObject, obj.transform.GetChild(2));
                    obj_.name = "ans";
                    //Debug.Log(obj.name);
                    obj.transform.GetChild(2).GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Fill;
                    obj_.GetComponent<Image>().sprite = NowStageData.Mode0Sprite;
                    obj_.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                    obj_.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    obj_.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    obj_.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
                    obj_.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                    ScaleAni(obj_);
                    obj_.transform.localScale = Vector3.one;
                    Destroy(obj_.GetComponent<EventTrigger>());

                }
            }
        }        
        //客製化 該死的日期關卡=.=
        else if(NowStageData.Mode == StageData.StageMode.Mode2)
        {
            if (Screen.height == 1350)
            {
                var obj = Instantiate(Resources.Load<GameObject>("DateBig"), AnserUI);
            }
            else
            {
                var obj = Instantiate(Resources.Load<GameObject>("DateStage"), AnserUI);
            }

        }
        //單一框框關卡
        else if (NowStageData.Mode == StageData.StageMode.Mode3)
        {
            if (Screen.height == 1350)
            {
                var obj = Instantiate(Resources.Load<GameObject>("SingleBig"), AnserUI);
            }
            else
            {
                var obj = Instantiate(Resources.Load<GameObject>("Single"), AnserUI);
            }
        }

        #endregion 生成上方UI

        #region 設定題目 與 解答文字

        TopicText.text = stageData.Topic;
        TopicText.GetComponent<Animation>().Play("TopicAni");
        AnserText.text = stageData.Answer;        
        AnserText.text =  AnserText.text.Replace("\\n", "\n");
        AnserText.enabled = false;
        ImageGroup.SetActive(true);

        #endregion 設定題目 與 解答文字
        //出現可選擇的圖片
        for (int i  = 0; i <　ImageGroup.transform.childCount; i++)
        {
            ImageGroup.transform.GetChild(i).GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
            var image = ImageGroup.transform.GetChild(i).GetComponent<Image>();
            if (i >= stageData.ImageOrder.Count)
            {
                image.gameObject.SetActive(false);
            }
            else
            {
                image.sprite = stageData.ImageOrder[i];
                image.gameObject.SetActive(true);
                ScaleAni(image.gameObject);
            }
            /*
            if (image.sprite == stageData.AnsImg1)
            {
                ImageGroup.transform.GetChild(i).GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans1;
            }
            if(image.sprite == stageData.AnsImg2)
            {
                ImageGroup.transform.GetChild(i).GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans2;
            }
            */
        }
        Canvas.ForceUpdateCanvases();
    }
    [Serializable]
    public class StageData
    {
        /// <summary>
        /// 題目
        /// </summary>
        public string Topic;
        /// <summary>
        /// 解答
        /// </summary>
        public string Answer;
        /// <summary>
        /// 關卡模式 ,模式用來決定UI生成
        /// </summary>
        public StageMode Mode;
        /// <summary>
        /// Mode0專用圖片置放 
        /// </summary>
        [Header("Mode0請設置好圖片")]
        public Sprite Mode0Sprite;        
        /// <summary>
        /// 正確答案的圖片 , 要求排序
        /// </summary>
        public List<SpriteAns> spriteAns;
        public List<Sprite> ImageOrder ;
        [Serializable]
        public class SpriteAns
        {
            public List<Sprite> Sprite ;
        }
        public enum StageMode
        {
            /// <summary>
            /// 教學關卡
            /// </summary>
            Mode0,
            /// <summary>
            /// 一般關卡 
            /// </summary>
            Mode1,
            /// <summary>
            /// 日期關卡....?
            /// </summary>
            Mode2,
            /// <summary>
            /// 只有一個框框的關卡
            /// </summary>
            Mode3
        }
    }
}
