using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NETWORK_ENGINE;

public class BadGuyMovement : NetworkComponent
{
    public Transform startPos;
    public Transform endPos;
    public GameObject badGuy;
    public NavMeshAgent badGuyNav;
    public bool whichPos;
    public string formattingV3 = "F4";
    // Start is called before the first frame update
    void Start()
    {
        badGuyNav = badGuy.gameObject.GetComponent<NavMeshAgent>();
        badGuyNav.SetDestination(endPos.position);
        whichPos = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDestination();
    }

    void CheckDestination()
    {
        if (whichPos == true)
        {
            float distance2Target = Vector3.Distance(badGuy.transform.position, endPos.transform.position);
            if (distance2Target < 2.0f)
            {
                badGuyNav.SetDestination(startPos.position);
                whichPos = false;
            }
        }

        if (whichPos == false)
        {
            float distance2Target = Vector3.Distance(badGuy.transform.position, startPos.transform.position);
            if (distance2Target < 2.0f)
            {
                badGuyNav.SetDestination(endPos.position);
                whichPos = true;
            }
        }
    }

    public override IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (IsServer)
            {
                SendUpdate("DUMMYPOS", this.transform.position.ToString(formattingV3));
            }
            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    public override void HandleMessage(string flag, string value)
    {
        char[] remove = { '(', ')' };
        if (flag == "DUMMYPOS")
        {
            string[] data = value.Trim(remove).Split(',');

            Vector3 target = new Vector3(
                                        float.Parse(data[0]),
                                        float.Parse(data[1]),
                                        float.Parse(data[2])
                                        );

            if ((target - this.transform.position).magnitude < .5f)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, target, .25f);
            }
            else
            {
                this.transform.position = target;
            }
        }
    }
}
