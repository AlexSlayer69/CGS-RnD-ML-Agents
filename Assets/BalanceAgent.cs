using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BalanceAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Platform platform;
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localRotation.eulerAngles);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float rotX = 90*actions.ContinuousActions[0];
        float rotZ = 90*actions.ContinuousActions[1];
        platform.rotateX(rotX);
        platform.rotateZ(rotZ);
    }

    public override void OnEpisodeBegin()
    {
        transform.localRotation = Quaternion.identity;
        targetTransform.localPosition = new Vector3(1.41f, 4f, -2.19f);
        targetTransform.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-4.0f , 4.0f), 
            Random.Range(-4.0f , 4.0f), Random.Range(-4.0f, 4.0f)); // Change this to random
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    void Update()
    {
        if (targetTransform.GetComponent<Rigidbody>().velocity == Vector3.zero) 
        {
            AddReward(10f);
            EndEpisode();
        }

        if (targetTransform.localPosition.y < (transform.localPosition.y-100f))
        {
            AddReward(-10f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.collider != null) AddReward(0.01f);
    }
    
}
