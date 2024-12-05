using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mapControllNS;
using Unity.VisualScripting;
using Unity.Properties;
using System.Data.SqlTypes;

public class interact_State : IState
{
    public State_Con stateCon;
    public map_con mapCon;

    int Px0,Py0;
    public interact_State(State_Con _stateCon,map_con _mapCon)
    {
        stateCon=_stateCon;
        mapCon=_mapCon;
    }
    public void Enter()
    {
        if(mapCon.map[mapCon.gamerData.y,mapCon.gamerData.x].category==2)
        {
            mapCon.complete_the_level();
        }
        if(mapCon.map[mapCon.gamerData.y,mapCon.gamerData.x].category==4)
        {
            mapCon.complete_the_level();
        }
        stateCon.ChangeState(stateCon.walkState);
    }

    public void Update()
    {
        
    }
    public void Exit()
    {
        Debug.Log("Exiting teleport State");
    }
}
