using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerController : MonoBehaviour
{
    public enum ShotingType {bolas, rayo}
    public ShotingType currentShot;

    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;

    //PlayerInput _pi;
    //InputsGameplay gameplay;
    //InputAction movement;

    SoundManager soundManager;
    GameController controller;

    [Space]
    [Header("Components")]
    public Transform apuntador;
    public SpriteRenderer brazo;
    public AudioSource pasosSonido;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public float reboundForce = 1;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    public bool canGrab;
    public bool isShoting;
    public bool canShot;
    public bool enPlataforma;

    public bool activeTimerGrab = true;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    [Space]
    [Header("Polish")]
    public TrailRenderer dashRenderer;
    //public ParticleSystem dashParticle;
    //public ParticleSystem jumpParticle;
    //public ParticleSystem wallJumpParticle;
    //public ParticleSystem slideParticle;

    [Space]
    [Header("References")]
    public GameObject rayPrefab;
    public GameObject ballPrefab;
    public Transform spawnBallR, spawnBallL;
    public GameObject hudBolas;
    public GameObject hudRayos;
    public TextMeshProUGUI restanteRayosText;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<GameController>();
        soundManager = FindObjectOfType<SoundManager>();
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
        
        //INPUTS 
        //_pi = GetComponent<PlayerInput>();
        //movement = InputManager.inputActions.Player.Move;
        //gameplay = new InputsGameplay();


        //currentShot = ShotingType.bolas;
    }

    // Update is called once per frame
    void Update()
    {
        //MOVIMIENTO
        Vector2 dir = InputManager.inputActions.Player.Move.ReadValue<Vector2>();

        if(!isShoting)
            Walk(dir);

        if(InputManager.inputActions.Player.Move.ReadValue<Vector2>().x!=0 && canMove && !isShoting && !wallGrab && !wallSlide & !isDashing && groundTouch)
        {
            pasosSonido.mute = false;
        }
        else 
        {
            pasosSonido.mute = true;
        }

        if(enPlataforma && InputManager.inputActions.Player.Move.ReadValue<Vector2>().x == 0)
        {
            rb.isKinematic = true;
        }
        if(!enPlataforma || InputManager.inputActions.Player.Move.ReadValue<Vector2>().x != 0)
        {
            rb.isKinematic = false;
        }

        anim.SetHorizontalMovement(InputManager.inputActions.Player.Move.ReadValue<Vector2>().x, InputManager.inputActions.Player.Move.ReadValue<Vector2>().y, rb.velocity.y);

        if(InputManager.inputActions.Player.Pause.WasReleasedThisFrame())
        {
            controller.ActivarMenuPausa();
        }



        //AGARRE
        if (coll.onWall && InputManager.inputActions.Player.Grab.WasPressedThisFrame() && canMove && canGrab && Stats.instance.GetNivel()>=3)
        {
            if (side != coll.wallSide)
                anim.Flip(side * -1);
            wallGrab = true;
            wallSlide = false;

            if (activeTimerGrab)
            {
                StartCoroutine(GrabTime(4));
                activeTimerGrab =false;
            }

        }

        //REBOTE EN PARED
        if(wallJumped) 
        {
            StartCoroutine(GrabTime(0));
        }

        if (InputManager.inputActions.Player.Grab.WasReleasedThisFrame() || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;

        }
        //DISPARAR

        if (currentShot == ShotingType.bolas)
        {
            if (InputManager.inputActions.Player.RaysShot.WasPressedThisFrame())
            {
                currentShot = ShotingType.rayo;
                soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 21);

                hudBolas.SetActive(false);
                hudRayos.SetActive(true);
            }

            if (InputManager.inputActions.Player.Shot.WasPressedThisFrame() && canShot)
            {
                anim.SetTrigger("ShotBall");
                SpawnBallBullet();

            }

        }

        if(currentShot == ShotingType.rayo)
        {
            if (InputManager.inputActions.Player.BallsShot.WasPressedThisFrame())
            {
                currentShot = ShotingType.bolas;
                soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(),21);

                hudBolas.SetActive(true);
                hudRayos.SetActive(false);
            }


            if (InputManager.inputActions.Player.Shot.IsPressed() && canShot && Stats.instance.GetBalizas()>0)
            {
                apuntador.gameObject.SetActive(true);
                isShoting = true;

                var binding = InputManager.inputActions.Player.Shot.GetBindingForControl(InputManager.inputActions.Player.Shot.activeControl).Value;

                if (binding.groups == "Keyboard&Mouse")
                {
                    Vector2 objetivo = Camera.main.ScreenToWorldPoint(InputManager.inputActions.Player.MousePosition.ReadValue<Vector2>());
                    float angulosRadientes = Mathf.Atan2(objetivo.y - apuntador.position.y, objetivo.x - apuntador.position.x);
                    float anguloGrados = (180 / Mathf.PI) * angulosRadientes - 90;


                    apuntador.transform.rotation = Quaternion.Euler(0, 0, anguloGrados);

                    if (InputManager.inputActions.Player.MousePosition.ReadValue<Vector2>().x>transform.position.x)
                    {
                        brazo.flipY= false;
                        side = 1;
                    }
                    else
                    {
                        brazo.flipY = true;
                        side = -1;
                    }
                }
                if (binding.groups == "Joystick")
                {
                    Vector2 objetivo = new Vector2(transform.position.x + (dir.x * 5), transform.position.y + (dir.y * 5));
                    float angulosRadientes = Mathf.Atan2(objetivo.y - apuntador.position.y, objetivo.x - apuntador.position.x);
                    float anguloGrados = (180 / Mathf.PI) * angulosRadientes - 90;


                    apuntador.transform.rotation = Quaternion.Euler(0, 0, anguloGrados);

                    if (dir.x > 0)
                    {
                        brazo.flipY = false;
                    }
                    else
                    {
                        brazo.flipY = true;
                    }
                }

            }
            if (InputManager.inputActions.Player.Shot.WasReleasedThisFrame() && Stats.instance.GetBalizas() > 0)
            {
                SpawnRayBullet();

                isShoting = false;
                canMove = true;
                apuntador.gameObject.SetActive(false);

            }

            if (isShoting)
            {
                canMove = false;

            }
        }



        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;

        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (InputManager.inputActions.Player.Move.ReadValue<Vector2>().x > .2f || InputManager.inputActions.Player.Move.ReadValue<Vector2>().x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = InputManager.inputActions.Player.Move.ReadValue<Vector2>().y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, InputManager.inputActions.Player.Move.ReadValue<Vector2>().y * (speed * speedModifier));

        }
        else
        {
            rb.gravityScale = 3;
        }

        if (coll.onWall && !coll.onGround)
        {
            if (InputManager.inputActions.Player.Move.ReadValue<Vector2>().x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();

            }
        }

        if(coll.onGround && !coll.onWall || !coll.onGround && !coll.onWall)
        {
            canGrab= true;
            activeTimerGrab= true;

        }

        if (!coll.onWall || coll.onGround)
            wallSlide = false;

        if (InputManager.inputActions.Player.Jump.WasPressedThisFrame())
        {
            anim.SetTrigger("jump");

            if (coll.onGround)
                Jump(Vector2.up, false);
            if (coll.onWall && !coll.onGround)
                WallJump();
        }

        if (InputManager.inputActions.Player.Dash.IsPressed() && !hasDashed)
        {
            if (InputManager.inputActions.Player.Move.ReadValue<Vector2>().x != 0 || InputManager.inputActions.Player.Move.ReadValue<Vector2>().y != 0)
                Dash(InputManager.inputActions.Player.Move.ReadValue<Vector2>());
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        WallParticle(InputManager.inputActions.Player.Move.ReadValue<Vector2>().y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (InputManager.inputActions.Player.Move.ReadValue<Vector2>().x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (InputManager.inputActions.Player.Move.ReadValue<Vector2>().x < 0)
        {
            side = -1;
            anim.Flip(side);
        }


    }

    void SpawnRayBullet()
    {

        soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 11);
        CameraEffects.Instance.ShakeCamera(5, 5, 0.5f);

        GameObject bullet = Instantiate(rayPrefab);
        bullet.GetComponent<Bullets>().SetType(1);

        bullet.transform.position=apuntador.transform.GetChild(0).transform.position;
        bullet.transform.parent = null;

        bullet.transform.rotation = apuntador.rotation;

        //bullet.GetComponent<Bullets>().SetVetorDir(dir);
        
    }
    void SpawnBallBullet()
    {
        soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 2);
        CameraEffects.Instance.ShakeCamera(5, 5, 0.5f);

        GameObject bullet = Instantiate(ballPrefab);
        bullet.GetComponent<Bullets>().SetType(0);

        if(side== 1) 
        {
            if(wallGrab || wallSlide)
            {
                bullet.transform.position = spawnBallL.position;
                bullet.transform.rotation = spawnBallL.rotation;
            }
            else 
            {
                bullet.transform.position = spawnBallR.position;
                bullet.transform.rotation = spawnBallR.rotation;
            }
        }
        if (side == -1)
        {
            if (wallGrab || wallSlide)
            {
                bullet.transform.position = spawnBallR.position;
                bullet.transform.rotation = spawnBallR.rotation;
            }
            else
            {
                bullet.transform.position = spawnBallL.position;
                bullet.transform.rotation = spawnBallL.rotation;
            }



        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        //jumpParticle.Play();
    }

    private void Dash(Vector2 a)
    {

        if(Stats.instance.GetNivel()>=2)
        {
            //Camera.main.transform.DOComplete();
            //Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
            //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

            CameraEffects.Instance.ShakeCamera(5, 5, 0.5f);
            soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 17);

            hasDashed = true;

            anim.SetTrigger("dash");

            rb.velocity = Vector2.zero;
            Vector2 dir = a;

            rb.velocity += dir.normalized * dashSpeed;
            StartCoroutine(DashWait());
        }

    }

    IEnumerator DashWait()
    {
        //FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        //DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        //dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        dashRenderer.emitting= true;

        yield return new WaitForSeconds(.3f);

        ///dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;

        dashRenderer.emitting = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }

    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump(((Vector2.up / 3f + wallDir / 3f))* reboundForce, true);

        soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 8);

        wallJumped = true;
    }

    private void WallSlide()
    {
        if (coll.wallSide != side)
            anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    private void Jump(Vector2 dir, bool wall)
    {
        soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 8);

        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        //particle.Play();
    }

    IEnumerator GrabTime(float time)
    {
        yield return new WaitForSeconds(time);

        canGrab= false;
        wallGrab = false;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    void WallParticle(float vertical)
    {
        //var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            //main.startColor = Color.white;
        }
        else
        {
            //main.startColor = Color.clear;
        }
    }

    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }

    IEnumerator Damage(int a)
    {
        Stats.instance.SetOxigeno(Stats.instance.GetOxigeno() - 5);
        GameController.instance.ActualizarTexto();

        anim.SetTrigger("Damage");
        canMove= false;
        if (a == -1)
            rb.AddForce(-transform.right * 20, ForceMode2D.Impulse);
        if (a == 1)
            rb.AddForce(transform.right * 20, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.8f);
        canMove= true;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 18);

            int lado;

            if (collision.transform.position.x > transform.position.x)
                lado = -1;
            else
                lado = 1;

            StartCoroutine(Damage(lado));

        }

        if(collision.gameObject.tag== "Oxigeno")
        {
            soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 12);
            Stats.instance.SetOxigeno(Stats.instance.GetOxigeno() + 10);
            if(Stats.instance.GetOxigeno()>100)
                Stats.instance.SetOxigeno(100);

            GameController.instance.ActualizarTexto();
            Destroy(collision.gameObject);

        }
        
        if (collision.gameObject.tag == "Polvo")
        {
            soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 12);
            Stats.instance.SetPolvo(Stats.instance.GetPolvoEstelar() + 1);
            GameController.instance.ActualizarPolvos();

            Destroy(collision.gameObject);

        }

    }

    public void ActualizarRestanteRayosText()
    {
        restanteRayosText.text = Stats.instance.GetBalizas().ToString();
    }

}
