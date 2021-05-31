using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
//public class GoalDetector : CollisionDetector
{
    MissionManager missionManager;
    public List<string> missionRequirement;
    // Start is called before the first frame update
    void Start()
    {
        missionManager = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<MissionManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerReached(Collider player)
    {

        Debug.Log("[Player" + player.GetInstanceID().ToString() + "] reaches the goal");
        bool allMission;
        if (missionRequirement.Count > 0)
            allMission = missionManager.CheckAllMissionCompleted(missionRequirement.ToArray());
        else
            allMission = missionManager.CheckAllMissionCompleted();
        Debug.Log("All Mission - " + allMission.ToString());

        if(allMission)
            GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString()).GetComponent<SceneControl>().TriggerWin();
    }
}
