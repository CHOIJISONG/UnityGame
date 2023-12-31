using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public static PlayerMove instance;
    public PlayerMove player;

    public GameObject scanObject;
    int direction;
    float detect_range = 1.5f;

    public AudioClip clip;

    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public event Action onEncountered;
    public event Action<Collider2D> onEnterEnemysView;
    

    void Start()
    {
        // 현재 씬에서 몬스터 카운트를 초기화
        
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    public void HandleUpdate()
    {
        // 점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            if (scanObject != null) //조사할게 있으면 이름출력(점프는 안됨)
            {
                gameManager.Action(scanObject);
                anim.SetBool("isWalking", false);
                anim.SetBool("isJumping", false);
                SoundManager.instance.PlaySFX("Search");
                //Debug.Log(scanObject.name);
            }
            else
            { //아니면 점프
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);

                SoundManager.instance.PlaySFX("JUMP");
            }

        }

        // 멈춤 스피드
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity 
                = new Vector2((rigid.velocity.normalized.x)*0.5f, rigid.velocity.y);
        }

        // 방향 전환
        if (gameManager.isAction == false && Input.GetButton("Horizontal"))
        {
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                spriteRenderer.flipX = true;
                direction = -1;
                //Debug.Log(direction);
            }
            else
            {
                spriteRenderer.flipX = false;
                direction = 1;
                //Debug.Log(direction);
            }
        }

        // 걷는 애니메이션 전환
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        // 움직임 스피드
        float h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // 최대 속도
        if (rigid.velocity.x > maxSpeed) //오른쪽 최대 속력
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 최대 속력
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayhit.collider != null)
            {
                if (rayhit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
        
        OnMoveOver();
    }


    void FixedUpdate()
    {
        

        //조사액션
        Debug.DrawRay(rigid.position, new Vector3(direction * detect_range, 0, 0), new Color(0, 0, 1));

        //Layer가 Object인 물체만 rayHit_detect에 감지 
        RaycastHit2D rayHit_detect = Physics2D.Raycast(rigid.position, new Vector3(direction, 0, 0), detect_range, LayerMask.GetMask("Object"));

        //감지되면 scanObject에 오브젝트 저장 
        if (rayHit_detect.collider != null)
        {
            scanObject = rayHit_detect.collider.gameObject;
            //Debug.Log(scanObject.name);

        }
        else
        {
            scanObject = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {   // "SceneController" 스크립트의 인스턴스를 찾아서 가져옴
            if (SceneManager.GetActiveScene().name == "Game")
            {
                SceneManager.LoadScene("Game 2");
            }
            else if (SceneManager.GetActiveScene().name == "Game 2")
            {
                SceneManager.LoadScene("Game Talk2");
            }
            else if (SceneManager.GetActiveScene().name == "Game 3")
            {
                SceneManager.LoadScene("Game Talk3");
            }
            else if (SceneManager.GetActiveScene().name == "Game 4")
            {
                SceneManager.LoadScene("Game Talk4");
            }
            // SceneManager.LoadScene("Game 2");
        }
        else if (collision.gameObject.tag == "Finish1")
        {
            SceneManager.LoadScene("Game 3");
        }
        else if (collision.gameObject.tag == "Finish2")
        {
            SceneManager.LoadScene("Game 4");
        }
        else if (collision.gameObject.tag == "Finish3")
        {
            SceneManager.LoadScene("Game 5");
        }
    }

    /*
     
     public void OnCollisionEnter2D(Collision2D collision)
    {
        var collider = collision.collider;
        if (collision.gameObject.tag == "Enemy" )
        {

            // 몬스터를 처치할 때마다 카운트를 증가
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
            onEncountered();
            OnAttack(collision.transform);
            gameManager.monsterKill += 1;
        }
        
       

    }
     
     */



    void OnMoveOver()
    {
        CheckIfInEnemyView();
    }

     
     private void CheckIfInEnemyView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer);

        if (collider != null)
        {
            Debug.Log("확인");
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
            onEnterEnemysView?.Invoke(collider);
            gameManager.monsterKill += 1;
        }
    }
     


    public void OnAttack(Transform enemy)
    {
        Enemy_move enemyMove = enemy.GetComponent<Enemy_move>();
        enemyMove.OnDamaged();
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}
