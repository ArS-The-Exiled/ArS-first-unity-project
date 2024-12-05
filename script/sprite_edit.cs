using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sprite_edit : MonoBehaviour
{
    public SpriteRenderer SR;

    public Sprite l,r,u,d;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    public void Update()
    {
         if(Input.GetKeyDown(KeyCode.DownArrow))
        {
           turn('d');
        }  
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            turn('u');
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            turn('r');
        }      
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            turn('l');
        }  
    }

    // Update is called once per frame
    public void turn(char way)
    { 
        switch(way)
        {
            case 'd': SR.sprite=d;    break;
            case 'u': SR.sprite=u;    break;
            case 'r': SR.sprite=r;    break;
            case 'l': SR.sprite=l;    break;
        }
    }
}
