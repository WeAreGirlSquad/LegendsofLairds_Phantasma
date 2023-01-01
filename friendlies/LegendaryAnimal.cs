using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LegendaryAnimal : MonoBehaviour
{
    [SerializeField] Transform[] locations;
    int anim_isMoving = Animator.StringToHash("isMoving");
    Animator animalAnimator;
    LegendaryHuntable huntable;
    bool enabledAi = false;
    bool hitdelay = false;

    private void Awake()
    {
        animalAnimator = GetComponent<Animator>();
        huntable = GetComponent<LegendaryHuntable>();
    }

    private void OnEnable()
    {
        enabledAi = true;
        StartCoroutine(LegendaryAnimalAi());
    }

    public void SetHitDelay()
    {
        hitdelay = true;
        DOTween.Kill(true);
    }

    IEnumerator LegendaryAnimalAi()
    {
        Vector3[] pathObjects = new Vector3[6];
        pathObjects[0] = locations[0].position;
        pathObjects[1] = locations[1].position;
        pathObjects[2] = locations[2].position;
        pathObjects[3] = locations[3].position;
        pathObjects[4] = locations[4].position;
        pathObjects[5] = locations[5].position;

        while(enabledAi && huntable.IsHuntableAlive())
        {
            animalAnimator.SetBool(anim_isMoving, true);

            if(!hitdelay)
            {
                transform.DOLocalPath(pathObjects, 10f, PathType.Linear, PathMode.Full3D, 10, null)
                .SetOptions(true, AxisConstraint.None, AxisConstraint.None)
                .SetLookAt(0.1f, false)
                .OnComplete(() => FinishMoving())
                ;

                yield return new WaitForSeconds(12.0f);
            } 
            else
            {
                DOTween.Kill(true);
                yield return new WaitForSeconds(2.0f);
                hitdelay = false;
            }

        }
        DOTween.Kill(false);

        LegendaryCore.io.DestructToGaia(gameObject);
        yield return null;
    }

    private void FinishMoving()
    {
        animalAnimator.SetBool(anim_isMoving, false);
    }

    private void OnDisable()
    {
        DOTween.Kill(true);
        animalAnimator.SetBool(anim_isMoving, false);
        StopAllCoroutines();
        
    }

}

