using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class NetworkTransform : NetworkComponent
{
    public Vector3 LastPosition = Vector3.zero;
    public Vector3 LastRotation = Vector3.zero;
    public Vector3 LastVelocity = Vector3.zero;
    public Vector3 LastRotVelocity = Vector3.zero;
    public Rigidbody rb;

    public override void HandleMessage(string flag, string value)
    {
        // (x, y, z)
        char[] remove = { '(', ')' };
        if(flag == "POS")
        {
            // naive approach
            string[] data = value.Trim(remove).Split(',');

            // If you have rigidbody
            // Find the distance between client position and server update position
            // If distance < .1 -- ignore
            // Else if distance <.5 -- lerp
            // else -- teleport

            Vector3 target = new Vector3(
                                        float.Parse(data[0]),
                                        float.Parse(data[1]),
                                        float.Parse(data[2])
                                        );
            if((target-rb.position).magnitude < .1f)
            {
                // do nothing
            }
            else if((target-rb.position).magnitude < .5f)
            {
                // lerp
                rb.position = Vector3.Lerp(rb.position, target, .25f);
            }
            else
            {
                rb.position = target;
            }
        }

        if(flag == "ROT")
        {
            // naive approach
            string[] data = value.Trim(remove).Split(',');
            Vector3 euler = new Vector3(
                                                float.Parse(data[0]),
                                                float.Parse(data[1]),
                                                float.Parse(data[2])
                                                );

            if((euler-rb.rotation.eulerAngles).magnitude < .1f)
            {
                // Do nothing
            }
            else if((euler-rb.rotation.eulerAngles).magnitude < .5f)
            {
                // slerp
                rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(euler), .25f);
            }
            else
            {
                rb.rotation = Quaternion.Euler(euler);
            }
        }
        
        if(flag == "VEL")
        {
            // naive approach
            string[] data = value.Trim(remove).Split(',');
            Vector3 targetVel = new Vector3(
                                            float.Parse(data[0]),
                                            float.Parse(data[1]),
                                            float.Parse(data[2])
                                            );
            rb.velocity = targetVel;
        }

        if (flag == "ROTVEL")
        {
            // naive approach
            string[] data = value.Trim(remove).Split(',');
            Vector3 targetRotVel = new Vector3(
                                            float.Parse(data[0]),
                                            float.Parse(data[1]),
                                            float.Parse(data[2])
                                            );
            rb.angularVelocity = targetRotVel;
        }
        
    }

    public override IEnumerator SlowUpdate()
    {
        while(IsServer)
        {
            //Is the position different?
            if(LastPosition != rb.position)
            {
                // SendUpdate
                SendUpdate("POS", rb.position.ToString("F4"));
                LastPosition = rb.position;
            }
            //Is the rotation different?
            if(LastRotation != rb.rotation.eulerAngles)
            {
                // Send Update
                SendUpdate("ROT", rb.rotation.eulerAngles.ToString("F4"));
                LastRotation = rb.rotation.eulerAngles;
            }
            
            //Is the velocity different?
            if(LastVelocity != rb.velocity)
            {
                // Send Update
                SendUpdate("VEL", rb.velocity.ToString("F4"));
                LastVelocity = rb.velocity;
            }

            // Is the rotational velocity different?
            if(LastRotVelocity != rb.angularVelocity)
            {
                // Send Update
                SendUpdate("ROTVEL", rb.angularVelocity.ToString("F4"));
                LastRotVelocity = rb.angularVelocity;
            }

            if(IsDirty)
            {
                SendUpdate("POS", rb.position.ToString("F4"));
                SendUpdate("ROT", rb.rotation.eulerAngles.ToString("F4"));
                SendUpdate("VEL", rb.velocity.ToString("F4"));
                SendUpdate("ROTVEL", rb.angularVelocity.ToString("F4"));
                IsDirty = false;
            }

            yield return new WaitForSeconds(MyCore.MasterTimer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
