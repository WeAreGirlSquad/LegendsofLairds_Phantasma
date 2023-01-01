using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class LegendaryPet : MonoBehaviour
{
    Animator petAnimator;
    NavMeshAgent petNavMeshAgent;
    int hitpoints = 20;

    bool isMoving = false;
    bool isActive = false;
    bool isFollowing = false;
    float distance;
    [SerializeField] Transform followTarget;
    int anim_isMoving = Animator.StringToHash("isMoving");

    private void Awake()
    {
        petAnimator = GetComponent<Animator>();
        petNavMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        followTarget = LegendaryCore.io.GetLocalPlayer();
    }

    void OnEnable()
    {
        //followTarget = LegendaryCore.io.GetLocalPlayer();
        
    }

    void FixedUpdate()
    {
        if(followTarget == null)
        {
            followTarget = LegendaryCore.io.GetLocalPlayer();
            return;
        }

        petNavMeshAgent.destination = followTarget.position;
        if(petNavMeshAgent.velocity.magnitude < 0.2f)
        {
            petAnimator.SetBool(anim_isMoving, false);
        } else
        {
            petAnimator.SetBool(anim_isMoving, true);
        }

    }
}
