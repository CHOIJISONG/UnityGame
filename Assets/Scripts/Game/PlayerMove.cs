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
    // ���� óġ Ƚ���� ������ ����
    private int monsterKills = 0;
    void Start()
    {
        // ���� ������ ���� ī��Ʈ�� �ʱ�ȭ
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
        // ����
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            if (scanObject != null) //�����Ұ� ������ �̸����(������ �ȵ�)
            {
                gameManager.Action(scanObject);
                //Debug.Log(scanObject.name);
            }
            else
            { //�ƴϸ� ����
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }

        }

        // ���� ���ǵ�
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity 
                = new Vector2((rigid.velocity.normalized.x)*0.5f, rigid.velocity.y);
        }

        // ���� ��ȯ
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

        // �ȴ� �ִϸ��̼� ��ȯ
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        // ������ ���ǵ�
        float h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // �ִ� �ӵ�
        if (rigid.velocity.x > maxSpeed) //������ �ִ� �ӷ�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // ���� �ִ� �ӷ�
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
        

        //����׼�
        Debug.DrawRay(rigid.position, new Vector3(direction * detect_range, 0, 0), new Color(0, 0, 1));

        //Layer�� Object�� ��ü�� rayHit_detect�� ���� 
        RaycastHit2D rayHit_detect = Physics2D.Raycast(rigid.position, new Vector3(direction, 0, 0), detect_range, LayerMask.GetMask("Object"));

        //�����Ǹ� scanObject�� ������Ʈ ���� 
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
        {   // "SceneController" ��ũ��Ʈ�� �ν��Ͻ��� ã�Ƽ� ������
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
            // ���͸� óġ�� ������ ī��Ʈ�� ����
            monsterKills++;

            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
            onEncountered();
            OnAttack(collision.transform);
            Debug.Log(monsterKills);

            // Finish ������Ʈ�� ã�ƾ� �մϴ�.
            GameObject finishObject = GameObject.FindWithTag("Finish");

            if (finishObject != null)
            {
                // Finish ������Ʈ�� Ȱ��ȭ
                finishObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Finish2 ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }

    /*void onDamaged(Vector2 targetPos)
    {
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        //�ִϸ��̼�
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
