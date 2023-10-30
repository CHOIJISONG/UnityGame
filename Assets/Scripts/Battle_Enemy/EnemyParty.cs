using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyParty : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies;

    private void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.Init();
        }
    }

    public Enemy GetEnemy()
    {
        return enemies.Where(x => x.HP > 0).FirstOrDefault();
    }
}
