using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Con : MonoBehaviour//附在Gamer上
{
    public map_con mapCon;
    IState currentState;
    public walk_State  walkState;
    public teleport_State teleportState;
    public interact_State interactState;  
    public GameObject circle;
    public GameObject can;
    public GameObject cant;
     void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //禁止在切換場景時摧毀此物件
    }
    void Start()
    {
        mapCon=FindObjectOfType<map_con>();
        walkState = new walk_State(this,mapCon);
        teleportState = new teleport_State(this,mapCon,circle,can,cant);
        interactState = new interact_State(this,mapCon);

        currentState = walkState;
        Debug.Log($"{currentState}");
    }

    void Update()
    {
        currentState.Update();
    }
    public void ChangeState(IState newState)
    {
        currentState.Exit();//离开现状
        currentState = newState;
        currentState.Enter();
        Debug.Log($"{currentState}");
    }

 
}
