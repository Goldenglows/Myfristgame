//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewBehaviourScript : MonoBehaviour
//{
//    private Rigidbody2D rb;      // 2D ������������������ƶ���
//    private BoxCollider2D coll;
//    private SpriteRenderer sprite; // ������Ⱦ�������ƽ�ɫ����
//    private Animator anim;       // ���������������Ŷ�����
//    private int extraJumps;
//    [SerializeField] private int maxExtraJumps = 1;

//    [SerializeField] private LayerMask jumpableGround;

//    private float dirX = 0f;     // �洢ˮƽ���뷽��
//    [SerializeField] private float moveSpeed = 7f;  // �ƶ��ٶȣ���Inspector������
//    [SerializeField] private float jumpForce = 7f;  // ��Ծ���ȣ���Inspector������

//    [SerializeField] private AudioSource jumpSoundEffect;

//    private enum MovementState { idle, running, jumping, falling }

//    // Start is called before the first frame update
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        coll = GetComponent<BoxCollider2D>();
//        sprite = GetComponent<SpriteRenderer>();
//        extraJumps = maxExtraJumps;
//        anim = GetComponent<Animator>();
//    }

//    //private bool IsGrounded()
//    //{
//    //    // �����ӵ����ĵ�
//    //    Vector2 boxCenter = coll.bounds.center;

//    //    // �����ӵĴ�С����΢��С��ȣ�����ǽ�����У�
//    //    Vector2 boxSize = new Vector2(coll.bounds.size.x * 0.9f, coll.bounds.size.y);

//    //    // ���¼��ľ��루���Ը������������
//    //    float extraHeight = 0.1f;

//    //    // ִ�к��μ��
//    //    RaycastHit2D hit = Physics2D.BoxCast(
//    //        boxCenter,
//    //        boxSize,
//    //        0f,
//    //        Vector2.down,
//    //        extraHeight,
//    //        jumpableGround
//    //    );

//    //    // �����⵽��ײ������ true
//    //    return hit.collider != null;
//    //}

//    // Update is called once per frame
//    private void Update()
//    {

//        // ˮƽ�ƶ�
//        dirX = Input.GetAxis("Horizontal");
//        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

//        // ����Ƿ��ڵ���
//        bool isGrounded = IsGrounded();

//        // ����ڵ��棬���ö�����Ծ����
//        if (isGrounded)
//        {
//            extraJumps = maxExtraJumps;
//        }

//        // ��Ծ
//        if ( Input.GetButtonDown("Jump") && IsGrounded() )
//        {
//            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//            if (isGrounded) // ������Ծ
//            {
//                jumpSoundEffect.Play();
//                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//            }
//            else if (extraJumps > 0) // ���ж�����
//            {
//                jumpSoundEffect.Play();
//                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//                extraJumps--; // ����ʣ����Ծ����
//            }
//        }
//        UpdateAnimationState();//���¶���״̬
//    }

//    private void UpdateAnimationState()
//    {
//        MovementState state;
//        if (dirX > 0f)
//        {
//            state = MovementState.running;
//            sprite.flipX = false;
//        }
//        else if( dirX < 0f)
//        {
//            state = MovementState.running;
//            sprite.flipX = true;
//        }
//        else // ��ֹ
//        {
//            state = MovementState.idle;
//        }

//        if (rb.velocity.y > .1f)
//        {
//            state = MovementState.jumping;
//        }
//        else if (rb.velocity.y < -.1f)
//        {
//            state = MovementState.falling;
//        }
//        anim.SetInteger("state", (int)state);
//    }

//    private bool IsGrounded()
//    {
//        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb;      // 2D ������������������ƶ���
    private BoxCollider2D coll;
    private SpriteRenderer sprite; // ��Ⱦ�������ƽ�ɫ����
    private Animator anim;       // ���������������Ŷ�����

    [SerializeField] private LayerMask jumpableGround;  // ����Ծ�ĵ���ͼ��

    private int jumpCount;                      // ��ǰʣ����Ծ����
    private const int MaxJumpCount = 2;         // �����Ծ����

    private bool isGrounded;                    // �Ƿ�վ�ڵ���
    private bool wasGroundedLastFrame;          // ��һ֡�Ƿ��ڵ���  

    private float dirX = 0f;     // �洢ˮƽ���뷽��
    [SerializeField] private float moveSpeed = 7f;  // �ƶ��ٶȣ���Inspector������
    [SerializeField] private float jumpForce = 7f;  // ��Ծ���ȣ���Inspector������
    [SerializeField] private float coyoteTime = 0.15f;  // ��غ����Ծʱ�䣨�׳�����ʱ�䣩
    [SerializeField] private float jumpBufferTime = 0.15f; // ��Ծ��������ʱ��

    private float coyoteCounter = 0f;           // ����ʱ���ʱ��
    private float jumpBufferCounter = 0f;       // ��Ծ�����ʱ��

    private enum MovementState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpCount = MaxJumpCount; // ��ʼ����Ծ����
    }

    private void Update()
    {
        // ���µ�����״̬
        isGrounded = IsGrounded();

        // ���˲��������Ծ����
        if (isGrounded && !wasGroundedLastFrame)
        {
            jumpCount = MaxJumpCount;
        }

        // ���� coyote ��ʱ������غ�������һС��ʱ�䣩
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // ���� jump buffer ��ʱ������ǰ����Ծ���Ժ��Զ�������
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // ˮƽ�ƶ�
        dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // ��� jump buffer �� coyote time �����㣬��ִ����Ծ
        if (jumpBufferCounter > 0f && (coyoteCounter > 0f || jumpCount > 0))
        {
            ExecuteJump();                      // ִ����Ծ
            jumpBufferCounter = 0f;             // �����Ծ����
        }

        // �ָ��Ż��������Ծ�в��ɿ��������򽵵���Ծ�߶�
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (1.5f) * Time.deltaTime;
        }

        UpdateAnimationState();

        // ���浱ǰ֡�Ƿ��ڵ�����
        wasGroundedLastFrame = isGrounded;

    }

    private void ExecuteJump()
    {
        // ����� coyote time ����Ծ�������ǰ��һ���ʱ�䣩�����۴���
        if (coyoteCounter > 0f && jumpCount == MaxJumpCount)
        {
            // ������Ծ��������Ϊ���״���
        }
        else
        {
            
            jumpCount--;  // �۳�һ����Ծ����
        }

        // ʩ����Ծ��
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        coyoteCounter = 0f; // ���� coyote�������ٴ�����


    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else // ��ֹ
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

}
