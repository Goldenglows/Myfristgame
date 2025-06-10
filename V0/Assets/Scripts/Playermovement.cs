//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NewBehaviourScript : MonoBehaviour
//{
//    private Rigidbody2D rb;      // 2D 刚体组件（控制物理移动）
//    private BoxCollider2D coll;
//    private SpriteRenderer sprite; // 精灵渲染器（控制角色朝向）
//    private Animator anim;       // 动画控制器（播放动画）
//    private int extraJumps;
//    [SerializeField] private int maxExtraJumps = 1;

//    [SerializeField] private LayerMask jumpableGround;

//    private float dirX = 0f;     // 存储水平输入方向
//    [SerializeField] private float moveSpeed = 7f;  // 移动速度（可Inspector调整）
//    [SerializeField] private float jumpForce = 7f;  // 跳跃力度（可Inspector调整）

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
//    //    // 检测盒子的中心点
//    //    Vector2 boxCenter = coll.bounds.center;

//    //    // 检测盒子的大小（稍微缩小宽度，避免墙壁误判）
//    //    Vector2 boxSize = new Vector2(coll.bounds.size.x * 0.9f, coll.bounds.size.y);

//    //    // 向下检测的距离（可以根据需求调整）
//    //    float extraHeight = 0.1f;

//    //    // 执行盒形检测
//    //    RaycastHit2D hit = Physics2D.BoxCast(
//    //        boxCenter,
//    //        boxSize,
//    //        0f,
//    //        Vector2.down,
//    //        extraHeight,
//    //        jumpableGround
//    //    );

//    //    // 如果检测到碰撞，返回 true
//    //    return hit.collider != null;
//    //}

//    // Update is called once per frame
//    private void Update()
//    {

//        // 水平移动
//        dirX = Input.GetAxis("Horizontal");
//        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

//        // 检测是否在地面
//        bool isGrounded = IsGrounded();

//        // 如果在地面，重置额外跳跃次数
//        if (isGrounded)
//        {
//            extraJumps = maxExtraJumps;
//        }

//        // 跳跃
//        if ( Input.GetButtonDown("Jump") && IsGrounded() )
//        {
//            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//            if (isGrounded) // 地面跳跃
//            {
//                jumpSoundEffect.Play();
//                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//            }
//            else if (extraJumps > 0) // 空中二段跳
//            {
//                jumpSoundEffect.Play();
//                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//                extraJumps--; // 减少剩余跳跃次数
//            }
//        }
//        UpdateAnimationState();//更新动画状态
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
//        else // 静止
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
    private Rigidbody2D rb;      // 2D 刚体组件（控制物理移动）
    private BoxCollider2D coll;
    private SpriteRenderer sprite; // 渲染器（控制角色朝向）
    private Animator anim;       // 动画控制器（播放动画）

    [SerializeField] private LayerMask jumpableGround;  // 可跳跃的地面图层

    private int jumpCount;                      // 当前剩余跳跃次数
    private const int MaxJumpCount = 2;         // 最大跳跃次数

    private bool isGrounded;                    // 是否站在地面
    private bool wasGroundedLastFrame;          // 上一帧是否在地面  

    private float dirX = 0f;     // 存储水平输入方向
    [SerializeField] private float moveSpeed = 7f;  // 移动速度（可Inspector调整）
    [SerializeField] private float jumpForce = 7f;  // 跳跃力度（可Inspector调整）
    [SerializeField] private float coyoteTime = 0.15f;  // 离地后可跳跃时间（俗称土狼时间）
    [SerializeField] private float jumpBufferTime = 0.15f; // 跳跃按键缓冲时间

    private float coyoteCounter = 0f;           // 土狼时间计时器
    private float jumpBufferCounter = 0f;       // 跳跃缓冲计时器

    private enum MovementState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpCount = MaxJumpCount; // 初始化跳跃次数
    }

    private void Update()
    {
        // 更新地面检测状态
        isGrounded = IsGrounded();

        // 落地瞬间重置跳跃次数
        if (isGrounded && !wasGroundedLastFrame)
        {
            jumpCount = MaxJumpCount;
        }

        // 设置 coyote 计时器（离地后还能跳的一小段时间）
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // 设置 jump buffer 计时器（提前按跳跃，稍后自动触发）
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 水平移动
        dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        // 如果 jump buffer 和 coyote time 都满足，就执行跳跃
        if (jumpBufferCounter > 0f && (coyoteCounter > 0f || jumpCount > 0))
        {
            ExecuteJump();                      // 执行跳跃
            jumpBufferCounter = 0f;             // 清空跳跃缓冲
        }

        // 手感优化：如果跳跃中并松开按键，则降低跳跃高度
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (1.5f) * Time.deltaTime;
        }

        UpdateAnimationState();

        // 保存当前帧是否在地面上
        wasGroundedLastFrame = isGrounded;

    }

    private void ExecuteJump()
    {
        // 如果在 coyote time 中跳跃（即落地前那一点点时间），不扣次数
        if (coyoteCounter > 0f && jumpCount == MaxJumpCount)
        {
            // 不减跳跃次数，因为是首次跳
        }
        else
        {
            
            jumpCount--;  // 扣除一次跳跃次数
        }

        // 施加跳跃力
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        coyoteCounter = 0f; // 重置 coyote，避免再次误判


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
        else // 静止
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
