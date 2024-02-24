using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public event Func<bool> getCollisionCheck;
    public Vector3 CurrentVelocity { get; private set; }
    private Vector3 workSpace;
    public bool CanSetVelosity { get; set; }
    public bool CanFlip { get; set; }

    public Rigidbody Rigidbody { get; private set; }

    public int FacingDirection { get; private set; }

    private bool ignoreLayers;

    protected override void Awake()
    {
        base.Awake();
        FacingDirection = 1;
        CanSetVelosity = true;
        CanFlip = true;
        Rigidbody = GetComponentInParent<Rigidbody>();
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = Rigidbody.velocity;
        if (getCollisionCheck != null)
        {
            ignoreLayers = getCollisionCheck.Invoke();
        }
    }


    public void SetVelocityZero()
    {
        workSpace = Vector3.zero;
        SetFinalVelocity();
    }

    public void MoveZ(float vel)
    {
        Rigidbody.gameObject.transform.localPosition += (Rigidbody.transform.forward * vel * Time.deltaTime);
        //Vector3 movement = transform.TransformDirection(Rigidbody.transform.forward * vel * Time.deltaTime);
        //Vector3 movement = transform.TransformDirection(new Vector3(moveHorizontal, 0.0f, moveVertical) * speed * Time.fixedDeltaTime);
        //workSpace.Set(vel, 0, CurrentVelocity.z);
        //Rigidbody.MovePosition(Rigidbody.transform.localPosition + movement);
        //Rigidbody.AddRelativeForce(workSpace * Time.deltaTime, ForceMode.Force);
        //workSpace.Set(vel , CurrentVelocity.y, CurrentVelocity.z);
        //SetFinalVelocity();
    }

    public void MoveX(float vel)
    {
        Vector3 movement = transform.TransformDirection(Rigidbody.transform.forward * vel * Time.deltaTime);
        //Vector3 movement = transform.TransformDirection(new Vector3(moveHorizontal, 0.0f, moveVertical) * speed * Time.fixedDeltaTime);
        //workSpace.Set(vel, 0, CurrentVelocity.z);
        //Rigidbody.MovePosition(Rigidbody.transform.localPosition + movement);
        Rigidbody.gameObject.transform.localPosition += (Rigidbody.transform.right * vel * Time.deltaTime);  
        //Rigidbody.AddRelativeForce(workSpace * Time.deltaTime, ForceMode.Force);
        //workSpace.Set(vel , CurrentVelocity.y, CurrentVelocity.z);
        //SetFinalVelocity();
    }

    public void SetVelocityZ(float vel)
    {

        workSpace.Set(CurrentVelocity.x, vel, CurrentVelocity.z);
        Rigidbody.AddForce(Vector3.forward * vel * Time.deltaTime, ForceMode.Impulse);
        //SetFinalVelocity();
    }

    public void SetVelocityY(float vel)
    {

        workSpace.Set(CurrentVelocity.x, vel, CurrentVelocity.z);
        SetFinalVelocity();
    }

    public void SetVelocity(Vector2 vel)
    {
        workSpace.Set(vel.x, vel.y, CurrentVelocity.z);
        Rigidbody.AddRelativeForce(workSpace * Time.deltaTime, ForceMode.Force);
        SetFinalVelocity();
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workSpace.Set(angle.x * velocity * direction, angle.y * velocity, CurrentVelocity.z);
        SetFinalVelocity();
    }


    public void RotateY(float rotationSpeed)
    {
        Rigidbody.gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public void Flip()
    {
        if (CanFlip)
        {
            //FacingDirection *= -1;
            //Rigidbody.transform.Rotate(0, 180f, 0);
        }
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void SetFacingDirection(int val)
    {
        FacingDirection = val;
    }

    private void SetFinalVelocity()
    {
        if (CanSetVelosity)
        {
            //Rigidbody.velocity = workSpace * Time.deltaTime;
            //CurrentVelocity = workSpace * Time.deltaTime;
            Rigidbody.velocity = workSpace;
            CurrentVelocity = workSpace;
        }
    }
}
