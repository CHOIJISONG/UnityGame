using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public class Enemy
{
    [SerializeField] EnemyBase _base;
    [SerializeField] int level;


    public EnemyBase Base
    {
        get { return _base; }
    }

    public int Level 
    {
        get { return level; }
    }

    public int Exp { get; set; }

    public int HP { get; set; }

    public int Mana { get; set; }

    public List<Move> Moves { get; set; }

    public MoveBase moveBase { get; set; }

    private Transform healthTransform;
    private Vector2 HPScale;

    public void Init()
    {
        
        HP = MaxHp;
        Mana = MaxMana;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4)
            {
                break;
            }
        }

        Exp = Base.GetExpForLevel(Level);
    }



    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }

        return false;
    }


    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }

    public int ManaAttack
    {
        get { return Mathf.FloorToInt((Base.ManaAttack * Level) / 100f) + 5; }
    }

    public int ManaDefense
    {
        get { return Mathf.FloorToInt((Base.ManaDefense * Level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
    }

    public int MaxMana
    {
        get { return Mathf.FloorToInt(Base.MaxMana); }
    }

    public DamageDetails TakeDamage(Move move, Enemy attacker)
    {
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) *
            TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Fainted = false
        };

        float modifiers = 1f * type;
        float a = (2 * attacker.Level + 10) / 250f;

        float atk = (float)attacker.Attack;
        float defense = attacker.Defense;
        if (move.Base.MagicAttack)
        {
            atk = (float)attacker.ManaAttack;
            defense = attacker.ManaDefense;
        }
        float d = a * move.Base.Power * ((float)atk / defense) + 2;

        int damage = Mathf.FloorToInt(d * modifiers);
        if (move.Base.Name == "Hill")
        {
            if (HP + damage >= MaxHp)
            {
                HP = MaxHp;
            }
            else if (HP + damage < MaxHp)
            {
                HP += damage;
            }
        }
        else
        {
            HP -= damage;
        }

        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }


    public ManaUpdate SpendMana(Move move, Enemy attacker)
    {
        var updateMana = new ManaUpdate()
        {
            NoMana = false
        };

        Mana -= move.Mana;
        if (attacker.Mana <= 0)
        {
            Mana = 0;
            updateMana.NoMana = true;
        }
        return updateMana;
    }


    public Move GetRandomMove()
    {
        var movesWithMana = Moves.Where(x => x.Mana > 0).ToList();

        int r = Random.Range(0, movesWithMana.Count);
        return movesWithMana[r];
    }
}

public class ManaUpdate
{
    public bool NoMana { get; set; }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float TypeEffectiveness { get; set; }
}