using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StageManager.stageManager.ScaleAni(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
