using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnserTag : MonoBehaviour
{
    public Anser AnserStatus;
    public enum Anser
    {
        /// <summary>
        /// �S����
        /// </summary>
        None ,
        /// <summary>
        /// �w�֦�����
        /// </summary>
        Fill,
        /// <summary>
        /// ���ץ��T
        /// </summary>
        Ans
    }
}
