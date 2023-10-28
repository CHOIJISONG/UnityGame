using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName ="Move/Create new Move")]

public class MoveBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField] string name;

    [SerializeField] Sprite image;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] EnemyType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;

    [SerializeField] bool magicAttack;

    [SerializeField] int mana;

    [SerializeField] string animaitionName;

    public string Name
    {
        get { return name; }
    }

    public Sprite Image { get { return image; } }

    public string Description
    {
        get { return description; }
    }

    public EnemyType Type { get { return type; } }

    public int Power { get { return power; } }

    public int Accuraacy { get { return accuracy; } }

    public bool MagicAttack { get { return magicAttack; } }

    public int Mana { get { return mana; } }
}
