using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMan : MonoBehaviourPunCallbacks, IPunObservable
{

    public PhotonView photonView;
    public float cubeSpeed;
    public bool color;

    private Renderer rend;
    private float cubeRed;

    // Start is called before the first frame update
    void Start()
    {
        cubeSpeed = 2.0f;
        color = false;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            transform.Translate(-Time.deltaTime * cubeSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            transform.Translate(Time.deltaTime * cubeSpeed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            photonView.RPC(nameof(ChangeColor), RpcTarget.AllBuffered);
        }
        if (photonView.IsMine)
        {
            cubeRed = Mathf.Sin(Time.time) / 2 + 0.5f;
        }

        rend.material.color = new Color(cubeRed, 0, 0);
    }

    [PunRPC]
    public void ChangeColor()
    {
        if (color == false)
        {
            GetComponent<Renderer>().material.color = Color.green;
            color = true;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
            color = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(cubeRed);
        }
        else
        {
            cubeRed = (float)stream.ReceiveNext();
        }
    }

}
