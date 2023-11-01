using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    public int nextMove;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    [SerializeField] GameObject exclamation;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        Invoke("Think", 5);
    }


    
    void FixedUpdate()
    {
        // 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 낭떠러지 인식
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y); 
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayhit.collider == null)
        {
            Turn();
        }

    }

    //재귀함수 ( 자기가 자기를 다시 호출)
    void Think()
    {
        // 다음 행동
        nextMove = Random.Range(-1, 2);

        //방향 애니메이션
        anim.SetInteger("WalkSpeed", nextMove);
        // 방향
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;


        // 다음 행동 (재귀)
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 2);
    }

    public void OnDamaged()
    {
        boxCollider.enabled = false;
        Invoke("DeActive", 2);
       
    }

    

    void DeActive()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }


     
     public IEnumerator TriggerEnemyBattle(PlayerMove player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        exclamation.SetActive(false);

        GameController.Instance.StartEnemyBattle(this);
        OnDamaged();
    }
     

}
