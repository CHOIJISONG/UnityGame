using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject expBar;

    [SerializeField] ManaBar manaBar;

    Enemy _enemy;

    public void SetData(Enemy enemy)
    {
        _enemy = enemy;

        nameText.text = enemy.Base.Name;
        hpBar.SetHP((float)enemy.HP / enemy.MaxHp);
        manaBar.SetMana((float)enemy.Mana / enemy.MaxMana);
        SetExp();


    }

    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }

    public IEnumerator SetExpSmooth(bool reset = false)
    {
        if (expBar == null) yield break;

        if (reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);

        float normalizedExp = GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }


    float GetNormalizedExp()
    {
        int currLevelExp = _enemy.Base.GetExpForLevel(_enemy.Level);
        int nextLevelExp = _enemy.Base.GetExpForLevel(_enemy.Level + 1);

        float normalizedExp = (float)(_enemy.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_enemy.HP / _enemy.MaxHp);
    }

    public IEnumerator UpdateHPPlus()
    {
        yield return hpBar.SetHPSmoothPlus((float)_enemy.HP / _enemy.MaxHp);
    }

    public IEnumerator UpdateMana()
    {
        yield return manaBar.SetMANASmooth((float)_enemy.Mana / _enemy.MaxMana);
    }
}
