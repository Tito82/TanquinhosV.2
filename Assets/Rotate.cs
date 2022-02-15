using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    
    void Start()
    {
        
    }

   
    void Update()
    {
         transform.Rotate (new Vector3 (0, 0.2f, 0));
    }
}
