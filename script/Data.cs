
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

namespace mapControllNS
{
    [System.Serializable]
    public class tile
    {
        public map_con mapCon;
        public int x;
        public int y;
        public int category;
        public int isObstacle;//0 no 1 yes 2 box
        public int interactable=0;
        public GameObject GO=null;
        public door Door;
        public SwitchCl Sw;
        public botttomCl botttom;
        public tile(map_con _mapCon,int _x,int _y,int _category,GameObject _GO=null)
        {
            mapCon=_mapCon;
            x=_x;
            y=_y;
            category=_category;
            
            if(category==0)//floor
            {
                isObstacle=0;
            }
            if(category==1)//wall
            {
                isObstacle=1;
            }
            if(category==2)//goal
            {
                interactable=1;
                isObstacle=0;
            }
            if(category==3)//door
            {
            }
            if(category==4)//switch
            {
                interactable=1;
                isObstacle=1;
            }
            if(category==5)//bottom
            {
                isObstacle=0;
            }
            if(category==10)//box
            {
                isObstacle=2;
                GO=Object.Instantiate(_GO,mapCon.get_position(x,y),Quaternion.identity);
            }
            //Debug.Log($"x:{x},y:{y},cat:{category}");
        }

        public void setDoor(door _Door)
        {
            Door=_Door;
            isObstacle=Door.status;
        }
        public void setSwitch(SwitchCl _Sw)
        {
            Sw=_Sw;
        }
        public void setBottom(botttomCl _Bottom)
        {
            botttom=_Bottom;
        }

        public void move_box(bool tar,GameObject _GO=null)
        {
            if(!tar)//out
            {
                category=0;
                isObstacle=0;
                GO=null;
            }
            else//in
            {
                GO=_GO;
                category=10;
                isObstacle=2;
            }
        }
    }

    [System.Serializable]
    public class tile0
    {   
        public int x;
        public int y;
        public int category;
        public int group;
        public int status;
    }

    [System.Serializable]
    public class door
    {
        public tile myTile;
        public int group;
        public int status;  
        GameObject doorGO=null;
        
        public door(tile _myTile,int _group,int _status,GameObject _GO)
        {
            //Debug for no reference
            if(_myTile==null)
            {
                Debug.LogError("no tile!");
                return;
            }
            if(_myTile.mapCon==null)
            {
                Debug.LogError("no map_con!");
                return;
            }
//            Debug.LogWarning($"{_group},{_status}");
            
            myTile=_myTile;
            group=_group;
            status=_status;
            doorGO=Object.Instantiate(_GO, myTile.mapCon.get_position(myTile.x,myTile.y),Quaternion.identity);
            
            set();
        }

        public void check()
        {
            if(status==1 && myTile.isObstacle==0)//door
            { 
                myTile.isObstacle=1;
                set();
            }
        }
        
        public void set()
        {
            // 根據 status 改變圖案
            if (status == 0)
            {
                 // 開門圖案 (假設有開門材質)
                doorGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Door_Open");
            }
            else
            {
                // 關門圖案 (假設有關門材質)
                doorGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Door_Close");
            }
           
            // 根據 group 改變顏色
            Color groupColor = Color.white; // 預設顏色
            switch (group)
            {
                case 1: groupColor = Color.green; break;
                case 2: groupColor = Color.cyan; break;
                case 3: groupColor = Color.yellow; break;
                // 可依需求新增更多顏色
            }
            doorGO.GetComponent<Renderer>().material.color = groupColor;         
        }

        public void Switch()
        {
            if(status==1)   
            {
                status=0;
            }
            else
            {
                status=1;
            }
//            Debug.LogWarning($"{status}");
  //          Debug.LogWarning($"{myTile.x},{myTile.y}");
            if(myTile.mapCon.gamerData.x==myTile.x && myTile.mapCon.gamerData.y==myTile.y)  
            {
                return;
            }
            myTile.isObstacle=status;
//            Debug.LogWarning($"{myTile.isObstacle}");
            set();
        }
    }

    [System.Serializable]
    public class SwitchCl
    {
        public tile myTile;
        public int group;
        public int status;  
        GameObject switchGO=null;
        
        public SwitchCl(tile _myTile,int _group,int _status,GameObject _GO)
        {
            //Debug for no reference
            if(_myTile==null)
            {
                Debug.LogError("no tile!");
                return;
            }
            if(_myTile.mapCon==null)
            {
                Debug.LogError("no map_con!");
                return;
            }
//            Debug.LogWarning($"{_group},{_status}");
            myTile=_myTile;
            group=_group;
            status=_status;
            switchGO=Object.Instantiate(_GO, myTile.mapCon.get_position(myTile.x,myTile.y),Quaternion.identity);
            set();
        }
        public void check()
        {
        }
        public void set()
        {
            // 根據 status 改變圖案
            if (status == 0)
            {
                // 開門圖案 (假設有開門材質)
                switchGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Switch_Open");
            }
            else
            {
                // 關門圖案 (假設有關門材質)
                switchGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Switch_Close");
            }
           
            // 根據 group 改變顏色
            Color groupColor = Color.white; // 預設顏色
            switch (group)
            {
                case 1: groupColor = Color.green; break;
                case 2: groupColor = Color.cyan; break;
                case 3: groupColor = Color.yellow; break;
                // 可依需求新增更多顏色
            }
            switchGO.GetComponent<Renderer>().material.color = groupColor;         
        }
        public void Switch()
        {
            myTile.mapCon.DoorActive(group);
            if(status==1)   
            {
                status=0;
            }
            else
            {
                status=1;
            }
            set();
        }
    }

     [System.Serializable]
    public class botttomCl
    {
        public tile myTile;
        public int group;  
        GameObject bottomGO=null;

        int status=0;
        
        public botttomCl(tile _myTile,int _group,GameObject _GO)
        {
            //Debug for no reference
            if(_myTile==null)
            {
                Debug.LogError("no tile!");
                return;
            }
            if(_myTile.mapCon==null)
            {
                Debug.LogError("no map_con!");
                return;
            }
//            Debug.LogWarning($"{_group},{_status}");
            myTile=_myTile;
            group=_group;
            bottomGO=Object.Instantiate(_GO, myTile.mapCon.get_position(myTile.x,myTile.y),Quaternion.identity);
            set();
        }
        public void check()
        {
        }
        public void set()
        {
            // 根據 status 改變圖案
            if (status == 0)
            {
                // 開門圖案 (假設有開門材質)
                bottomGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Switch_Open");
            }
            else
            {
                // 關門圖案 (假設有關門材質)
                bottomGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Switch_Close");
            }
           
            // 根據 group 改變顏色
            Color groupColor = Color.white; // 預設顏色
            switch (group)
            {
                case 1: groupColor = Color.green; break;
                case 2: groupColor = Color.cyan; break;
                case 3: groupColor = Color.yellow; break;
                // 可依需求新增更多顏色
            }
            bottomGO.GetComponent<Renderer>().material.color = groupColor;         
        }
        public void Switch()
        {
            myTile.mapCon.DoorActive(group);
            if(status==1)   
            {
                status=0;
            }
            else
            {
                status=1;
            }
            set();
        }
    }

    [System.Serializable]
    public class MapData
    {
        public int width;
        public int height;
        public float unit;
        public int xi;
        public int yi; 
        public tile0[] category_i ;
    }

    [System.Serializable]
    public class GamerData
    {
        public map_con mapCon;
        public int x;
        public int y;
        public float xt;
        public float yt;
        public int level;
        public int status;
        public int way;//1234uldr    
        public GamerData(map_con _mapCon,MapData mapData)
        {
            mapCon=_mapCon;
            x=mapData.xi;
            y=mapData.yi;
            xt=mapData.xi*mapData.unit+0.5f;
            yt=mapData.yi*mapData.unit+1;
            status=0;
        }

        public void set(int dx,int dy,float unit)
        {
            check_door();
            x=dx;
            y=dy;
            xt=dx*unit+0.5f;
            yt=dy*unit+1;

        }

        public void move(int dx,int dy,float unit)
        {
            check_door();
            //way=
            x+=dx;
            y+=dy;
            xt+=dx*unit;
            yt+=dy*unit;
        }

        public void check_door()
        {
            if(mapCon.map[y,x].category==3)
            {
                mapCon.map[y,x].Door.check();
            }
        }
    }
}