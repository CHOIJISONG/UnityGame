using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_move : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    [SerializeField] GameObject exclamation;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    


    public IEnumerator TriggerEnemyBattle(PlayerMove player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        exclamation.SetActive(false);

        GameController.Instance.StartBossBattle(this);
        
    }
}
