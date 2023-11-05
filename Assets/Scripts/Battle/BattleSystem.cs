using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { Start, MoveSelection, PerformMove/*EnemyMove*/, Busy , BattleOver, BossBattleOver}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    /*
     *     [SerializeField] BattleHud playerHud;
     *     [SerializeField] BattleHud enemyHud;
     * 
     */
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;
    public event Action<bool> OnBossBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;

    DamageDetails damageDetails;

    EnemyParty playerParty;
    EnemyParty enemyParty;

    PlayerMove player;
    Enemy_move enemyP;


    public void StartBattle(EnemyParty playerParty, EnemyParty enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;
        StartCoroutine(SetUpBattle());
    }


     
    public void StartEnemyBattle(EnemyParty playerParty, EnemyParty enemyParty)
    {
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;

        player = playerParty.GetComponent<PlayerMove>();
        enemyP = enemyParty.GetComponent<Enemy_move>();

        StartCoroutine(SetUpBattle());
        SoundManager.instance.bgSound.Stop();
        SoundManager.instance.PlaySFX("Battle");
    }
     


    public IEnumerator SetUpBattle()
    {
        playerUnit.Clear();
        enemyUnit.Clear();


        playerUnit.gameObject.SetActive(false);
        enemyUnit.gameObject.SetActive(false);
        dialogBox.gameObject.SetActive(false);

        var enemysEnemy = enemyParty.GetEnemy();
        enemyUnit.SetUp(enemysEnemy);
        enemyUnit.gameObject.SetActive(true);
        yield return dialogBox.TypeDialog($"{enemysEnemy.Base.Name} ���� ");
        yield return new WaitForSeconds(1f);

        /*--------------------------------------------------------------------*/

        var playerEnemy = playerParty.GetEnemy();
        playerUnit.SetUp(playerEnemy);
        playerUnit.gameObject.SetActive(true); 
        yield return new WaitForSeconds(0.5f);

        dialogBox.SetMovesName(playerUnit.Enemy.Moves);
        dialogBox.gameObject.SetActive(true);

        MoveSelection();

    }

    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        OnBattleOver(won);
        SoundManager.instance.sfxSource.Stop();
        SoundManager.instance.bgSound.Play();
    }

    

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        StartCoroutine(dialogBox.TypeDialog("��ų ����"));
        dialogBox.EnableDialogText(true);
        dialogBox.EnableMoveSelector(true);

    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;

        var move = playerUnit.Enemy.Moves[currentMove];

        yield return RunMove(playerUnit, enemyUnit, move);

        if (state == BattleState.PerformMove)
        {
            StartCoroutine(EnemyMove());
            yield return new WaitForSeconds(0.2f);
        }
        

    }

    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;


        var move = enemyUnit.Enemy.GetRandomMove();

        yield return RunMove(enemyUnit, playerUnit, move);

        if (state == BattleState.PerformMove)
            MoveSelection();
    }

    IEnumerator RunMove(BattleUnit sourceUnit/*����*/, BattleUnit targetUnit/*Ÿ��*/, Move move)
    {
        yield return dialogBox.TypeDialog($"{move.Base.Name}");
        yield return new WaitForSeconds(0.25f);

        sourceUnit.PlayAttackAnimation();

        var useMana = sourceUnit.Enemy.SpendMana(move, sourceUnit.Enemy);
        yield return sourceUnit.Hud.UpdateMana();
        yield return new WaitForSeconds(0.25f);

        if (move.Base.Name == "ȸ��")
        {
            // ȸ���� ���õǾ��� ��
            // ����  -ȸ��-  �Ҹ��� �ִ´ٸ� ���Ⱑ �ƴұ� ������ �� �𸣰ڴ�.
            SoundManager.instance.PlaySFX("Hill");
            var damageDetails = sourceUnit.Enemy.TakeDamage(move, sourceUnit.Enemy);
            yield return sourceUnit.Hud.UpdateHPPlus();

            useMana = sourceUnit.Enemy.SpendMana(move, sourceUnit.Enemy);
            yield return sourceUnit.Hud.UpdateMana();
        }
        else
        {
            // ȸ�� ���� �ٸ� ������ ���õǾ��� ��
            // ����  -����-  �Ҹ��� �ִ´ٸ� ���Ⱑ �ƴұ� ������ �� �𸣰ڴ�.
            SoundManager.instance.PlaySFX("Attack");
            targetUnit.PlayHitAnimation();
            var damageDetails = targetUnit.Enemy.TakeDamage(move, sourceUnit.Enemy);
            yield return targetUnit.Hud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if (damageDetails.Fainted)
            {
                yield return HandleEnemyFainted(targetUnit);
                // targetUnit.PlayFaintAnimation();
                yield return new WaitForSeconds(0.2f);

                CheckForBattleOver(targetUnit);
            }
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            BattleOver(false);
            // �÷��̾ �׾��� ��
            SceneManager.LoadScene("Game Over");
            
        }
        else if(faintedUnit.Enemy.Base.Name == "������")
        {
            BossBattleOver(true);
        } else
            BattleOver(true);
    }

    void BossBattleOver(bool won)
    {
        state = BattleState.BossBattleOver;
        OnBossBattleOver(won);
    }


    IEnumerator HandleEnemyFainted(BattleUnit faintedUnit)
    {

        yield return dialogBox.TypeDialog($"{faintedUnit.Enemy.Base.Name} �й�");
        // �� �״�� �й��ϴ� ��.

        faintedUnit.PlayFaintAnimation();

        yield return new WaitForSeconds(1f);
    }


    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("ȿ�����Դϴ�");

        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("ȿ���� �����ϴ�");

        yield return new WaitForSeconds(0.5f);
    }

    public void HandleUpdate()
    {
        if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Enemy.Moves.Count - 1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Enemy.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Enemy.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var move = playerUnit.Enemy.Moves[currentMove];
            // ������ ������ �������� �� �����̴� ��

            if (move.Mana == 0) return; // ������ ������ �����Ѵ�.

            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
            // �÷��̾ �����δ�.
        }

    }
}
