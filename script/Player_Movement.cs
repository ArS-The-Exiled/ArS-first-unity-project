using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float v;
    public Rigidbody2D rb;

    private float InX,InY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InX=Input.GetAxisRaw("Horizontal");
        InY=Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(InX*v,InY*v);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            teleport();
        }
    }

    void teleport()
    {

    }
}
