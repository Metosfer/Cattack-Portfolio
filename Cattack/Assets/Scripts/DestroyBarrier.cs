using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrier : MonoBehaviour
{
    Barrier barrier;
    
    // Start is called before the first frame update
    void Start()
    {
        barrier = GetComponent<Barrier>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
