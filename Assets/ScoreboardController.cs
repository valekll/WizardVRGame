using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    public Transform scoreboard;
    public TextMesh textMesh;
    public bool on;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = scoreboard.gameObject.GetComponent<TextMesh>();
        on = false;
    }

    // Update is called once per frame
    void Update()
    {
        float btn3 = Input.GetAxis("Fire3");

        if (Input.GetKeyDown(KeyCode.O))
        {
            on = on ? false : true;
            Debug.Log("Scoreboard On: " + on);
        }
        if (btn3 > 0.5f || on)
        {
            string scoreboardText = "Hits Taken:\nMe: " + PhotonNetwork.LocalPlayer.CustomProperties["HitCount"].ToString();
            Player[] players = PhotonNetwork.PlayerListOthers;
            foreach (Player p in players)
            {
                scoreboardText += "\nPlayer" + p.ActorNumber + ": " + p.CustomProperties["HitCount"].ToString();
            }

            textMesh.text = scoreboardText;
            scoreboard.gameObject.SetActive(true);
        }
        else
        {
            scoreboard.gameObject.SetActive(false);
        }
    }
}
