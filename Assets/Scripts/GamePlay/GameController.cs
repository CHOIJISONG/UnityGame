
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, CutScene }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMove playerController;
    [SerializeField] Enemy_move enemyController;
    [SerializeField] BattleSystem battleSystem;

    [SerializeField] Camera worldCamera;



    GameState state;

    public static GameController Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerController.onEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

       

        playerController.onEnterEnemysView += (Collider2D enemyCollider) =>
        {
            var enemy = enemyCollider.GetComponentInParent<Enemy_move>();
            if (enemy != null)
            {
                state = GameState.CutScene;
                StartCoroutine(enemy.TriggerEnemyBattle(playerController));
            }
        };




    }



    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<EnemyParty>();
        var enemyParty = enemyController.GetComponent<EnemyParty>();

        battleSystem.StartBattle(playerParty, enemyParty);
    }

    
     
    public void StartEnemyBattle(Enemy_move enemy)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<EnemyParty>();
        var enemyParty = enemy.GetComponent<EnemyParty>();

        battleSystem.StartEnemyBattle(playerParty, enemyParty);
    } 

     


    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
