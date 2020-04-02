using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Coin : NetworkComponent
{
    public override void HandleMessage(string flag, string value)
    {
        char[] remove = { '(', ')' };
        if (flag == "DUMMYROT")
        {
            string[] data = value.Trim(remove).Split(',');
            Vector3 euler = new Vector3(
                                                float.Parse(data[0]),
                                                float.Parse(data[1]),
                                                float.Parse(data[2])
                                                );

            if ((euler - this.transform.rotation.eulerAngles).magnitude < .5f)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(euler), .25f);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(euler);
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsServer)
            {
                this.transform.Rotate(new Vector3(10, 10, 10));
                SendUpdate("DUMMYROT", this.transform.rotation.eulerAngles.ToString());
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
        if (IsServer)
        {

        }
    }
}
