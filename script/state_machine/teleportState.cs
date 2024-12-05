using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mapControllNS;
using Unity.VisualScripting;
using Unity.Properties;
using System.Data.SqlTypes;

public class teleport_State : IState
{
    public State_Con stateCon;
    public map_con mapCon;

    private List<GameObject> teleObjects = new List<GameObject>(); // 儲存範圍物件
    private List<GameObject> CirObjects = new List<GameObject>(); // 儲存範圍物件
    public GameObject circle;
    public GameObject can_TP;
    public GameObject cant_TP;

    int Px0,Py0;
    public teleport_State(State_Con _stateCon,map_con _mapCon,GameObject _circle,GameObject _can,GameObject _cant)
    {
        stateCon=_stateCon;
        mapCon=_mapCon;
        circle=_circle;
        can_TP=_can;
        cant_TP=_cant;
    }
    public void Enter()
    {
        Px0=-1;
        Py0=-1;
        Debug.Log("Entering Walking State");
        Time.timeScale = 0;
        GameObject cir= Object.Instantiate(circle , mapCon.get_position(mapCon.gamerData.x,mapCon.gamerData.y) , Quaternion.identity);
        CirObjects.Add(cir);
    }
   
    public int SetP(float n)
    {
        if(n<0) return -1;
        else return (int)n/(int)mapCon.mapData.unit;
    }

    public void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
    
        // 設定 mousePosition.z 為相機與物體的 Z 距離 (在這裡是 10)
        mousePosition.z = 10f;  // 這是從相機到物體的 Z 距離

        // 把螢幕座標轉換為世界座標
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        int Px,Py;

        Px=SetP(worldPosition.x);
        Py=SetP(worldPosition.y);
        
        if((Px!=Px0 || Py!=Py0) && check(Px,Py))
        {
            Px0=Px;
            Py0=Py;
            teleport_set_single(Px0,Py0,Px,Py);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log($"{Px},{Py}");
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            stateCon.ChangeState(stateCon.walkState);
        }//cancel teleport 
        if(Input.GetKeyDown(KeyCode.X))
        {
            teleport_set();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(check(Px,Py) && mapCon.map[Py,Px].isObstacle==0)
            {
                teleport(Px,Py);
                stateCon.ChangeState(stateCon.walkState);
            }
        }
    }
    public bool check(int x,int y)
    {
        return x>=0 && x<mapCon.mapData.width && y>=0 && y<mapCon.mapData.height &&  Mathf.Abs(mapCon.gamerData.x-x)<=2 &&  Mathf.Abs(mapCon.gamerData.y-y)<=2 ;
    }
    public void teleport_set_single(int x0,int y0,int x,int y)
    {
        if(x0!=-1)
        {
            foreach(GameObject obj in teleObjects)
            {
                Object.Destroy(obj);
            }
            teleObjects.Clear();
        }
        if(mapCon.map[y,x].isObstacle!=0)
        {
            GameObject mark= Object.Instantiate(cant_TP,mapCon.get_position(x,y),Quaternion.identity);
            teleObjects.Add(mark);
        }  
        else
        {
            GameObject mark= Object.Instantiate(can_TP,mapCon.get_position(x,y),Quaternion.identity);
            teleObjects.Add(mark);
        }     
    }
    public void teleport_set()
    {
        int gx=mapCon.gamerData.x;
        int gy=mapCon.gamerData.y;

        for(int i=-2 ; i<=2 ; i++)
        {
            for(int j=-2 ; j<=2 ; j++)
            {
                if(check(gx+j,gy+i))
                {
                    if(mapCon.map[gy+i,gx+j].isObstacle!=0)
                    {
                        GameObject mark= Object.Instantiate(cant_TP,mapCon.get_position(gx+j,gy+i),Quaternion.identity);
                        teleObjects.Add(mark);
                    }  
                    else
                    {
                        GameObject mark= Object.Instantiate(can_TP,mapCon.get_position(gx+j,gy+i),Quaternion.identity);
                        teleObjects.Add(mark);
                    }     
                }
            }
        }
    }
    public void teleport(int nx, int ny)
    {
        mapCon.gamerData.move( nx-mapCon.gamerData.x, ny-mapCon.gamerData.y,mapCon.mapData.unit);
        stateCon.gameObject.transform.position=mapCon.get_position(nx,ny);
    }

    public void Exit()
    {
        foreach(GameObject obj in teleObjects)
        {
            Object.Destroy(obj);
        }
        teleObjects.Clear();
        foreach(GameObject obj in CirObjects)
        {
            Object.Destroy(obj);
        }
        CirObjects.Clear();

        Time.timeScale=1;
        Debug.Log("Exiting teleport State");
    }
}
