using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MissionContent
{
    public MissionContent(string name, string description = "", bool isFinished = false)
    {
        this.name = name;
        this.description = description;
        this.isFinished = false;
    }
    public string name;
    [TextArea]
    public string description;
    public bool isFinished = false;
    //public UnityEvent eventFinished;
    //public UnityEvent eventApplied;
}
public class MissionManager : MonoBehaviour
{
    public List<MissionContent> missionList;
    public UnityEngine.UI.Text missionText;
    [Header("Manager Events")]
    public UnityEvent<string> newMissionAdded;
    public UnityEvent<string> anyMissionFinished;
    public UnityEvent allMissionCompleted;

    void Start()
    {
        
    }

    public void RegMission(string missionName, string description = "", bool isFinished = false)
    {
        MissionContent mission = new MissionContent(missionName, description, isFinished);
        missionList.Add(mission);
        /*if(mission.eventApplied != null)
            mission.eventApplied.Invoke();*/
        newMissionAdded.Invoke(missionName);
    }

    public void RegMission(MissionContent mission)
    {
        missionList.Add(mission);
        /*if(mission.eventApplied != null)
            mission.eventApplied.Invoke();*/
        newMissionAdded.Invoke(mission.name);
    }

    public void SwitchMission(string missionName, bool state = true)
    {
        SwitchMission(FindMissionIndex(missionName), state);
    }
    public void SwitchMission(int missionIndex, bool state = true)
    {
        if (missionList[missionIndex].isFinished == state)
            return;

        missionList[missionIndex].isFinished = state;

        if (state)
        {
            //missionList[missionIndex].eventFinished.Invoke();
            anyMissionFinished.Invoke(missionList[missionIndex].name);
            if (CheckAllMissionCompleted())
                allMissionCompleted.Invoke();
        }
    }

    

    public bool CheckAllMissionCompleted()
    {
        bool result = true;
        foreach (MissionContent m in missionList)
        {
            result &= m.isFinished;
        }
        return result;
    }

    public bool CheckAllMissionCompleted(string[] list)
    {
        bool result = true;
        foreach (MissionContent m in missionList)
        {
            if (Array.FindIndex(list, listResult =>
            {
                return listResult == m.name;
            }) != -1)
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
        return missionList.Find(result =>
        {
            return missionName == result.name;
        });
    }

    public MissionContent FindMission(int missionIndex)
    {
        return missionList[missionIndex];
    }

    public int FindMissionIndex(string missionName)
    {
        return missionList.FindIndex(result =>
        {
            return missionName == result.name;
        });
    }

    public void ClearMissions()
    {
        missionList.Clear();
    }

    public void AddMissionFinishedEvent(string missionName, UnityAction action)
    {
        AddMissionFinishedEvent(FindMissionIndex(missionName), action);
    }

    public void AddMissionAppliedEvent(string missionName, UnityAction action)
    {
        AddMissionAppliedEvent(FindMissionIndex(missionName), action);
    }

    public void AddMissionFinishedEvent(int index, UnityAction action)
    {
        //missionList[index].eventFinished.AddListener(action);
    }

    public void AddMissionAppliedEvent(int index, UnityAction action)
    {
        //missionList[index].eventApplied.AddListener(action);
    }

    public string[] GetMissionArray()
    {
        string[] arr = missionList.ConvertAll<string>(result =>
        {
            return result.name;
        }).ToArray();
        return arr;
    }
}
