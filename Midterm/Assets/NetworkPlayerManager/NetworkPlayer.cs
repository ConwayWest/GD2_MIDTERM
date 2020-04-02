using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;
using UnityEngine.UI;

public class NetworkPlayer : NetworkComponent
{
    public int UserCharacter;
    public string UserName;
    public bool Ready = false;
    public int score = 0;
    public override void HandleMessage(string flag, string value)
    {
        if (flag == "CHARACTER")
        {
            UserCharacter = int.Parse(value);
            if (IsServer)
            {
                SendUpdate("CHARACTER", value);
            }
        }

        if (flag == "UNAME")
        {
            UserName = value;
            if(IsServer)
            {
                SendUpdate("UNAME", value);
            }
        }

        if (flag == "CLIENTREADY")
        {
            Ready = bool.Parse(value);
            if(IsServer)
            {
                SendUpdate("CLIENTREADY", value);
            }
        }

        //if (flag == "CREATEPLAYER")
        //{
        //    if (IsServer)
        //    {
        //        if (UserCharacter == 0)
        //        {
                    
        //        }
        //        else if (UserCharacter == 1)
        //        {
                    
        //        }
        //        else if (UserCharacter == 2)
        //        {
                    
        //        }

        //    }
        //}
    }

    public override IEnumerator SlowUpdate()
    {
        if (!IsLocalPlayer)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        //if (IsLocalPlayer)
        //{
        //    this.transform.GetChild(0).gameObject.SetActive(true);
        //}
        yield return new WaitForSeconds(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter(int c)
    {
        // Color = c;
        SendCommand("CHARACTER", c.ToString());
    }

    public void ChangeName(string n)
    {
        // Name = n;
        SendCommand("UNAME", n);
    }

    public void ReadyStatus()
    {
        SendCommand("CLIENTREADY", "True");
        //SendCommand("CREATEPLAYER", 0.ToString());
        //this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
