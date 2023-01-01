using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LegendaryShark : MonoBehaviour
{
    [SerializeField] Transform[] locations;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LegendaryAudio.io.PlaySfx(LegendaryAudioType.DUNGEON_TRAP_GORE);
            LegendaryCore.io.KillPlayerByTrap();
            //gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            other.transform.DOMoveY(-4f, 1f);
            //other.transform.Translate(new Vector3(0, -2, 0), Space.Self);
            transform.LookAt(other.transform);
        }
    }

    private void OnEnable()
    {
        Vector3[] pathObjects = new Vector3[6]; 
        pathObjects[0] = locations[0].position;
        pathObjects[1] = locations[1].position;
        pathObjects[2] = locations[2].position;
        pathObjects[3] = locations[3].position;
        pathObjects[4] = locations[4].position;
        pathObjects[5] = locations[5].position;

        transform.DOLocalPath(pathObjects, 10f, PathType.Linear, PathMode.Full3D, 10, null).SetOptions(true, AxisConstraint.None, AxisConstraint.None).SetLookAt(0.5f, false).SetLoops(-1);
    }
}
