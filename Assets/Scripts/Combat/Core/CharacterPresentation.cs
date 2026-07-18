using UnityEngine;

[RequireComponent(typeof(Health))]
public class CharacterPresentation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int LightHash =
    Animator.StringToHash("HitLight");

    private static readonly int HeavyHash =
        Animator.StringToHash("HitHeavy");

    private static readonly int LaunchHash =
        Animator.StringToHash("Launch");

    private static readonly int KnockdownHash =
        Animator.StringToHash("Knockdown");

    private static readonly int StaggerHash =
        Animator.StringToHash("Stagger");

    private static readonly int DeathHash =
        Animator.StringToHash("Death");

    private Health health;

    private static readonly int HitTrigger =
        Animator.StringToHash("Hit");

    private static readonly int DeathTrigger =
        Animator.StringToHash("Death");

    [SerializeField] private MeshFlash meshFlash;
    private void Awake()
    {
        if (meshFlash == null)
            meshFlash = GetComponent<MeshFlash>();
        health = GetComponent<Health>();

        if (animator == null)
            animator = GetComponent<Animator>();

        health.OnHit += PlayHitReaction;
        health.OnDeath += PlayDeath;
    }

    private void OnDestroy()
    {
        if (health == null)
            return;

        health.OnHit -= PlayHitReaction;
        health.OnDeath -= PlayDeath;
    }

    private void PlayHitReaction(HitData hit)
    {
        PlayReaction(hit.Attack.hitReaction);

        meshFlash.Flash();
    }

    private void PlayDeath()
    {
        animator.SetTrigger(DeathTrigger);
    }
    private void PlayReaction(HitReaction reaction)
    {
        switch (reaction)
        {
            case HitReaction.Light:
                animator.SetTrigger(LightHash);
                Debug.Log("Light hit reaction triggered.");
                Debug.Log(LightHash);   
                break;

            case HitReaction.Heavy:
                animator.SetTrigger(HeavyHash);
                break;

            case HitReaction.Launch:
                animator.SetTrigger(LaunchHash);
                break;
        }
    }
}