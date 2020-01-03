using UnityEngine;
using RPG.Combat;

namespace RPG.Control {
    
    [RequireComponent(typeof(Battler))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Combat Input")]
        [SerializeField] string attackInput;
        [SerializeField] string defendInput;
        [SerializeField] string dodgeInput;

        Battler battler => GetComponent<Battler>();

        void Update()
        {
            CombatInput();
        }

        void CombatInput()
        {
            if (Input.GetButtonDown(attackInput))
            {
                battler.Attack();
                return;
            }

            if (Input.GetButtonDown(defendInput))
            {
                battler.Defend();
                return;
            }

            if (Input.GetButtonDown(dodgeInput))
            {
                battler.Dodge();
                return;
            }
        }
    }
}
