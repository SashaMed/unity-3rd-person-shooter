using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    private Entity entity;

    public int FacingDirection { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        entity = GetComponentInParent<Entity>();
    }

    public override void LogicUpdate()
    {
    }



    public void MoveForward(float vel)
    {
        entity.gameObject.transform.localPosition += (entity.transform.forward * vel * Time.deltaTime);

    }

    public void MoveX(float vel)
    {
        entity.gameObject.transform.localPosition += (entity.transform.right * vel * Time.deltaTime);  
    }

    public void RotateY(float rotationSpeed)
    {
        entity.gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
