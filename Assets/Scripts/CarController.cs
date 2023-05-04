using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    private Rigidbody carRB;

    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float accInput;
    public float dirInput;
    public float frenoInput;

    public float motorHP;
    public float frenoP;
    private float slipAngle;
    private float speed;
    public AnimationCurve direccionCurva;
    // Start is called before the first frame update
    void Start()
    {
        carRB = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        speed = carRB.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplyDireccion();
        ApplyFreno();
        ApplyWheelPosition();
    }

    void CheckInput()
    {
        accInput = Input.GetAxis("Vertical");
        dirInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);

        if (slipAngle < 120f)
        {
            if (accInput < 0)
            {
                frenoInput = Mathf.Abs(accInput);
                accInput = 0;
            }
        }
        else
        {
            frenoInput = 0;
        }
    }
    void ApplyMotor()
    {
        colliders.RRWheel.motorTorque = motorHP * accInput;
        colliders.RLWheel.motorTorque = motorHP * accInput;
    }
    void ApplyDireccion()
    {
        float anguloDir = dirInput * direccionCurva.Evaluate(speed);
        colliders.FRWheel.steerAngle = anguloDir;
        colliders.FLWheel.steerAngle = anguloDir;
    }
    void ApplyFreno()
    {
        colliders.FRWheel.brakeTorque = frenoInput * frenoP * 0.7f;
        colliders.FLWheel.brakeTorque = frenoInput * frenoP * 0.7f;

        colliders.RRWheel.brakeTorque = frenoInput * frenoP * 0.3f;
        colliders.RLWheel.brakeTorque = frenoInput * frenoP * 0.3f;
    }
    void ApplyWheelPosition()
    {
        UpdateWheels(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheels(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheels(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheels(colliders.RLWheel, wheelMeshes.RLWheel);
    }
    void UpdateWheels(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RLWheel;
    public WheelCollider RRWheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RLWheel;
    public MeshRenderer RRWheel;
}