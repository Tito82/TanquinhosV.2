using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

   public static AudioScript inst;

void Awake()
{
    if (AudioScript.inst == null)
    {
        AudioScript.inst = this;
        DontDestroyOnLoad(gameObject);
        
    }else{
        Destroy(gameObject);

    }
}

 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
