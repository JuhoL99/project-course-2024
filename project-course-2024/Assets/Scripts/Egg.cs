using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Egg : MonoBehaviour, InteractInterface
{
    [SerializeField] private Slider slider;
    [SerializeField] int health = 100;
    public int maxHealth = 100;
    void Start()
    {
        slider.value = (health / (float)maxHealth) * 100;
    }
    public void GetInteracted(GameObject player)
    {
        //Check if player has egg food and heal if does
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        int eggNum = playerManager.nameToResourceNum["Egg Food"];
        if (playerManager.currentResources[eggNum] > 0)
        {
            ChangeHealth(20);
            playerManager.currentResources[eggNum]--;
        }
        else
        {
            print("You have no food for the egg!");
        }
    }
    public void ChangeHealth(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        slider.value = (health/(float)maxHealth)*100;
        if (health <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        print("Game Over");
    }
}