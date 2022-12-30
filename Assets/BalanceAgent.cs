using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BalanceAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localRotation.eulerAngles.z);
        sensor.AddObservation(transform.localRotation.eulerAngles.x);
        sensor.AddObservation(transform.localRotation.eulerAngles.y);
        sensor.AddObservation(targetTransform.localPosition - transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionZ = 2f * Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var actionX = 2f * Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        if((transform.localRotation.z < 0.25f && actionZ > 0f) || 
            (transform.localRotation.z > -0.25f && actionZ < 0f))
        {
            transform.Rotate(new Vector3(0, 0, 1), actionZ);
        }
        if ((transform.localRotation.x < 0.25f && actionX > 0f) ||
            (transform.localRotation.x > -0.25f && actionX < 0f))
        {
            transform.Rotate(new Vector3(1, 0, 0), actionX);
        }
        if (targetTransform.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            AddReward(10f);
            EndEpisode();
        }
            if ((targetTransform.localPosition.y - transform.localPosition.y) < -4f ||
            Mathf.Abs(targetTransform.localPosition.x - transform.localPosition.x) >10f ||
            Mathf.Abs(targetTransform.localPosition.z - transform.localPosition.z) > 10f)
        {
            SetReward(-1f);
            EndEpisode();
        }

        else
        {
            SetReward(0.1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localRotation = Quaternion.identity;
        targetTransform.localPosition = new Vector3(0f, 4f, 0f);
        targetTransform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-4.0f , 4.0f), 
            Random.Range(-4.0f , 4.0f), Random.Range(-4.0f, 4.0f)); // Change this to random
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    /*private void OnCollisionEnter(Collision collision)
    {
       if(collision.collider != null) AddReward(0.01f);
    }*/
    
}
