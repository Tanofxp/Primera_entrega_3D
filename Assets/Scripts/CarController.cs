using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DriveType
{
    RWD,
    FWD,
    AWD
}

public class CarController : MonoBehaviour
{
    public DriveType DriveType;
    private Rigidbody carRB;

    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;

    public GameObject humoPrefab;

    public float accInput;
    public float dirInput;
    public float frenoInput;
    
    public float motorHP;
    public float frenoP;
    public float slipAngle;
    private float speed;
    public AnimationCurve direccionCurva;
    [SerializeField] private GameObject stop_izq;
    [SerializeField] private GameObject stop_der;

    // Start is called before the first frame update
    void Start()
    {
        carRB = gameObject.GetComponent<Rigidbody>();
        InstanciarHumo();
    }
    void InstanciarHumo()
    {
        wheelParticles.FLWheel = Instantiate(humoPrefab, colliders.FLWheel.transform.position-Vector3.up*colliders.FLWheel.radius, Quaternion.identity, colliders.FLWheel.transform)
            .GetComponent<ParticleSystem>();
        wheelParticles.FRWheel = Instantiate(humoPrefab, colliders.FRWheel.transform.position - Vector3.up * colliders.FRWheel.radius, Quaternion.identity, colliders.FRWheel.transform)
           .GetComponent<ParticleSystem>();
        wheelParticles.RLWheel = Instantiate(humoPrefab, colliders.RRWheel.transform.position - Vector3.up * colliders.RRWheel.radius, Quaternion.identity, colliders.RRWheel.transform)
           .GetComponent<ParticleSystem>();
        wheelParticles.RRWheel = Instantiate(humoPrefab, colliders.RLWheel.transform.position - Vector3.up * colliders.RLWheel.radius, Quaternion.identity, colliders.RLWheel.transform)
           .GetComponent<ParticleSystem>();
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
        CheckParticles();
    }
    void CheckInput()
    {
        accInput = Input.GetAxis("Vertical");
        dirInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);
        
        if (accInput >= 0)
        {
            stop_izq.SetActive(false);
            stop_der.SetActive(false);
        }
        else
        {
            stop_izq.SetActive(true);
            stop_der.SetActive(true);
        }
        
        float movingDirection = Vector3.Dot(transform.forward, carRB.velocity);
        if (movingDirection < -0.5f && accInput > 0)
        {
            frenoInput = Mathf.Abs(accInput);
        }
        else if (movingDirection > 0.5f && accInput < 0)
        {
            frenoInput = Mathf.Abs(accInput);
            
        }
        else
        {
            frenoInput = 0;

        }
    }
    void ApplyMotor()
    {
        switch (DriveType)
        {
            case DriveType.FWD:
                colliders.FRWheel.motorTorque = motorHP * accInput;
                colliders.FLWheel.motorTorque = motorHP * accInput;
                break;

            case DriveType.RWD:
                colliders.RRWheel.motorTorque = motorHP * accInput;
                colliders.RLWheel.motorTorque = motorHP * accInput;
                break;

            case DriveType.AWD:
                colliders.FRWheel.motorTorque = motorHP * accInput;
                colliders.FLWheel.motorTorque = motorHP * accInput;
                colliders.RRWheel.motorTorque = motorHP * accInput;
                colliders.RLWheel.motorTorque = motorHP * accInput;
                break;

        };
        
    }
    void ApplyDireccion()
    {
        float steeringAngle = dirInput * direccionCurva.Evaluate(speed);
        if (slipAngle < 120f && slipAngle > 2f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, carRB.velocity + transform.forward, Vector3.up);
            steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        }
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }
    void ApplyFreno()
    {
        colliders.FRWheel.brakeTorque = frenoInput * frenoP * 0.9f;
        colliders.FLWheel.brakeTorque = frenoInput * frenoP * 0.9f;

        colliders.RRWheel.brakeTorque = frenoInput * frenoP * 0.5f;
        colliders.RLWheel.brakeTorque = frenoInput * frenoP * 0.5f;
    }
    void ApplyWheelPosition()
    {
        UpdateWheels(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheels(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheels(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheels(colliders.RLWheel, wheelMeshes.RLWheel);
    }
    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        colliders.FRWheel.GetGroundHit(out wheelHits[0]);
        colliders.FLWheel.GetGroundHit(out wheelHits[1]);

        colliders.RRWheel.GetGroundHit(out wheelHits[2]);
        colliders.RLWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.5f;
        if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > slipAllowance))
        {
            wheelParticles.FRWheel.Play();
        }
        else
        {
            wheelParticles.FRWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > slipAllowance))
        {
            wheelParticles.FLWheel.Play();
        }
        else
        {
            wheelParticles.FLWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > slipAllowance))
        {
            wheelParticles.RRWheel.Play();
        }
        else
        {
            wheelParticles.RRWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > slipAllowance))
        {
            wheelParticles.RLWheel.Play();
        }
        else
        {
            wheelParticles.RLWheel.Stop();
        }
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

[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem RLWheel;
    public ParticleSystem RRWheel;
}