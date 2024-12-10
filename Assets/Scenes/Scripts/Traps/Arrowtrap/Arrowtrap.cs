using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrowtrap : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] arrows;

    [Header ("Sounds")]
    [SerializeField] private AudioClip arrowFireSound;
    private float cooldownTimer;

    // Fires an arrow
    private void Attack() {
        cooldownTimer = 0;
        SoundManager.instance.PlaySound(arrowFireSound);
        arrows[FindArrow()].transform.position = firepoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    // Looks if an arrow is available
    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    // Update cooldowntimer & check if it should fire
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}
