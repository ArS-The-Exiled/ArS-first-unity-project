using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mapControllNS;
using Unity.VisualScripting;
using Unity.Collections;

public class walk_State : IState
{
    GameObject interactGO=Resources.Load<GameObject>("interact_GO");
    GameObject IGO=null;
    public map_con mapCon;
    public State_Con stateCon;
    tile interact_tile;
    public char way='d';    
    public void Enter()
    {
        Debug.Log("Entering Walking State");
    }

    public walk_State(State_Con _stateCon,map_con _mapCon)
    {
        way='d';
        stateCon=_stateCon;
        mapCon=_mapCon;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            way='d';
            move(0,-1);
        }  
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            way='u';
            move(0,1);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            way='r';
            move(1,0);
        }      
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            way='l';
            move(-1,0);
        }  
        if(Input.GetKeyDown(KeyCode.Z))
        {
            stateCon.ChangeState(stateCon.teleportState);
        }  
        if(Input.GetKeyDown(KeyCode.F))
        {
                switch(way)
                {
                    case 'u':try_to_interact(mapCon.gamerData.x,mapCon.gamerData.y+1); break;
                    case 'd':try_to_interact(mapCon.gamerData.x,mapCon.gamerData.y-1); break;
                    case 'l':try_to_interact(mapCon.gamerData.x-1,mapCon.gamerData.y); break;
                    case 'r':try_to_interact(mapCon.gamerData.x+1,mapCon.gamerData.y); break;
                }
        }    
    }

    void try_to_interact(int x,int y)
    {
        if(!check(x,y)) return;
        if(mapCon.map[y,x].category==2)
        {
            mapCon.complete_the_level();
        }
        if(mapCon.map[y,x].category==4)
        {
            mapCon.map[y,x].Sw.Switch();
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Walking State");
    }

    bool check(int x,int y)
    {
        if(x<0 || x>= mapCon.mapData.width || y<0 || y>= mapCon.mapData.height) return false;
        return true;
    }
    
    public void move(int dx,int dy)
    {
        int x=mapCon.gamerData.x,y=mapCon.gamerData.y;
        int nx=x+dx,ny=y+dy;
        
        
        if(!check(nx,ny))//場外(1類障礙)
        {
            if(IGO!=null)
            {
                Object.Destroy(IGO);
                IGO=null;
            }
            //Debug.Log("Daichan-overflow!!!");
            //Debug.Log($"{nx},{ny},w={mapCon.mapData.width},h={mapCon.mapData.height}");
        }
        else 
        {
            tile start_tile=mapCon.map[y,x],target_tile=mapCon.map[ny,nx];
            if(target_tile.category==4)//switch
            {
                IGO=Object.Instantiate(interactGO,mapCon.get_position(nx,ny),Quaternion.identity);
            }
            else if(IGO!=null)
            {
                Object.Destroy(IGO);
                IGO=null;
            }
            //Debug.Log($"{target_tile.category},{target_tile.isObstacle}");
            if(target_tile.isObstacle==0)
            {
                if(target_tile.category==5)
                {
                    target_tile.botttom.Switch();
                }
                if(start_tile.category==5)
                {
                    start_tile.botttom.Switch();
                }
                mapCon.gamerData.move(dx,dy,mapCon.mapData.unit);
                stateCon.gameObject.transform.position+=new Vector3(dx*mapCon.mapData.unit,dy*mapCon.mapData.unit,0);
                //Debug.Log($"{mapCon.gamerData.x},{mapCon.gamerData.y}");
            }
            else if(target_tile.isObstacle==2)
            {
                if(check(nx+dx,ny+dy))
                {
                     if(mapCon.map[ny+dy,nx+dx].isObstacle==0)
                    {
                        target_tile.GO.transform.position +=new Vector3(dx*mapCon.mapData.unit,dy*mapCon.mapData.unit,0);
                        
                        mapCon.map[ny+dy,nx+dx].move_box(true,target_tile.GO);
                        target_tile.move_box(false);
                
                        mapCon.gamerData.move(dx,dy,mapCon.mapData.unit); 
                        stateCon.gameObject.transform.position+=new Vector3(dx*mapCon.mapData.unit,dy*mapCon.mapData.unit,0);
                    }
                }
            }
        }
    }
}
