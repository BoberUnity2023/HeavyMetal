using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // CAR SETUP
    [Space(20)]
    [Range(20, 190)]
    public int maxSpeed = 90; //The maximum speed that the car can reach in km/h.
    [Range(1, 10)]
    public int accelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.
    [Space(10)]
    [Range(10, 45)]
    public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
    [Space(10)]
    [Range(1000, 6000)]
    public int brakeForce = 3000; // The strength of the wheel brakes.

    // WHEELS
    [Space(20)]
    public GameObject frontLeftMesh;
    public WheelCollider frontLeftCollider;
    [Space(10)]
    public GameObject frontRightMesh;
    public WheelCollider frontRightCollider;
    [Space(10)]
    public GameObject rearLeftMesh;
    public WheelCollider rearLeftCollider;
    [Space(10)]
    public GameObject rearRightMesh;
    public WheelCollider rearRightCollider;

    //PARTICLE SYSTEMS
    [Space(20)]
    //The following variable lets you to set up particle systems in your car
    public bool useEffects = false;

    // The following particle systems are used as tire smoke when the car drifts.
    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;

    [Space(10)]
    // The following trail renderers are used as tire skids when the car loses traction.
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;

    //SOUNDS
    [Space(20)]
    //The following variable lets you to set up sounds for your car such as the car engine or tire screech sounds.
    public bool useSounds = false;
    public AudioSource carEngineSound; // This variable stores the sound of the car engine.
    public AudioSource tireScreechSound; // This variable stores the sound of the tire screech (when the car is drifting).
    float initialCarEngineSoundPitch; // Used to store the initial pitch of the car engine sound.

    //CAR DATA
    [HideInInspector]
    public float carSpeed; // Used to store the speed of the car.
    bool isDrifting; // Used to know whether the car is drifting or not.

    [HideInInspector]
    public Rigidbody carRigidbody; // Stores the car's rigidbody.
    float driftingAxis;
    float localVelocityZ;
    float localVelocityX;

    WheelFrictionCurve wheelFriction;
    float extremumSlip;

    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = Vector3.zero;

        wheelFriction = new WheelFrictionCurve();
        extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;

        wheelFriction.extremumSlip = extremumSlip;
        wheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        wheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        wheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        wheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;

        // We save the initial pitch of the car engine sound.
        if (carEngineSound != null)
        {
            initialCarEngineSoundPitch = carEngineSound.pitch;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // We determine the speed of the car.
        carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
        // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
        localVelocityX = transform.InverseTransformDirection(carRigidbody.linearVelocity).x;
        // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
        localVelocityZ = transform.InverseTransformDirection(carRigidbody.linearVelocity).z;

        AnimateWheelMeshes();
        DriftCarPS();
        CarSounds();

        if (isDrifting)
        {
            RecoverTraction();
        }
    }

    public float getMass()
    {
        return carRigidbody.mass;
    }

    public void setMass(float mass)
    {
        carRigidbody.mass = mass;
        Debug.Log("Current mass: " + mass);
    }

    public void SetSteeringAxis(float axis)
    {
        float steeringAngle = axis * maxSteeringAngle;
        frontLeftCollider.steerAngle = steeringAngle;
        frontRightCollider.steerAngle = steeringAngle;
    }

    public void SetThrottle(float power)
    {
        isDrifting = Mathf.Abs(localVelocityX) > 2.5f;

        if ((power > 0.0f && localVelocityZ < -1f) || (power < 0.0f && localVelocityZ > 1f))
        {
            UseBrakes(true);
            SetTorque(0.0f);
        }
        else
        {
            if (Mathf.RoundToInt(carSpeed) < maxSpeed)
            {
                SetTorque(accelerationMultiplier * 500f * power);
                UseBrakes(false);
            }
            else
            {
                SetTorque(0.0f);
            }
        }

        if (power == 0f)
        {
            carRigidbody.linearVelocity = carRigidbody.linearVelocity * 0.985f;

            SetTorque(0.0f);

            if (carRigidbody.linearVelocity.magnitude < 0.25f)
            {
                carRigidbody.linearVelocity = Vector3.zero;
            }
        }
    }

    void SetTorque(float torque)
    {
        frontLeftCollider.motorTorque = torque;
        frontRightCollider.motorTorque = torque;
        rearLeftCollider.motorTorque = torque;
        rearRightCollider.motorTorque = torque;
    }

    void UseBrakes(bool use)
    {
        float localBrakeForce = use ? brakeForce : 0f;

        frontLeftCollider.brakeTorque = localBrakeForce;
        frontRightCollider.brakeTorque = localBrakeForce;
        rearLeftCollider.brakeTorque = localBrakeForce;
        rearRightCollider.brakeTorque = localBrakeForce;
    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    void RecoverTraction()
    {
        driftingAxis = Mathf.Max(driftingAxis - Time.deltaTime * 1.5f, 0.0f);

        if (wheelFriction.extremumSlip > extremumSlip)
        {
            wheelFriction.extremumSlip = extremumSlip * driftingAxis;
        }
        else
        {
            wheelFriction.extremumSlip = extremumSlip;
            driftingAxis = 0f;
        }

        frontLeftCollider.sidewaysFriction = wheelFriction;
        frontRightCollider.sidewaysFriction = wheelFriction;
        rearLeftCollider.sidewaysFriction = wheelFriction;
        rearRightCollider.sidewaysFriction = wheelFriction;
    }

    // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
    void AnimateWheelMeshes()
    {
        frontLeftCollider.GetWorldPose(out Vector3 flwPosition, out Quaternion flwRotation);
        frontLeftMesh.transform.position = flwPosition;
        frontLeftMesh.transform.rotation = flwRotation;

        frontRightCollider.GetWorldPose(out Vector3 frwPosition, out Quaternion frwRotation);
        frontRightMesh.transform.position = frwPosition;
        frontRightMesh.transform.rotation = frwRotation;

        rearLeftCollider.GetWorldPose(out Vector3 rlwPosition, out Quaternion rlwRotation);
        rearLeftMesh.transform.position = rlwPosition;
        rearLeftMesh.transform.rotation = rlwRotation;

        rearRightCollider.GetWorldPose(out Vector3 rrwPosition, out Quaternion rrwRotation);
        rearRightMesh.transform.position = rrwPosition;
        rearRightMesh.transform.rotation = rrwRotation;
    }

    void DriftCarPS()
    {
        if (useEffects)
        {
            if (isDrifting)
            {
                if (RLWParticleSystem != null)
                {
                    RLWParticleSystem.Play();
                }
                if (RLWParticleSystem != null)
                {
                    RRWParticleSystem.Play();
                }
            }
            else
            {
                if (RLWParticleSystem != null)
                {
                    RLWParticleSystem.Stop();
                }
                if (RRWParticleSystem != null)
                {
                    RRWParticleSystem.Stop();
                }
            }

            RRWTireSkid.emitting = RLWTireSkid.emitting = Mathf.Abs(localVelocityX) > 5f && Mathf.Abs(carSpeed) > 12f;
        }
    }

    void CarSounds()
    {
        if (useSounds)
        {
            if (carEngineSound != null)
            {
                carEngineSound.pitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.linearVelocity.magnitude) / 25f);
            }
            if (isDrifting || Mathf.Abs(carSpeed) > 12f)
            {
                if (!tireScreechSound.isPlaying)
                {
                    tireScreechSound.Play();
                }
            }
            else
            {
                tireScreechSound.Stop();
            }
        }
    }
}
