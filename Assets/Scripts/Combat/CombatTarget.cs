using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerControls callingControls)
        {
            if (!enabled) return false;
            if (!callingControls.GetComponent<Fighter>().CanAttack(this.gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                callingControls.GetComponent<Fighter>().Attack(this.gameObject);
            }
            return true;
        }

        public CursorMapping GetCursor(PlayerControls callingControls)
        {
            if (callingControls.GetComponent<Fighter>().CurrentWeapon.cursorMapping.texture != null)
            {
                return callingControls.GetComponent<Fighter>().CurrentWeapon.cursorMapping;
            }
            else
            {
                return callingControls.GetCursorMapping(CursorType.Attack);
            }
        }
    }

}