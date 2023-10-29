using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
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

    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public event Action onEncountered;
    // 몬스터 처치 횟수를 저장할 변수
    private int monsterKills = 0;
    void Start()
    {
        // 현재 씬에서 몬스터 카운트를 초기화
        monsterKills = 0;
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
                //Debug.Log(scanObject.name);
            }
            else
            { //아니면 점프
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
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
        if(collision.gameObject.tag == "Finish")
        {   // "SceneController" 스크립트의 인스턴스를 찾아서 가져옴
            SceneManager.LoadScene("Game 2");
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            // 몬스터를 처치할 때마다 카운트를 증가
            monsterKills++;

            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
            onEncountered();
            OnAttack(collision.transform);
            Debug.Log(monsterKills);

            // Finish 오브젝트를 찾아야 합니다.
            GameObject finishObject = GameObject.FindWithTag("Finish");

            if (finishObject != null)
            {
                // Finish 오브젝트를 활성화
                finishObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Finish2 오브젝트를 찾을 수 없습니다.");
            }
        }
    }

    /*void onDamaged(Vector2 targetPos)
    {
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        //애니메이션
        anim.SetTrigger("damaged");

    }
    */

    public void OnAttack(Transform enemy)
    {
        Enemy_move enemyMove = enemy.GetComponent<Enemy_move>();
        enemyMove.OnDamaged();
    }

    public void OnCollisionEnter(Collision collision)
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isJumping", false);
        onEncountered();
        OnAttack(collision.transform);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}
