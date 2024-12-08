using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class invisible_when_awake : MonoBehaviour
{

    void Awake()
    {
        TilemapRenderer renderer = GetComponent<TilemapRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
