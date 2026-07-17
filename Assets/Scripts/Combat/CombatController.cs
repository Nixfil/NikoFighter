using UnityEngine;

[RequireComponent(typeof(CharacterMovement3D))]
[RequireComponent(typeof(Animator))]
public class CombatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private IInputProvider inputProvider;

    [Header("Debug")]
    [SerializeField] private bool debugLogs = true;

    private CharacterMovement3D movement;
    private Animator animator;

    private AttackData currentAttack;
    public AttackData CurrentAttack => currentAttack;

    private bool isAttacking;
    private bool comboWindowOpen;
    private bool comboQueued;

    private CombatInput queuedInput;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement3D>();
        animator = GetComponent<Animator>();

        if (inputProvider == null)
            inputProvider = GetComponent<IInputProvider>();
    }

    private void Update()
    {
        if (currentWeapon == null || inputProvider == null)
            return;

        if (inputProvider.LightAttackTriggered)
            HandleAttackInput(CombatInput.Light);

        if (inputProvider.HeavyAttackTriggered)
            HandleAttackInput(CombatInput.Heavy);
    }

    private void HandleAttackInput(CombatInput input)
    {
        if (debugLogs)
        {
            Debug.Log(
                $"Input\n" +
                $"Attacking: {isAttacking}\n" +
                $"Current Attack: {(currentAttack != null ? currentAttack.attackName : "NULL")}\n" +
                $"ComboQueued: {comboQueued}\n" +
                $"ComboWindow: {comboWindowOpen}");
        }

        //==================================================
        // Queue combo continuation
        //==================================================

        if (isAttacking)
        {
            if (!comboWindowOpen)
                return;

            // Only buffer one input
            if (comboQueued)
                return;

            comboQueued = true;
            queuedInput = input;

            if (debugLogs)
                Debug.Log($"Queued {input}");

            return;
        }

        //==================================================
        // Start new combo
        //==================================================

        CombatState state = GetCombatState(input);

        currentAttack = currentWeapon
            .WeaponData
            .GetEntryAttack(state);

        if (currentAttack == null)
        {
            Debug.LogWarning($"No entry attack assigned for {state}");
            return;
        }

        PlayAttack(currentAttack);
    }

    private void PlayAttack(AttackData attack)
    {
        currentAttack = attack;

        isAttacking = true;
        comboQueued = false;
        comboWindowOpen = false;

        string animatorState =
            currentWeapon.WeaponData.AnimatorStateMachine +
            "." +
            attack.animatorState.State;

        if (debugLogs)
        {
            Debug.Log($"Playing {attack.attackName}");
        }

        animator.CrossFade(
            Animator.StringToHash(animatorState),
            0.05f);
    }

    private CombatState GetCombatState(CombatInput input)
    {
        switch (input)
        {
            case CombatInput.Light:
                return movement.IsGrounded
                    ? CombatState.GroundLight
                    : CombatState.AirLight;

            case CombatInput.Heavy:
                return movement.IsGrounded
                    ? CombatState.GroundHeavy
                    : CombatState.AirHeavy;

            default:
                return CombatState.None;
        }
    }

    public void OpenComboWindow()
    {
        comboWindowOpen = true;

        if (debugLogs)
            Debug.Log("Combo Window Open");
    }

    public void CloseComboWindow()
    {
        comboWindowOpen = false;

        if (debugLogs)
            Debug.Log("Combo Window Closed");
    }

    public void FinishAttack()
    {
        isAttacking = false;

        //==================================================
        // Continue combo if possible
        //==================================================

        if (comboQueued)
        {
            AttackData nextAttack =
                ComboGraph.GetNextAttack(
                    currentAttack,
                    queuedInput);

            if (nextAttack != null)
            {
                PlayAttack(nextAttack);
                return;
            }
        }

        //==================================================
        // Combo ended
        //==================================================

        ResetCombatState();

        // Do NOT CrossFade back to Idle here.
        // Let the Animator's Exit transition return to Locomotion.
    }

    private void ResetCombatState()
    {
        isAttacking = false;
        comboQueued = false;
        comboWindowOpen = false;

        queuedInput = default;
        currentAttack = null;
    }
}