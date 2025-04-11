using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerProperties : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int health { get; set; } = 100;

    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int score { get; set; } = 0;

    [Networked, OnChangedRender(nameof(OnAnimationChanged))]
    public bool Slash { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnHitChanged))]
    public bool isHit { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnDieChanged))]
    public bool isDead { get; set; } = false;

    Animator anim;
    public Slider sliderHealth;
    public TextMeshProUGUI scoreText;

    public override void Spawned()
    {
        anim = GetComponent<Animator>();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player received damage. Health: " + health);

        isHit = true; // Trigger Hit animation

        if (health <= 0 && !isDead)
        {
            isDead = true; // Trigger Die animation
        }
    }

    private void OnInfoChanged()
    {
        if (sliderHealth != null)
            sliderHealth.value = health;
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void OnAnimationChanged()
    {
        anim.SetTrigger("Slash");
    }

    private void OnHitChanged()
    {
        anim.SetTrigger("Hit");
    }

    private void OnDieChanged()
    {
        anim.SetTrigger("Die");
    }

    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Slash = !Slash;
            }
        }
    }
}
