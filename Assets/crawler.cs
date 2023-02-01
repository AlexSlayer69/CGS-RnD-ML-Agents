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
    public Transform bodyTransform,targetTransform;
    public Joint[] joints;
    public float jointTorque;
    public GameObject target;
    public Collider bodyCollider;
    public float energyUsage;

    enum actionState { 
        idle,moveForward,moveBack,moveLeft,moveRight,turnLeft,turnRight
    };
    actionState action;

    public override void CollectObservations(VectorSensor sensor)
    {
        
        sensor.AddObservation(bodyTransform.localPosition.y);
        sensor.AddObservation(bodyTransform.localPosition.x);
        sensor.AddObservation(bodyTransform.localPosition.z);

        sensor.AddObservation(targetTransform.localPosition.y);
        sensor.AddObservation(targetTransform.localPosition.x);
        sensor.AddObservation(targetTransform.localPosition.z);

        //sensor.AddObservation(bodyTransform.localEulerAngles.y);
        //sensor.AddObservation(bodyTransform.localEulerAngles.x);
        //sensor.AddObservation(bodyTransform.localEulerAngles.z);
        
    }

    public override void OnEpisodeBegin()
    {
        //sets a random action state at the start of the episode
        System.Array A = System.Enum.GetValues(typeof(actionState));
        action = (actionState)A.GetValue(UnityEngine.Random.Range(0, A.Length));

        target.transform.localPosition = new Vector3(Random.Range(-20, 20),2, Random.Range(-20, 20));
        bodyTransform.localPosition = new Vector3(0, 5, 0);
        bodyTransform.rotation = Quaternion.identity;
        foreach (Joint joint in joints)
        {
            joint.transform.rotation = Quaternion.identity;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        for (int i = 0; i < 4; i++)
        {
            joints[i].transform.eulerAngles += new Vector3(actions.ContinuousActions[3*i], actions.ContinuousActions[3 * i+1], actions.ContinuousActions[3 * i+2])*jointTorque*Time.fixedDeltaTime;
        }

        for (int i = 4; i < joints.Length; i++)
        {
            joints[i].transform.eulerAngles += new Vector3(actions.ContinuousActions[i+8], actions.ContinuousActions[i + 8], actions.ContinuousActions[i + 8]) * jointTorque*Time.fixedDeltaTime;
        }

        //for (int i = 0; i < actions.ContinuousActions.Length; i++)
        //{
            //AddReward(-1 * energyUsage * actions.ContinuousActions[i] * actions.ContinuousActions[i]);
        //}

        Vector2 relativePos = new Vector2(bodyTransform.position.x - targetTransform.position.x, bodyTransform.position.z - targetTransform.position.z);
        if (relativePos.sqrMagnitude<0.25f)
        {
            SetReward(1 / Time.fixedDeltaTime);
            EndEpisode();
        }

        if(bodyTransform.localPosition.x>=25 || bodyTransform.localPosition.x <=-25 || bodyTransform.localPosition.y >= 25 || bodyTransform.localPosition.y <= -25 || bodyTransform.localPosition.y <= 3f)
        {
            SetReward(-1 / Time.fixedDeltaTime);
            EndEpisode();
        }

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float moveX, moveY;
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        target.transform.Translate(moveX * Time.deltaTime, 0, moveY * Time.deltaTime);
    }
    void Start()
    {
        rbd = GetComponent<Rigidbody>();
        targetTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            rbd.AddForce(0,1 * f, 0);
        rbd.AddForce(f/5*Input.GetAxis("Horizontal"), -1*g, f/5*Input.GetAxis("Vertical"));
    }

   
}
