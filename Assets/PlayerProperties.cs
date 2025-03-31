using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;


public class PlayerProperties : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int health { get; set; } = 100;
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int mana { get; set; } = 100;
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int score { get; set; } = 0;

    [Networked, OnChangedRender(nameof(OnAnimationFireChanged))]
    public bool Fire { get; set; } = false;
    private void OnAnimationFireChanged()
    {
        anim.SetTrigger("Fire");
    }


    public GameObject weapon;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(int damage)
    {
        health -= damage;  // Giảm sức khỏe khi nhận sát thương
        Debug.Log("Player received damage. Health: " + health);


        if (health <= 0)
        {
            //Die();  // Player chết khi sức khỏe <= 0
        }
    }


    Animator anim;
    [Networked, OnChangedRender(nameof(OnAnimationChanged))]
    public bool Slash { get; set; } = false;


    private void OnAnimationChanged()
    {
        anim.SetTrigger("Slash");
    }

    public Slider sliderHealth;
    public Slider sliderMana;
    public TextMeshProUGUI scoreText;


    private void OnInfoChanged()
    {
        sliderHealth.value = health;
        sliderMana.value = mana;
        scoreText.text = score + "";
    }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // health -= 10;
                // mana -= 10;
                // score += 10;
                Slash = !Slash;
                weapon.GetComponent<BoxCollider>().enabled = true;
            }
            else if (Input.GetMouseButtonUp(0)) 
            {
                weapon.GetComponent<BoxCollider>().enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Fire = !Fire;
            }

        }
    }
}
