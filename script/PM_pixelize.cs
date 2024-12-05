using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using mapControllNS;
using Unity.VisualScripting;
using System.Net.NetworkInformation;

public class PM_pixelize : MonoBehaviour
{ 
    public map_con map_Con;

    int x,y;


    // Start is called before the first frame update
    void Start()
    {
        map_Con=FindObjectOfType<map_con>(); 
        Debug.Log($"{map_Con.gamerData.x},{map_Con.gamerData.y},{map_Con.gamerData.xt},{map_Con.gamerData.yt}");  
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
           move(0,-1);
        }  
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            move(0,1);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(1,0);
        }      
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(-1,0);
        }      
        if(Input.GetKeyDown(KeyCode.Z))
        {
            teleport();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            interact();
        }
    }
    void move(int dx,int dy)
    {
        int nx=map_Con.gamerData.x+dx;
        int ny=map_Con.gamerData.y+dy;
        
        if(nx<0 || nx>= map_Con.mapData.width || ny<0 || ny>= map_Con.mapData.height)//場外(1類障礙)
        {
            Debug.Log("Daichan-overflow!!!");
            Debug.Log($"{nx},{ny},w={map_Con.mapData.width},h={map_Con.mapData.height}");
        }
        else if(map_Con.map[ny,nx].isObstacle == 0)//無障礙(場內)(2類障礙)
        {
            move_sucess(dx,dy);
        }
        else if(map_Con.map[ny,nx].isObstacle == 2)
        {
            //how to connect map[y,x] with gameObject?
            move_sucess(dx,dy);
        }
    }
    void move_sucess(int dx,int dy)
    {
        map_Con.gamerData.move( dx, dy,map_Con.mapData.unit);
        transform.position+=new Vector3(dx*map_Con.mapData.unit,dy*map_Con.mapData.unit,0);
    }

    void interact()
    {
        if(map_Con.map[y,x].category==2)//door
        {
            Debug.Log("Complete the level");
            map_Con.complete_the_level();
        }
    }

    void teleport()
    {
        
    }
}
