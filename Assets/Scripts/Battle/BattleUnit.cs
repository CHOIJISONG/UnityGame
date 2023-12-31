using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public AudioClip clip;

    public BattleHud Hud
    {
        get { return hud; }
    }

    public bool IsPlayerUnit {
        get { return isPlayerUnit; }
    }

    public Enemy Enemy { get; set; }

    public MoveBase Move { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void SetUp(Enemy enemy)
    {
        Enemy = enemy;
        if (isPlayerUnit)
            GetComponent<Image>().sprite = Enemy.Base.FrontSprite;
        else
            GetComponent<Image>().sprite = Enemy.Base.FrontSprite;

        hud.gameObject.SetActive(true);
        hud.SetData(enemy);

        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void Clear() {
        hud.gameObject.SetActive(false);
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
            image.transform.localPosition = new Vector3(-465f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(465f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));

        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y + 20f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}
