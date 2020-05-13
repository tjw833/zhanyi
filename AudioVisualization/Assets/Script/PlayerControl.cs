using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator anim;
    public Collider2D coll;
    public LayerMask Ground;
    public float speed;
    public float jumpforce;
    public int cherry;
    public int gem;

    public Text Cherrynum;
    public Text Gemnum;

    private bool isHurt;//默认=false

    public AudioSource jumpAudio,hurtAudio;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt)
        {
            move();
        }
        SwitchAnim();
    }

    //移动
    void move()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");

        //水平移动
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running", Mathf.Abs(horizontalmove));
        }
        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }

        //跳跃 
        if ((coll.IsTouchingLayers(Ground)) && (Input.GetButtonDown("Jump"))) { 
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            jumpAudio.Play();
            anim.SetBool("jumping", true);
            
        }


    }

    //切换动画
    void SwitchAnim()
    {
        anim.SetBool("idle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                anim.SetFloat("running", 0);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集樱桃
        if (collision.tag == "Cherry")
        {
            Destroy(collision.gameObject);
            cherry += 1;
            Cherrynum.text = cherry.ToString();

        }
        //收集钻石
        if (collision.tag == "Gem")
        {
            Destroy(collision.gameObject);
            gem += 1;
            Gemnum.text = gem.ToString();

        }
        if (collision.tag == "DeadLine")
        {
            
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
           
        }

    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            if (anim.GetBool("falling"))
            {
                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-3, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                hurtAudio.Play();
                isHurt = true;
            }
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
