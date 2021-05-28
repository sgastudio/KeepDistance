using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MissionContent{   
    public MissionContent(string name)
    {
        this.name = name;
    }
    public string name;
    public bool isFinished = false;
    public UnityEvent eventFinished;
    public UnityEvent eventApplied;
}
public class MissionManager : MonoBehaviour
{
    public List<MissionContent> missionList;
    [Header("Manager Events")]
    public UnityEvent newMissionAdded; 
    public UnityEvent anyMissionFinished; 
    public UnityEvent allMissionCompleted;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegMission(string missionName, UnityEvent onFinish=null,UnityEvent onReg=null)
    {
        MissionContent mission = new MissionContent(missionName);
        missionList.Add(mission);
        if(mission.eventApplied != null)
            mission.eventApplied.Invoke();
        newMissionAdded.Invoke();
    }
    public void SwitchMission(string missionName, bool state = true)
    {
        SwitchMission(FindMissionIndex(missionName),state);
    }
    public void SwitchMission(int missionIndex, bool state = true)
    {
        if(missionList[missionIndex].isFinished == state)
            return;
            
        missionList[missionIndex].isFinished = state;
        if(state)
        {
            missionList[missionIndex].eventFinished.Invoke();
            anyMissionFinished.Invoke();
            if(CheckAllMissionCompleted())
                allMissionCompleted.Invoke();
        }
    }

    public bool CheckAllMissionCompleted()
    {
        bool result = true;
        foreach(MissionContent m in missionList)
        {
            result &= m.isFinished;
        }
        return result;
    }
    public void FinishMission(int missionIndex)
    {
        SwitchMission(missionIndex);
    }
    public void FinishMission(string missionName)
    {
        SwitchMission(missionName);
    }

    public void UnfinishedMission(string missionName)
    {
        SwitchMission(missionName, false);
    }

    public void UnfinishedMission(int missionIndex)
    {
        SwitchMission(missionIndex, false);
    }

    /*
    public void SwitchMissionIndex(int missionIndex, bool state = true)
    {
        FindMissionIndex(missionName).isFinished = state;
        if(state & FindMission(missionName).eventFinished!=null)
            FindMission(missionName).eventFinished.Invoke();
    }*/

    public MissionContent FindMission(string missionName)
    {
        return missionList.Find(result=>{
            return missionName == result.name;
        });
    }

    public MissionContent FindMission(int missionIndex)
    {
        return missionList[missionIndex];
    }

    public int FindMissionIndex(string missionName)
    {
        return missionList.FindIndex(result=>{
            return missionName == result.name;
        });
    }

    public void ClearMissions()
    {
        missionList.Clear();
    }

    public void AddMissionFinishedEvent(string missionName, UnityAction action)
    {
        AddMissionFinishedEvent(FindMissionIndex(missionName),action);
    }   

    public void AddMissionAppliedEvent(string missionName, UnityAction action)
    {
        AddMissionAppliedEvent(FindMissionIndex(missionName),action);
    }

    public void AddMissionFinishedEvent(int index, UnityAction action)
    {
        missionList[index].eventFinished.AddListener(action);
    }   

    public void AddMissionAppliedEvent(int index, UnityAction action)
    {
        missionList[index].eventApplied.AddListener(action);
    }
}
