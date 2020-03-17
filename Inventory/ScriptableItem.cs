using UnityEngine;
using RPG.Effect;
using RPG.Core;

namespace RPG.Inventory
{

    public enum ItemEnum
    {
        ITEM,
        WEAPON,
        EQUIPMENT
    }

    public enum ConsumeEffect
    {
        RESTORE_HEALTH
    }

    [CreateAssetMenu(fileName = "Items", menuName = "Items/New Item", order = 0)]
    public class ScriptableItem : ScriptableObject
    {
        [Header("Basic Information")]
        public string itemName = "Name";
        public string itemDescription = "...";
        public Sprite itemSprite = null;
        public ItemEnum itemType = ItemEnum.ITEM;


        [Header("Consumable")]
        public bool consumable = true;
        public ConsumeEffect consumeEffect = ConsumeEffect.RESTORE_HEALTH;
        public float restoreAmount;

        [Header("World")]
        public GameObject droppedInstance;

        [Header("Animations")]
        public AnimatorOverrideController animatorOverrideController;

        [Header("Prefab")]
        public GameObject weaponPrefab;

        [Header("Stack")]
        public bool stackable = true;

        [Header("Settings")]
        public bool visibleInInventory = true;

        public void Consume(GameObject target)
        {
            if (consumeEffect == ConsumeEffect.RESTORE_HEALTH)
            {
                RestoreHealth restoreHealth = new RestoreHealth(target.GetComponent<Health>(), restoreAmount);
                restoreHealth.Restore();
            }

            // Remove this item from the target's inventory
            target.GetComponent<CharacterInventory>().Remove(this);
        }


    }

}
