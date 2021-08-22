using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelPlayerSpawner : MonoBehaviour
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;
    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag(EnumTag.GameController.ToString());
        if (!networkManager && controller)
            networkManager = controller.GetComponent<NetworkManager>();
        else
            return;

        if (networkManager.localPlayerObject == null)
            if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
            {
                GameObject playerObject = PhotonNetwork.Instantiate(networkManager.gender > 0 ? boyPrefab.name : girlPrefab.name, Vector3.zero, Quaternion.identity, 0);
                networkManager.localPlayerObject = playerObject;
                PlayerAgent agent = playerObject.GetComponent<PlayerAgent>();
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { PlayerAgent.PLAYER_GENDER_KEY, (byte)networkManager.gender } });
                agent.playerName = networkManager.clientNickName;
                agent.gender = networkManager.gender;
                agent.spawnOffset = GetIdPositionOffset(PhotonNetwork.LocalPlayer.ActorNumber,10,20,3);
                agent.RestPostion();
            }
            else
            {
                networkManager.localPlayerObject.transform.position = Vector3.up * 2;
            }

    }

    Vector2 GetIdPositionOffset(int id, int maxPerline, int maxLimit, int width, bool centered = true)
    {
        int lines = maxLimit % maxPerline != 0 ? maxLimit / maxPerline + 1 : maxLimit / maxPerline;

        int posX = id % maxPerline;
        int posY = id / maxPerline;

        if (centered)
        {
            posX -= maxPerline / 2;
            posY -= lines / 2;
        }

        return new Vector2(width * posX, width * posY);
    }
}
