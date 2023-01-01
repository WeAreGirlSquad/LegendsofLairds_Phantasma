using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendaryPushable : MonoBehaviour
{
    [SerializeField]
    private float forceMagnitude;

    private void Awake()
    {
        
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if(hit.collider.gameObject.CompareTag("Pushable"))
        {
            Debug.Log("Hit Pushable");
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                rb.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
            }
        }

      
    }
}
