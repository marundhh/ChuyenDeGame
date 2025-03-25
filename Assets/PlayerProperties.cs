using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;


[System.Serializable]
public struct PlayerInfo : INetworkStruct
{
    public int health;
    public int mana;
    public int score;
}
public class PlayerProperties : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    private PlayerInfo info { get; set; }


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
        sliderHealth.value = info.health;
        sliderMana.value = info.mana;
        scoreText.text = info.score + "";
        Debug.Log("score: " + info.score);
    }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        info = new PlayerInfo
        {
            health = (int)sliderHealth.value,
            mana = (int)sliderMana.value,
            score = 0
        };
    }
    //void Update()
    public void Update()
    {
        if (HasInputAuthority)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                int currenthealth = info.health;
                int currentmana = info.mana;
                int currentscore = info.score;
                info = new PlayerInfo
                {
                    health = currenthealth - 10,
                    mana = currentmana - 20,
                    score = currentscore + 30,
                };
                Slash = !Slash;
            }
        }
    }
}
