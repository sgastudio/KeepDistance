using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGenerator : MonoBehaviour
{
    public enum ApplyMode
    {
        overwrite,
        additive
    }
    public List<MissionContent> missionList;
    public ApplyMode missionApplyMode;
    public MissionManager missionManager;
    // Start is called before the first frame update
    void Start()
    {

        GameObject controller = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if (!missionManager && controller)
            missionManager = controller.GetComponent<MissionManager>();

        if (!missionManager)
            Debug.LogError(this + " missing component reference of MissionManager");
        else
            if (missionApplyMode == ApplyMode.overwrite)
        {
            missionManager.ClearMissions();
            missionManager.missionList.AddRange(this.missionList);
        }
        else
            missionManager.missionList.AddRange(this.missionList);
    }
}
