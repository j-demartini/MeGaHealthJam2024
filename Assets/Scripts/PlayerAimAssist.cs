using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimAssist : MonoBehaviour
{
    public GameObject CurrentTarget { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Trigger Enter");
            if (CurrentTarget == null)
            {
                CurrentTarget = other.gameObject;
                Debug.Log("Target Acquired");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == CurrentTarget)
        {
            CurrentTarget = null;
        }
    }
}
