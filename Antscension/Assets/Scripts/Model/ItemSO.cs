using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    [field: SerializeField]
        public bool IsStackable { get; set; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }

        // [field: SerializeField]
        // public List<ItemParameter> DefaultParametersList { get; set; }

    }

    // [Serializable]
    // public struct ItemParameter : IEquatable<ItemParameter>
    // {
    //     public ItemParameterSO itemParameter;
    //     public float value;

    //     public bool Equals(ItemParameter other)
    //     {
    //         return other.itemParameter == itemParameter;
    //     }
    // }


