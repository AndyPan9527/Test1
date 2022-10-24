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
    /// ��عw������
    /// </summary>
    public GameObject AnsPrefabs;
    /// <summary>
    /// ���ͤ��UI��������
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
    [Header("�s��оǱM��")]
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
    [Header("�ʵe�]�w����")]
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
    /// ����ʵe Scale�ܧ�
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
    /// Ū�����d���
    /// </summary>
    /// <param name="lv">�ǤJ�ĴX�����d</param>
    public void LoadStage(int lv)
    {
        LlevelText.text = $"Level {lv + 1}";
        var stageData = stageDatas[lv];
        NowStageData = stageData;
        for (int i = 0; i < AnserUI.childCount; i++)
        {
            Destroy(AnserUI.GetChild(i).gameObject);
        }

        #region �ͦ��W��UI

        if (NowStageData.Mode == StageData.StageMode.Mode0 || NowStageData.Mode == StageData.StageMode.Mode1)
        {
            for (int i = 0; i < stageData.spriteAns.Count; i++)
            {
                var obj = Instantiate(AnsPrefabs, AnserUI);
                //�p�G�O�s��о�
                if (NowStageData.Mode == StageData.StageMode.Mode0)
                {
                    Debug.Log("�s��о�Ĳ�o");

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
        //�Ȼs�� �Ӧ���������d=.=
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
        //��@�خ����d
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

        #endregion �ͦ��W��UI

        #region �]�w�D�� �P �ѵ���r

        TopicText.text = stageData.Topic;
        TopicText.GetComponent<Animation>().Play("TopicAni");
        AnserText.text = stageData.Answer;        
        AnserText.text =  AnserText.text.Replace("\\n", "\n");
        AnserText.enabled = false;
        ImageGroup.SetActive(true);

        #endregion �]�w�D�� �P �ѵ���r
        //�X�{�i��ܪ��Ϥ�
        for (int i  = 0; i <�@ImageGroup.transform.childCount; i++)
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
        /// �D��
        /// </summary>
        public string Topic;
        /// <summary>
        /// �ѵ�
        /// </summary>
        public string Answer;
        /// <summary>
        /// ���d�Ҧ� ,�Ҧ��ΨӨM�wUI�ͦ�
        /// </summary>
        public StageMode Mode;
        /// <summary>
        /// Mode0�M�ιϤ��m�� 
        /// </summary>
        [Header("Mode0�г]�m�n�Ϥ�")]
        public Sprite Mode0Sprite;        
        /// <summary>
        /// ���T���ת��Ϥ� , �n�D�Ƨ�
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
            /// �о����d
            /// </summary>
            Mode0,
            /// <summary>
            /// �@�����d 
            /// </summary>
            Mode1,
            /// <summary>
            /// ������d....?
            /// </summary>
            Mode2,
            /// <summary>
            /// �u���@�Ӯخت����d
            /// </summary>
            Mode3
        }
    }
}
