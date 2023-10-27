using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    [SerializeField] protected string id;

    public float Score { get; set; }

    public string Id => id;

    public abstract void Init(Unit unit);

    public abstract void StartAction(Unit unit);

    public abstract void StopAction(Unit unit);

    public virtual void UpdateAction(Unit unit)
    {

    }
}
