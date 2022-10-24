using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    public GameObject fillPos , Pos1 , Pos2;
    public Status status;
    List<GameObject> InsObjs = new List<GameObject>();
    bool Ans1Isright , Ans2Isright ; // ���
    public Image UIBackGround1, UIBackGround2; // ���ӷ|���
    public Color Green, Red;
    public ParticleSystem effect1, effect2;
    public Button NextButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    List<GameObject>ClickItem = new List<GameObject>();
    bool ClickBlock = false;
    public void ImgInClick(GameObject obj)
    {
        if (ClickBlock) return;
        ClickItem.Add(obj);
        obj.GetComponent<Image>().enabled = false;
        List<GameObject> BoxList = new List<GameObject>();
        //����Ҧ���ئ�m
        for (int i = 0; i < StageManager.stageManager.AnserUI.transform.childCount; i++)
        {
            var ui_obj = StageManager.stageManager.AnserUI.transform.GetChild(i);
            for (int a = 0; a < ui_obj.transform.childCount; a++)
            {
                var box = ui_obj.transform.GetChild(a).gameObject;
                if (box.tag == "box")
                    BoxList.Add(box);
            }
        }
        //Debug.Log(BoxList.Count);
        //��ت��A��s
        bool checkAns = false ;
        Sprite sprite1 = null;
        Sprite sprite2 = null;
        switch (StageManager.NowStageData.Mode)
        {
            //�ھڼҦ����P �ϥΤ��P���P�_�覡
            case StageManager.StageData.StageMode.Mode0:
                #region �о����d                
                Debug.Log("Click");
                //�K�[����UI
                var Obj_ = Instantiate(obj, BoxList[0].transform);
                Destroy(Obj_.GetComponent<EventTrigger>());
                Obj_.GetComponent<Image>().enabled = true;
                Obj_.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                Obj_.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                Obj_.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                Obj_.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Obj_.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                Obj_.transform.DOShakeScale(0.5f, 0.5f);
                Obj_.name = "ans";
                sprite1 = BoxList[0].transform.Find("ans").GetComponent<Image>().sprite;
                BoxList[0].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Fill;
                sprite2 = BoxList[1].transform.Find("ans").GetComponent<Image>().sprite;
                //Debug.Log("AnsCheck");
                bool correct_ = false;

                InsObjs.Add(Obj_);
                StageManager.NowStageData.spriteAns.ForEach(x =>
                {
                    //���ץ��T
                    if (x.Sprite[0] == sprite1 && x.Sprite[1] == sprite2)
                    {
                        correct_ = true;
                    }
                });
                if (correct_)
                {
                    Debug.Log("���T����");
                    //��sUI�I����
                    BoxList[0].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans;
                    BoxList[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    BoxList[0].transform.GetChild(0).GetComponent<Image>().color = Green;
                    BoxList[1].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans;
                    BoxList[1].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    BoxList[1].transform.GetChild(0).GetComponent<Image>().color = Green;
                    Debug.Log("����U�@��");
                    NextButton.gameObject.SetActive(true);
                    NextButton.onClick.RemoveAllListeners();
                    GetComponent<StageManager>().AnserText.enabled = true;
                    GetComponent<StageManager>().AnserText.GetComponent<Animation>().Play("AnserTextAni");
                    GetComponent<StageManager>().ImageGroup.SetActive(false);
                    effect1.Play();
                    effect2.Play();
                    //�K�[���s�ƥ�
                    NextButton.onClick.AddListener(() =>
                    {
                        ImageSetBack();
                        ClickItem.ForEach(x =>
                        {
                            x.GetComponent<Image>().enabled = true;
                        });
                        ClickItem.Clear();
                        Debug.Log("�K�[���s�ƥ󦨥\");
                        NextButton.gameObject.SetActive(false);
                        if (StageManager.stageLv + 1 >= GetComponent<StageManager>().stageDatas.Count)
                            StageManager.stageLv = -1;
                        GetComponent<StageManager>().LoadStage(StageManager.stageLv += 1);
                        foreach (var p in InsObjs)
                        {
                            Destroy(p.gameObject);
                        }
                        InsObjs.Clear();

                    });
                }
                else
                {

                    Debug.Log("���׿��~");
                    BoxList[0].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
                    BoxList[0].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    BoxList[0].transform.GetChild(0).GetComponent<Image>().color = Red;
                    StartCoroutine(BackGrondBack());
                    IEnumerator BackGrondBack()
                    {
                        ClickBlock = true;
                        yield return new WaitForSeconds(1f);
                        //�p�G�O�s��о�
                        BoxList[0].transform.GetChild(0).GetComponent<Image>().enabled = false;
                        Destroy(InsObjs.Last());
                        InsObjs.Remove(InsObjs.Last());
                        ClickItem.Last().GetComponent<Image>().enabled = true;
                        ClickItem.Remove(ClickItem.Last());
                        ClickBlock = false;
                        //Debug.Log(ClickBlock);

                    }
                }
                #endregion �о����d
                break;
            case StageManager.StageData.StageMode.Mode1:
                #region �@�����d
                for (int i = 0; i < BoxList.Count; i++)
                {
                    var status = BoxList[i].GetComponent<AnserTag>();
                    if (status.AnserStatus == AnserTag.Anser.Fill)
                    {
                        checkAns = true;
                        sprite1 = BoxList[i].transform.Find("ans").GetComponent<Image>().sprite;
                    }
                    else if (status.AnserStatus == AnserTag.Anser.None)
                    {
                        //�K�[����UI
                        var InsObj = Instantiate(obj, BoxList[i].transform);
                        Destroy(InsObj.GetComponent<EventTrigger>());
                        InsObj.GetComponent<Image>().enabled = true;
                        InsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                        InsObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        InsObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        InsObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        InsObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        InsObj.transform.DOShakeScale(0.5f, 0.5f);
                        InsObj.name = "ans";
                        sprite2 = BoxList[i].transform.Find("ans").GetComponent<Image>().sprite;
                        status.AnserStatus = AnserTag.Anser.Fill;

                        InsObjs.Add(InsObj);
                        Debug.Log(checkAns);
                        //�O�_�i�浪�קP�_
                        if (checkAns)
                        {
                            bool correct = false;
                            StageManager.NowStageData.spriteAns.ForEach(x =>
                            {
                                //���ץ��T
                                if (x.Sprite[0] == sprite1 && x.Sprite[1] == sprite2)
                                {
                                    correct = true;
                                }
                                //���׿��~
                                else
                                {

                                }
                            });
                            //����O�_�i��U�@���P�_
                            if (correct)
                            {
                                Debug.Log("���T����");
                                //��sUI�I����
                                BoxList[i].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans;
                                BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                                BoxList[i].transform.GetChild(0).GetComponent<Image>().color = Green;
                                BoxList[i - 1].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans;
                                BoxList[i - 1].transform.GetChild(0).GetComponent<Image>().enabled = true;
                                BoxList[i - 1].transform.GetChild(0).GetComponent<Image>().color = Green;
                                //�O�_�i��U�@���P�_
                                bool Next = true;
                                BoxList.ForEach(x =>
                                {
                                    if (x.GetComponent<AnserTag>().AnserStatus != AnserTag.Anser.Ans)
                                        Next = false;
                                });
                                //���׳����� �i��U�@�� 
                                if (Next)
                                {
                                    Debug.Log("����U�@��");
                                    NextButton.gameObject.SetActive(true);
                                    NextButton.onClick.RemoveAllListeners();
                                    GetComponent<StageManager>().AnserText.enabled = true;
                                    GetComponent<StageManager>().AnserText.GetComponent<Animation>().Play("AnserTextAni");
                                    GetComponent<StageManager>().ImageGroup.SetActive(false);
                                    effect1.Play();
                                    effect2.Play();
                                    //�K�[���s�ƥ�
                                    NextButton.onClick.AddListener(() =>
                                    {
                                        ImageSetBack();
                                        ClickItem.ForEach(x =>
                                        {
                                            x.GetComponent<Image>().enabled = true;
                                        });
                                        ClickItem.Clear();
                                        Debug.Log("�K�[���s�ƥ󦨥\");
                                        NextButton.gameObject.SetActive(false);
                                        if (StageManager.stageLv + 1 >= GetComponent<StageManager>().stageDatas.Count)
                                            StageManager.stageLv = -1;
                                        GetComponent<StageManager>().LoadStage(StageManager.stageLv += 1);
                                        foreach (var p in InsObjs)
                                        {
                                            Destroy(p.gameObject);
                                        }
                                        InsObjs.Clear();

                                    });
                                }
                            }
                            else
                            {
                                Debug.Log("���׿��~");


                                BoxList[i].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
                                BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                                BoxList[i].transform.GetChild(0).GetComponent<Image>().color = Red;
                                BoxList[i - 1].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
                                BoxList[i - 1].transform.GetChild(0).GetComponent<Image>().enabled = true;
                                BoxList[i - 1].transform.GetChild(0).GetComponent<Image>().color = Red;


                                StartCoroutine(BackGrondBack());
                                IEnumerator BackGrondBack()
                                {
                                    ClickBlock = true;
                                    yield return new WaitForSeconds(1f);
                                    //�p�G�O�s��о�

                                    BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                                    BoxList[i - 1].transform.GetChild(0).GetComponent<Image>().enabled = false;
                                    Destroy(InsObjs.Last());
                                    Destroy(InsObjs[InsObjs.Count - 2]);
                                    InsObjs.Remove(InsObjs.Last());
                                    InsObjs.Remove(InsObjs.Last());
                                    //Debug.Log("4");
                                    ClickItem.Last().GetComponent<Image>().enabled = true;
                                    ClickItem[ClickItem.Count - 2].GetComponent<Image>().enabled = true;
                                    ClickItem.RemoveAt(ClickItem.Count - 1);
                                    ClickItem.RemoveAt(ClickItem.Count - 1);
                                    ClickBlock = false;

                                }
                            }
                        }
                        break;
                    }


                }
                #endregion �@�����d   
                break;
            case StageManager.StageData.StageMode.Mode2:
                #region ������d Damnnnnnnnn
                //����ثe���שҦ��Ϥ�
                List<Sprite> sprites = new List<Sprite>();
                for (int i = 0; i < BoxList.Count; i++)
                {
                    
                    var status = BoxList[i].GetComponent<AnserTag>();                    
                    if (status.AnserStatus == AnserTag.Anser.None)
                    {
                        //�K�[����UI
                        var InsObj = Instantiate(obj, BoxList[i].transform);
                        Destroy(InsObj.GetComponent<EventTrigger>());
                        InsObj.GetComponent<Image>().enabled = true;
                        InsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                        InsObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        InsObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        InsObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        InsObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        InsObj.transform.DOShakeScale(0.5f, 0.5f);
                        InsObj.name = "ans";
                        sprite2 = BoxList[i].transform.Find("ans").GetComponent<Image>().sprite;
                        status.AnserStatus = AnserTag.Anser.Fill;
                        InsObjs.Add(InsObj);                        
                        break;                        
                    }
                }
                for (int i = 0; i < BoxList.Count; i++)
                {
                    var status = BoxList[i].GetComponent<AnserTag>();
                    if (status.AnserStatus == AnserTag.Anser.Fill)
                        sprites.Add(BoxList[i].transform.Find("ans").GetComponent<Image>().sprite);
                }
                //���楿�T���קP�_
                if(sprites.Count == StageManager.NowStageData.spriteAns[0].Sprite.Count)
                {
                    Debug.Log("�����ˬd");
                    bool correct = true;
                    for (int i  =0; i < StageManager.NowStageData.spriteAns[0].Sprite.Count; i++)
                    {
                        var AnsSprite = StageManager.NowStageData.spriteAns[0].Sprite[i];
                        if (sprites[i] != AnsSprite)
                        {
                            correct = false;
                        }
                    }
                    if (correct)
                    {
                        Debug.Log("���T");
                        //��sUI�I����
                        for (int i = 0; i < BoxList.Count ; i++)
                        {
                            BoxList[i].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().color = Green;
                        }
                        Debug.Log("����U�@��");
                        NextButton.gameObject.SetActive(true);
                        NextButton.onClick.RemoveAllListeners();
                        GetComponent<StageManager>().AnserText.enabled = true;
                        GetComponent<StageManager>().AnserText.GetComponent<Animation>().Play("AnserTextAni");
                        GetComponent<StageManager>().ImageGroup.SetActive(false);
                        effect1.Play();
                        effect2.Play();
                        ClickBlock = true;
                        //�K�[���s�ƥ�
                        NextButton.onClick.AddListener(() =>
                        {
                            ClickBlock = false;
                            ImageSetBack();
                            ClickItem.ForEach(x =>
                            {
                                x.GetComponent<Image>().enabled = true;
                            });
                            ClickItem.Clear();
                            Debug.Log("�K�[���s�ƥ󦨥\");
                            NextButton.gameObject.SetActive(false);
                            if (StageManager.stageLv + 1 >= GetComponent<StageManager>().stageDatas.Count)
                                StageManager.stageLv = -1;
                            GetComponent<StageManager>().LoadStage(StageManager.stageLv += 1);
                            foreach (var p in InsObjs)
                            {
                                Destroy(p.gameObject);
                            }
                            InsObjs.Clear();

                        });
                    }
                    else
                    {
                        Debug.Log("�����T");
                        for (int i = 0; i < BoxList.Count -1; i++)
                        {
                            
                            BoxList[i].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().color = Red;
                            
                        }

                        StartCoroutine(BackGrondBack());
                        IEnumerator BackGrondBack()
                        {
                            ClickBlock = true;
                            yield return new WaitForSeconds(1f);
                            
                            for (int i = 0; i < BoxList.Count-1; i++)
                            {
                                BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                                Destroy(InsObjs.Last());
                                InsObjs.Remove(InsObjs.Last());
                                ClickItem.Last().GetComponent<Image>().enabled = true;
                                ClickItem.Remove(ClickItem.Last());
                                ClickBlock = false;
                            }

                        }
                    }
                }
                //Debug.Log(sprites.Count);
                #endregion ������d Damnnnnnnnn
                break;
            case StageManager.StageData.StageMode.Mode3:
                #region �u���@�Ӯخت����d
                //����ثe���שҦ��Ϥ�
                List<Sprite> sprites3 = new List<Sprite>();
                for (int i = 0; i < BoxList.Count; i++)
                {

                    var status = BoxList[i].GetComponent<AnserTag>();
                    if (status.AnserStatus == AnserTag.Anser.None)
                    {
                        //�K�[����UI
                        var InsObj = Instantiate(obj, BoxList[i].transform);
                        Destroy(InsObj.GetComponent<EventTrigger>());
                        InsObj.GetComponent<Image>().enabled = true;
                        InsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                        InsObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        InsObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                        InsObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        InsObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                        InsObj.transform.DOShakeScale(0.5f, 0.5f);
                        InsObj.name = "ans";
                        sprite2 = BoxList[i].transform.Find("ans").GetComponent<Image>().sprite;
                        status.AnserStatus = AnserTag.Anser.Fill;
                        InsObjs.Add(InsObj);
                        break;
                    }
                }
                for (int i = 0; i < BoxList.Count; i++)
                {
                    var status = BoxList[i].GetComponent<AnserTag>();
                    if (status.AnserStatus == AnserTag.Anser.Fill)
                        sprites3.Add(BoxList[i].transform.Find("ans").GetComponent<Image>().sprite);
                }
                //���楿�T���קP�_
                if (sprites3.Count == StageManager.NowStageData.spriteAns[0].Sprite.Count)
                {
                    Debug.Log("�����ˬd");
                    bool correct = true;
                    for (int i = 0; i < StageManager.NowStageData.spriteAns[0].Sprite.Count; i++)
                    {
                        var AnsSprite = StageManager.NowStageData.spriteAns[0].Sprite[i];
                        if (sprites3[i] != AnsSprite)
                        {
                            correct = false;
                        }
                    }
                    if (correct)
                    {
                        Debug.Log("���T");
                        //��sUI�I����
                        for (int i = 0; i < BoxList.Count; i++)
                        {
                            BoxList[i].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.Ans;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().color = Green;
                        }
                        Debug.Log("����U�@��");
                        NextButton.gameObject.SetActive(true);
                        NextButton.onClick.RemoveAllListeners();
                        GetComponent<StageManager>().AnserText.enabled = true;
                        GetComponent<StageManager>().AnserText.GetComponent<Animation>().Play("AnserTextAni");
                        GetComponent<StageManager>().ImageGroup.SetActive(false);
                        effect1.Play();
                        effect2.Play();
                        //�K�[���s�ƥ�
                        NextButton.onClick.AddListener(() =>
                        {
                            ImageSetBack();
                            ClickItem.ForEach(x =>
                            {
                                x.GetComponent<Image>().enabled = true;
                            });
                            ClickItem.Clear();
                            Debug.Log("�K�[���s�ƥ󦨥\");
                            NextButton.gameObject.SetActive(false);
                            if (StageManager.stageLv + 1 >= GetComponent<StageManager>().stageDatas.Count)
                                StageManager.stageLv = -1;
                            GetComponent<StageManager>().LoadStage(StageManager.stageLv += 1);
                            foreach (var p in InsObjs)
                            {
                                Destroy(p.gameObject);
                            }
                            InsObjs.Clear();

                        });
                    }
                    else
                    {
                        Debug.Log("�����T");
                        for (int i = 0; i < BoxList.Count ; i++)
                        {

                            BoxList[i].GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                            BoxList[i].transform.GetChild(0).GetComponent<Image>().color = Red;

                        }

                        StartCoroutine(BackGrondBack());
                        IEnumerator BackGrondBack()
                        {
                            ClickBlock = true;
                            yield return new WaitForSeconds(1f);

                            for (int i = 0; i < BoxList.Count ; i++)
                            {
                                BoxList[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                                Destroy(InsObjs.Last());
                                InsObjs.Remove(InsObjs.Last());
                                ClickItem.Last().GetComponent<Image>().enabled = true;
                                ClickItem.Remove(ClickItem.Last());
                                ClickBlock = false;
                            }

                        }
                    }
                }
                //Debug.Log(sprites.Count);
                #endregion �u���@�Ӯخت����d
                break;
        }         
        return;

        #region xxx
        /*
        if (status == Status.None)
        {
            var InsObj = Instantiate(obj, Pos1.transform);
            InsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            InsObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            InsObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            InsObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            status = Status.Item1;
            InsObjs.Add(InsObj);
            if (obj.GetComponent<AnserTag>().AnserStatus == AnserTag.Anser.Ans1)
                Ans1Isright = true;
        }
        else if (status == Status.Item1)
        {
            var InsObj = Instantiate(obj, Pos2.transform);
            InsObj.transform.position = Pos2.transform.position;
            InsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            InsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            InsObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            InsObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            InsObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            status = Status.Item2;
            InsObjs.Add(InsObj);
            if (obj.GetComponent<AnserTag>().AnserStatus == AnserTag.Anser.Ans2)
                Ans2Isright = true;

            //�ˬd�O�_���T
            //���T�ѵ�
            if (Ans1Isright && Ans2Isright)
            {
                UIBackGround1.enabled = true;
                UIBackGround2.enabled = true;
                UIBackGround1.color = Green;
                UIBackGround2.color = Green;
                GetComponent<StageManager>().AnserText.enabled = true;
                GetComponent<StageManager>().ImageGroup.SetActive(false);                
                effect1.Play();
                effect2.Play();
                Debug.Log("���T�ѵ�");
                //�U�@�����s�ƥ�K�[
                NextButton.gameObject.SetActive(true);                    
                NextButton.onClick.RemoveAllListeners();
                //�K�[���s�ƥ�
                NextButton.onClick.AddListener(() => {
                    ImageSetBack();
                    Debug.Log("�K�[���s�ƥ󦨥\");
                    NextButton.gameObject.SetActive(false);
                    if (StageManager.stageLv + 1 >= GetComponent<StageManager>().stageDatas.Count)
                        StageManager.stageLv = -1;
                    GetComponent<StageManager>().LoadStage(StageManager.stageLv += 1);
                    foreach (var p in InsObjs)
                    {
                        Destroy(p.gameObject);
                    }
                    InsObjs.Clear();
                    status = Status.None;
                    UIBackGround1.enabled = false;
                    UIBackGround2.enabled = false;
                    Ans1Isright = false;
                    Ans2Isright = false;
                });
            }
            //���~����
            else
            {

                UIBackGround1.enabled = true;
                UIBackGround2.enabled = true;
                UIBackGround1.color = Red;
                UIBackGround2.color = Red;
                StartCoroutine(enumerator());

                IEnumerator enumerator (){
                    yield return new WaitForSeconds(1);
                    foreach (var p in InsObjs)
                    {
                        Destroy(p.gameObject);
                    }
                    ImageSetBack();
                    InsObjs.Clear();
                    status = Status.None;
                    UIBackGround1.enabled = false;
                    UIBackGround2.enabled = false;
                }

                
            }
        }
        obj.GetComponent<Image>().enabled = false;
        */
        #endregion

    }
    /// <summary>
    /// �����ĪG��_
    /// </summary>
    void ImageSetBack()
    {
        for (int i = 0; i < StageManager.stageManager.ImageGroup.transform.childCount; i++)
        {
            StageManager.stageManager.ImageGroup.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
        }
    }
    public void ReplyClick()
    {
        //�p�G�O�s��о� �T�Ϋ�_���s
        if (StageManager.NowStageData.Mode == StageManager.StageData.StageMode.Mode0) return;
        //�^�_��m�i�椤 , �T�Ϋ�_���s
        if (ClickBlock) return;

        if (StageManager.NowStageData.Mode == StageManager.StageData.StageMode.Mode1)
        {

            List<GameObject> BoxList = new List<GameObject>();
            //����Ҧ���ئ�m
            for (int i = 0; i < StageManager.stageManager.AnserUI.transform.childCount; i++)
            {
                var ui_obj = StageManager.stageManager.AnserUI.transform.GetChild(i);
                for (int a = 0; a < ui_obj.transform.childCount; a++)
                {
                    var box = ui_obj.transform.GetChild(a).gameObject;
                    if (box.tag == "box")
                        BoxList.Add(box);
                }
            }
            //����^�_�\��
            for (int i = 0; i < BoxList.Count; i++)
            {
                var status = BoxList[i].GetComponent<AnserTag>();
                if (status.AnserStatus == AnserTag.Anser.Fill)
                {
                    status.AnserStatus = AnserTag.Anser.None;
                    Destroy(BoxList[i].transform.Find("ans").gameObject);
                    ClickItem.Last().GetComponent<Image>().enabled = true;
                    ClickItem.Remove(ClickItem.Last());
                }
            }

        }
        else if (StageManager.NowStageData.Mode == StageManager.StageData.StageMode.Mode2)
        {
            if (InsObjs.Count == 0) return;
            InsObjs.Last().transform.parent.GetComponent<AnserTag>().AnserStatus = AnserTag.Anser.None;
            Destroy(InsObjs.Last().gameObject);
            InsObjs.Remove(InsObjs.Last());
            ClickItem.Last().GetComponent<Image>().enabled = true;
            ClickItem.Remove(ClickItem.Last());
        }
            return;
        if (status == Status.Item2) return;
        foreach (var p in InsObjs)
        {
            Destroy(p.gameObject);
        }
        InsObjs.Clear();
        status = Status.None;
        UIBackGround1.enabled = false;
        UIBackGround2.enabled = false;
        ImageSetBack();
    }
    public void ScaleBack(GameObject obj)
    {
        obj.transform.localScale = Vector3.one;
    }
    public void ScaleSmall(GameObject obj)
    {
        obj.transform.localScale = Vector3.one * 0.8f;
    }
    public enum Status
    {
        None , Item1,Item2
    }
}
