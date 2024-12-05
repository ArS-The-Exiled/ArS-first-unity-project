using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mapControllNS;
using UnityEngine.SceneManagement; 


public class map_con : MonoBehaviour
{
    public MapData mapData = new MapData();
    public GameObject gamer;
    public GameObject box;
    public GameObject doorPrefab;
    public GameObject switchPrefab;
    public GameObject bottomPrefab;
    public GamerData gamerData;
    public tile[,] map;
    public int level=1;

    public Dictionary<int, List<door>> doorGroups = new Dictionary<int, List<door>>();

   

 
    // Start is called before the first frame update

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //禁止在切換場景時摧毀此物件
    }
    void Start()
    {
        initialize_map();
    }

    public Vector3 get_position(int x,int y)
    {
        float nx = x*mapData.unit + 0.5f;
        float ny = y*mapData.unit + 0.5f;
        Vector3 pos = new Vector3(nx,ny,0);
        return pos;
    }

    void initialize_map()
    {
       
        string filePath = Path.Combine(Application.streamingAssetsPath, $"Level{level}.json");
        Debug.Log($"{filePath}");

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            mapData = JsonUtility.FromJson<MapData>(jsonContent);
        }  
        else
        {
            Debug.Log("no file");
        } 

        int h=mapData.height,w=mapData.width;

        map=new tile[h,w];
        //Debug.Log($"{h},{w}");
        for(int i=0;i<h;i++)
        {
            for(int j=0;j<w;j++)
            {
                int k=i*w+j;
                int category=mapData.category_i[k].category;
                if(category==3)
                {
                    map[i,j]=new tile(this,j,i,category);
                   
                    door Door=new door(map[i,j],mapData.category_i[k].group,mapData.category_i[k].status,doorPrefab);
                    
                    map[i,j].setDoor(Door); 
                    
                    if(!doorGroups.ContainsKey(Door.group))
                    {
                        doorGroups[Door.group]=new List<door>();
                       // Debug.LogWarning($"creat dict{Door.group}");
                    }
                    doorGroups[Door.group].Add(Door);
                    //Debug.LogWarning($"add {Door.myTile.x},{Door.myTile.y} to {Door.group}");
                }
                else  if(category==4)
                {
                    map[i,j]=new tile(this,j,i,category);
                   
                    SwitchCl sw=new SwitchCl(map[i,j],mapData.category_i[k].group,mapData.category_i[k].status,switchPrefab);
                    
                    map[i,j].setSwitch(sw); 
                    //好像不用遍歷
                    /*
                    if(!switchGroups.ContainsKey(sw.group))
                    {
                        switchGroups[sw.group]=new List<SwitchCl>();
                       // Debug.LogWarning($"creat dict{Door.group}");
                    }
                    switchGroups[sw.group].Add(sw);
                    */
                }
                else  if(category==4)
                {
                    map[i,j]=new tile(this,j,i,category);
                   
                    botttomCl bottom=new botttomCl(map[i,j],mapData.category_i[k].group,bottomPrefab);
                    
                    map[i,j].setBottom(bottom); 
                }
                else if(category==10)
                {
                    map[i,j]=new tile(this,j,i,category,box); 
                }//Debug.Log($"Tile at ({x}, {y}): Category = {category}, Obstacle? = {map[i,j].isObstacle}, Status = {map[i,j].status}");
                else
                {
                    map[i,j]=new tile(this,j,i,category); 
                }
            }
        }
        initialize_Gamer();
       // Debug.Log("end"); 
    }

    public void initialize_Gamer()
    {
        gamerData = new GamerData(this,mapData);   
        //Debug.Log($"{gamerData.x},{gamerData.y},{gamerData.xt},{gamerData.yt}");  
        if (gamer != null)
        {
           gamer.transform.position = new Vector3(0.5f+gamerData.x * mapData.unit, 0.5f+gamerData.y * mapData.unit, gamer.transform.position.z);
        }
    }

    public void complete_the_level()
    {
        level++;
        Debug.Log($"{level}");
        StartCoroutine(LoadLevelAsync($"Level{level}"));
    }

    private IEnumerator LoadLevelAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null; // 等待場景加載完成
        }
        initialize_map(); // 場景加載完成後執行初始化
    }
    public void DoorActive(int group)
    {
        if(doorGroups.ContainsKey(group))
        {
            foreach (door Door in doorGroups[group])
            {
                Door.Switch(); // 切換狀態
            }
        }
        else    Debug.LogError("no dict key!");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            complete_the_level();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            int h=mapData.height,w=mapData.width;
            Debug.LogError("Start print map");
            for(int i=0;i<h;i++)
            {
                for(int j=0;j<w;j++)
                {
                    int k=i*w+j;
                    int category=mapData.category_i[k].category;
                    
                    if(category==3)
                    {
                        Debug.LogError($"x:{j},y:{i},cate{category},isO:{map[i,j].isObstacle}");
                        Debug.LogWarning($"{j},{i}:g:{mapData.category_i[k].group},s:{mapData.category_i[k].status}");
                    }     
                }
            }

        }

    }




    
}
