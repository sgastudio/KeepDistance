using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public string missionName;
    MissionManager missionManager;
    // Start is called before the first frame update
    void Start()
    {
        missionManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<MissionManager>();
        if(!missionManager)
            Debug.LogError("MissionTrigger missing component MissionManager");
    }

    public void SwitchMission(bool state)
    {
        if (missionManager)
            missionManager.SwitchMission(missionName, state);
    }

    public bool GetMission()
    {
        if (missionManager)
            return missionManager.FindMission(missionName).isFinished;
        Debug.LogWarning("Mission - " + missionName + " not found");
        return false;
    }
}
