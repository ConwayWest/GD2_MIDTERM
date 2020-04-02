using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;

public class GameManagerScript : NetworkComponent
{
    public bool GameStarted = false;
    NetworkCore MyCore;
    public NetworkPlayer[] temp;
    public bool playerSpawned = false;
    public bool playerCameraSpawn = false;
    public GameObject MainCamera;
    public GameObject EndScreen;
    public Text EndScreenText;
    public GameObject[] PlayerCameras;
    public GameObject[] PlayerManagers;
    public GameObject[] PlayerCharacters;
    public GameObject[] Coins;
    public GameObject[] SpawnPoints;

    public override void HandleMessage(string flag, string value)
    {
        if (flag == "START")
        {
            if (IsClient)
            {
                PlayerManagers = GameObject.FindGameObjectsWithTag("NetPlayManager");
                for (int i = 0; i < PlayerManagers.Length; i++)
                {
                    PlayerManagers[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        if (flag == "PLAYERSPAWNED")
        {
            if(IsClient)
            {
                MainCamera.gameObject.SetActive(false);
            }
        }

        if (flag == "ENDGAME")
        {
            PlayerCharacters = GameObject.FindGameObjectsWithTag("PlayerCharacter");
            for (int i = 0; i < PlayerCharacters.Length; i++)
            {
                GameCharacter temp = PlayerCharacters[i].gameObject.GetComponent<GameCharacter>();
                EndScreenText.text += "\n" + temp.Uname + ": " + temp.score.ToString();
            }
            if (IsClient)
            {
                EndScreen.SetActive(true);
                Time.timeScale = 0.0000001f;
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            yield return new WaitWhile(() => MyCore.IsConnected == false);
            while (MyCore.IsServer)
            {
                while (!GameStarted || MyCore.Connections.Count == 0)
                {
                    Debug.Log("The number of players is : " + MyCore.Connections.Count);

                    if (MyCore.Connections.Count != 0)
                    {
                        GameStarted = true;
                    }
                    temp = GameObject.FindObjectsOfType<NetworkPlayer>();
                    
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if(!temp[i].Ready)
                        {
                            GameStarted = false;
                        }
                    }
                    yield return new WaitForSeconds(MyCore.MasterTimer);
                }

                SendUpdate("START", "True");

                if (playerSpawned == false)
                {
                    SpawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
                    int spawnCount = SpawnPoints.Length - 1;
                    for (int i = 0; i < temp.Length; i++)
                    {
                        GameObject tempGo;
                        if (temp[i].UserCharacter == 0)
                        {
                            tempGo = MyCore.NetCreateObject(0, temp[i].Owner, SpawnPoints[spawnCount].transform.position);
                            tempGo.GetComponent<GameCharacter>().Uname = temp[i].UserName;
                        }
                        else if (temp[i].UserCharacter == 1)
                        {
                            tempGo = MyCore.NetCreateObject(1, temp[i].Owner, SpawnPoints[spawnCount].transform.position);
                            tempGo.GetComponent<GameCharacter>().Uname = temp[i].UserName;
                        }
                        else if (temp[i].UserCharacter == 2)
                        {
                            tempGo = MyCore.NetCreateObject(2, temp[i].Owner, SpawnPoints[spawnCount].transform.position);
                            tempGo.GetComponent<GameCharacter>().Uname = temp[i].UserName;
                        }
                        spawnCount -= 1;
                    }
                    playerSpawned = true;
                }
                SendUpdate("PLAYERSPAWNED", "0");
                Debug.Log(playerSpawned);
                if (playerSpawned == true)
                {
                    Coins = GameObject.FindGameObjectsWithTag("Coin");
                    if (Coins.Length <= 0)
                    {
                        EndScreen.SetActive(true);
                        SendUpdate("ENDGAME", EndScreenText.text);
                    }
                }

                yield return new WaitForSeconds(MyCore.MasterTimer);
            }
            yield return new WaitWhile(() => MyCore.IsConnected);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyCore = GameObject.FindObjectOfType<NetworkCore>();
        StartCoroutine("SlowUpdate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
