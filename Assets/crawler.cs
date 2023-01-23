using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class crawler : Agent
{
    Rigidbody rbd;
    public float g=1,f=1;
    public Transform bodyTransform;
    public Joint[] joints;

    enum actionState { 
        idle,moveForward,moveBack,moveLeft,moveRight,turnLeft,turnRight
    };
    actionState action;

    public override void CollectObservations(VectorSensor sensor)
    {
        
        sensor.AddObservation(bodyTransform.localPosition.y);
        sensor.AddObservation(bodyTransform.localPosition.x);
        sensor.AddObservation(bodyTransform.localPosition.z);
        sensor.AddObservation(bodyTransform.localEulerAngles.y);
        sensor.AddObservation(bodyTransform.localEulerAngles.x);
        sensor.AddObservation(bodyTransform.localEulerAngles.z);
        
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();

        //sets a random action state at the start of the episode
        System.Array A = System.Enum.GetValues(typeof(actionState));
        action = (actionState)A.GetValue(UnityEngine.Random.Range(0, A.Length));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)Input.GetAxis("Horizontal");
        discreteActions[1] = (int)Input.GetAxis("Vertical");
        discreteActions[2] = Input.GetKey(KeyCode.E) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0;

    }
    void Start()
    {
        rbd = GetComponent<Rigidbody>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            rbd.AddForce(0,1 * f, 0);
        rbd.AddForce(f/5*Input.GetAxis("Horizontal"), -1*g, f/5*Input.GetAxis("Vertical"));
    }
}
