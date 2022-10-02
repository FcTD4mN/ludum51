using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float mSpeed = 1f;
    public float mBaseDamage = 1f;


    private GameObject mHealthBar;
    private Killable mKillable;
    private float mHealthBarInitialScaleX = 1f;

    void Start()
    {
        mHealthBar = transform.Find("HealthBar").gameObject;
        Debug.Assert(mHealthBar != null, "No health bar");
        mHealthBarInitialScaleX = mHealthBar.transform.localScale.x;
        mHealthBar.SetActive(false);

        mKillable = GetComponent<Killable>();
        Debug.Assert(mKillable != null, "Not killable");
    }


    void FixedUpdate()
    {
        PlayerController thePlayer = GameManager.mInstance.mThePlayer;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector3 direction = (thePlayer.transform.position - transform.position).normalized;
        float finalSpeed = Time.fixedDeltaTime * mSpeed;

        rb.velocity = new Vector2(direction.x * finalSpeed, direction.y * finalSpeed);

        UpdateHealthBar();
    }


    void UpdateHealthBar()
    {
        float ratio = mKillable.mLife / mKillable.mBaseLife;

        if (ratio < 1.0)
        {
            mHealthBar.SetActive(true);
            mHealthBar.transform.localScale = new Vector3(mHealthBarInitialScaleX * ratio, mHealthBar.transform.localScale.y, 1);
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        Ludum51.Player.Player thePlayer = collision.collider.gameObject.GetComponent<Ludum51.Player.Player>();

        if (thePlayer != null)
        {
            Killable killable = thePlayer.GetComponent<Killable>();
            PlayerController pController = thePlayer.GetComponent<PlayerController>();

            killable.Hit(mBaseDamage);
            // TODO: connect life
            pController.UpdateHealthBar();

            if (killable.IsDead())
            {
                pController.PlayDeathAnimation(() =>
                {
                    thePlayer.gameObject.SetActive(false);
                });
            }
        }
    }
}
