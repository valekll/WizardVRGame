using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonMan : MonoBehaviourPunCallbacks
{

    public GameObject cubePrefab;
    public GameObject playerPrefab;
    public Rig rig;

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.SendRate = 2;
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(Reconnect());
    }

    IEnumerator Reconnect()
    {
        yield return new WaitForSeconds(5f);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        };
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinOrCreateRoom("room", new Photon.Realtime.RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        Debug.Log("Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
        Avatar avatar = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Avatar>();
        PhotonHashtable ht = new PhotonHashtable();
        ht.Add("HitCount", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht, null, null);
        avatar.id = PhotonNetwork.LocalPlayer.ActorNumber;
        avatar.rig = rig;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            PhotonNetwork.Instantiate(cubePrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}
