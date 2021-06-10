using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class GoalDetector : MonoBehaviourPunCallbacks,IPunObservable
//public class GoalDetector : CollisionDetector
{
    MissionManager missionManager;
    public List<string> missionRequirement;
    // Start is called before the first frame update
    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if(missionManager == null && controller)
            missionManager = controller.GetComponent<MissionManager>();
        
        if(missionManager == null)
            Debug.LogError(gameObject+" missing component MissionManager");
    }

    public void PlayerReached(Collider player)
    {
        if(missionManager == null)
            return;
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

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    #endregion
}
