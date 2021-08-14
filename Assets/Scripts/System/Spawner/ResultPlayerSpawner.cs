using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPlayerSpawner : MonoBehaviour
{
    public GameObject malePrefab;
    public GameObject femalePrefab;
    public AnimationClip winClip;

    public TMP_Text textTitle;
    public TMP_Text textPlayer;
    GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        if (!DataManager.ContainData("result"))
            return;

        Data resultData = DataManager.GetData("result");

        GameObject characterPrefab = ((int)resultData.data)>0?malePrefab:femalePrefab;

        if (((int)resultData.data) >= 0)
        {
            playerObject = GameObject.Instantiate(characterPrefab,this.transform);
            playerObject.GetComponent<Animator>().Play("Wave");
            textTitle.text = " <rainb f=0.2><wave>Employee of the Day</wave><rainb>";
            textPlayer.text = "Player - " + resultData.str;
        }
        else
        {
            textTitle.text = "<pend>No best employee today<pend>";
            textPlayer.text = "";
        }

        DataManager.RemoveData("result");
    }
}
