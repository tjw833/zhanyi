using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_frog : MonoBehaviour
{
    private Rigidbody2D rb;

    public Transform leftpoint, rightpoint;
    public float speed;
    private float leftx,rightx;

    public bool faceleft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();//断绝父子关系
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

  
    void Update()
    {
        move();
    }

    //水平移动
    void move()
    {
        //面向左
        if (faceleft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
           
        }
        else//面向右
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceleft = true;
            }
        }
    }
}
