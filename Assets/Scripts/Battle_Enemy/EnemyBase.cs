using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName ="Enemy/Create new Enemy")]

public class EnemyBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] EnemyType type1;
    [SerializeField] EnemyType type2;

    // Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int maxMana;
    [SerializeField] float attack;
    [SerializeField] int defense;
    [SerializeField] float manaAttack;
    [SerializeField] int manaDefense;
    [SerializeField] int speed;

    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;

    [SerializeField] List<LearnableMove> learnableMoves;

    public int GetExpForLevel(int level)
    {
        if (growthRate == GrowthRate.Fast)
        {
            return 4 * (level * level * level) / 5;
        }

        return -1;
    }




    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite { get { return frontSprite; } }

    public EnemyType Type1 { get { return type1; } }
    public EnemyType Type2 { get { return type2; } }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int MaxMana
    {
        get { return maxMana; }
    }

    public float Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public float ManaAttack
    {
        get { return manaAttack; }
    }
    public int ManaDefense
    {
        get { return manaDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

    public int ExpYield => expYield;

    public GrowthRate GrowthRate => growthRate;

}


[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base { get { return moveBase; } }
    public int Level { get { return level; } }

}

public enum EnemyType
{
    None,
    hill,
    Normal,
    fire,
    venom,
    ice,
    Magic
}

public enum GrowthRate
{
    Fast
}

public class TypeChart
{
    static float[][] chart =
    {

        //                       H   N   F   V  I   Ma
        /* Hill */ new float[] { 1f, 1f, 1f, 1f, 1f, 1f},
        /* Normal */ new float[] { 1f, 1f, 1f, 1f, 1f, 1f},
        /* Fire */ new float[] { 1f, 1.5f, 0.5f, 1f, 2f, 1f},
        /* Venom */ new float[] { 1f, 1.5f, 1f, 0.5f, 1f, 1f},
        /* Ice */ new float[] { 1f, 1.5f, 2f, 1f, 1.5f, 1f},
        /* Masic */ new float[] { 1f, 1f, 1f, 1f, 1f, 1.5f},

    };

    public static float GetEffectiveness(EnemyType attackType, EnemyType defenseType)
    {
        if (attackType == EnemyType.None || defenseType == EnemyType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}
