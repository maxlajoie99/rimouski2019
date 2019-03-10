﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sailor_Actions : MonoBehaviour
{
    private GameObject particuleShoot;
    private Transform bulletInitPos;

    private int minLoot = 20;
    private int maxLoot = 100;

    private float cooldown = 0;
    private float lootRange = 7;

    public Sailor_Stats Stats { get; set; }


    private void Awake() {
        this.Stats = new Sailor_Stats();
        this.bulletInitPos = this.transform.GetChild(0).GetComponent<Transform>();

        this.particuleShoot = Resources.Load<GameObject>("FireEffect");
        if (this.particuleShoot == null) {
            Debug.Log("Couldn't load 'FireEffect' game object from ressources");
        }
    }

    private void Update() {
        if (this.cooldown > 0) {
            this.cooldown -= Time.deltaTime;
        }
    }
    

    public void Shoot() {
        if (this.cooldown > 0) {
            return;
        }
        this.cooldown = this.Stats.weaponCooldown;

        var anim = Instantiate(this.particuleShoot, this.bulletInitPos.position, this.bulletInitPos.rotation);
        Destroy(anim, 0.25f);

        var ray = Physics2D.Raycast(this.bulletInitPos.position, this.bulletInitPos.transform.up * 2);
        Debug.Log(ray.collider);
        if (ray.collider && ray.collider.tag == "Enemy") {
            var otherSailer = ray.transform.gameObject.GetComponent<Sailor_Actions>();
            if (otherSailer != null) {
                otherSailer.takeDamage(this.Stats.weaponDamage);
            }
        }
    }

    public void Loot(GameObject p_loot) {
        if (Vector2.Distance(this.transform.position, p_loot.transform.position) > this.lootRange) {
                return;
        }
        int lootAmount = Random.Range(this.minLoot, this.maxLoot);
        Debug.Log("You just looted " + lootAmount + " coins");

        Destroy(p_loot);
    }

    public void takeDamage(int p_damage) {
        this.Stats.HP -= p_damage;

        if (this.Stats.HP <= 0) {
            Destroy(this.gameObject);
        }
    }
}