using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class GameCharacter : NetworkComponent
{
    public string Uname;
    public bool cameraSpawned = false;
    public Text MyTextBox;
    public GameObject playerCamera;
    public int score = 0;
    
    public override void HandleMessage(string flag, string value)
    {
        if (flag == "UNAME")
        {

            Uname = value;
            MyTextBox.text = value + "\nPlayer Score: " + score;
        }

        if (flag == "UPDATESCORE")
        {
            MyTextBox.text = value + "\nPlayer Score: " + score;
        }
    }

    public override IEnumerator SlowUpdate()
    {
        //if (IsServer)
        //{
        //    NetworkPlayer[] AllPlayers = GameObject.FindObjectsOfType<NetworkPlayer>();
        //    for (int i = 0; i < AllPlayers.Length; i++)
        //    {
        //        if (AllPlayers[i].Owner == Owner)
        //        {
        //            Uname = AllPlayers[i].UserName;
        //            SendUpdate("UNAME", Uname);
        //        }
        //    }
        //}

        while (true)
        {
            if (IsServer)
            {
                if (IsDirty)
                {
                    SendUpdate("UNAME", Uname);
                    MyTextBox.text = Uname;
                    IsDirty = false;
                }
            }
            SendUpdate("UPDATESCORE", Uname);
            if (IsClient && !cameraSpawned)
            {
                if (!cameraSpawned)
                {
                    if (IsLocalPlayer)
                    {
                        playerCamera.SetActive(true);
                    }

                    if (!IsLocalPlayer)
                    {
                        playerCamera.SetActive(false);
                    }
                    cameraSpawned = true;
                }
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
