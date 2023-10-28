using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, MoveSelection, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;

    DamageDetails damageDetails;

    PlayerMove player;
    Enemy_move enemyT;


    public void StartBattle()
    {

        StartCoroutine(SetUpBattle());
    }

    public IEnumerator SetUpBattle()
    {
        playerUnit.SetUp();
        enemyUnit.SetUp();

        playerHud.SetData(playerUnit.Enemy);
        enemyHud.SetData(enemyUnit.Enemy);

        dialogBox.SetMovesName(playerUnit.Enemy.Moves);

        yield return dialogBox.TypeDialog($"{enemyUnit.Enemy.Base.Name} 등장 ");
        yield return new WaitForSeconds(0.5f);

        MoveSelection();

    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        StartCoroutine(dialogBox.TypeDialog("액션 선택"));
        dialogBox.EnableDialogText(true);
        dialogBox.EnableMoveSelector(true);

    }

    IEnumerator PlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Enemy.Moves[currentMove];

        yield return dialogBox.TypeDialog($"{move.Base.Name}");
        yield return new WaitForSeconds(0.25f);

        playerUnit.PlayAttackAnimation();

        var useMana = playerUnit.Enemy.SpendMana(move, playerUnit.Enemy);
        yield return playerHud.UpdateMana();
        yield return new WaitForSeconds(0.25f);

        if (move.Base.Name == "Hill")
        {
            var damageDetails = playerUnit.Enemy.TakeDamage(move, playerUnit.Enemy);
            yield return playerHud.UpdateHPPlus();

            if (damageDetails.Fainted)
            {
                yield return HandleEnemyFainted(enemyUnit);
            }
            else
            {
                StartCoroutine(EnemyMove());
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            enemyUnit.PlayHitAnimation();
            var damageDetails = enemyUnit.Enemy.TakeDamage(move, playerUnit.Enemy);
            yield return enemyHud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if (damageDetails.Fainted)
            {
                yield return HandleEnemyFainted(enemyUnit);
            }
            else
            {
                StartCoroutine(EnemyMove());
                yield return new WaitForSeconds(0.5f);
            }
        }

    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;


        var move = enemyUnit.Enemy.GetRandomMove();

        yield return dialogBox.TypeDialog($"{move.Base.Name}");
        yield return new WaitForSeconds(0.25f);

        enemyUnit.PlayAttackAnimation();

        var useMana = enemyUnit.Enemy.SpendMana(move, enemyUnit.Enemy);
        yield return enemyHud.UpdateMana();
        yield return new WaitForSeconds(0.25f);

        playerUnit.PlayHitAnimation();
        var damageDetails = playerUnit.Enemy.TakeDamage(move, playerUnit.Enemy);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Enemy.Base.Name} 다운");
            playerUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(1f);
            OnBattleOver(false);
        }
        else
        {
            MoveSelection();
        }
    }

    IEnumerator HandleEnemyFainted(BattleUnit faintedUnit)
    {
        yield return dialogBox.TypeDialog($"{faintedUnit.Enemy.Base.Name} 다운");
        faintedUnit.PlayFaintAnimation();

        yield return new WaitForSeconds(1f);

        OnBattleOver(true);
    }


    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialogBox.TypeDialog("효과적입니다");

        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialogBox.TypeDialog("효과가 없습니다");

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
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PlayerMove());
        }

    }
}
