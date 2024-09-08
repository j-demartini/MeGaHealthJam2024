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
            if (CurrentTarget == null)
            {
                if (!other.gameObject.GetComponent<Enemy>().isDying)
                    CurrentTarget = other.gameObject;
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
