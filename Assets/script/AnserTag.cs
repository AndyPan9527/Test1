using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnserTag : MonoBehaviour
{
    public Anser AnserStatus;
    public enum Anser
    {
        /// <summary>
        /// 沒物件
        /// </summary>
        None ,
        /// <summary>
        /// 已擁有物件
        /// </summary>
        Fill,
        /// <summary>
        /// 答案正確
        /// </summary>
        Ans
    }
}
