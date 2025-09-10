using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public GameObject rangeUI;
    private Transform target;
    private Animator animator;
    public float rangeX = 10f;
    public float rangeZ = 10f;
    public float offsetX = 0f;
    public float offsetZ = 0f;
    private float flipSpeed = 20f;
    public string enemyTag = "Enemy";
    public string enemyTag2 = "Enemy";
    [Range(0f, 1f)]
    public float fireRate = 1f;
    public float fireCountdown = 0f;
    public bool flip = false;
    public GameObject sprite;
    public int damage = 50;
   
    private bool startCooldown = false;

    public Image healthBar;
    public GameObject healthUI;
    public float maxHealth = 100;
    public float health;

    public AudioSource attackSFX;
    public float flipDirection;

    void Start()
    {
        if (BuildManager.instance.isMapFlipped)
        {
            sprite.transform.localScale = new Vector3(-sprite.transform.localScale.x, sprite.transform.localScale.y, sprite.transform.localScale.z);
            rangeUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-rangeUI.GetComponent<RectTransform>().anchoredPosition.x, rangeUI.GetComponent<RectTransform>().anchoredPosition.y);
            offsetX = -offsetX;
        }

        flipDirection = sprite.transform.localScale.x;
        startCooldown = false;
        rangeUI.SetActive(false);
        health = maxHealth;
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    void Update()
    {
        if (GameManager.gameEnded)
        {
            animator.enabled = false;
            healthUI.SetActive(false);
            return;
        }

        if (GameManager.gamePaused)
        {
            healthUI.SetActive(false);
            return;
        }

        if (health < maxHealth)
        {
            healthUI.SetActive(true);
        }
        else
        {
            healthUI.SetActive(false);
        }

        if (startCooldown && fireCountdown > 0)
            fireCountdown -= Time.deltaTime;

        Flip();

        if (target == null)
            return;

        if (fireCountdown <= 0f)
        {
            startCooldown = false;

            if (flipDirection * (target.position.x - transform.position.x) < 0)
            {
                flip = true;
            }
            else   
            {
                flip = false;
            }

            animator.SetTrigger("attack");

            fireCountdown = 1f / fireRate;
        }
    }

    void Flip()
    {
        float targetScaleX = flip ? -flipDirection : flipDirection;
        float newScaleX = Mathf.MoveTowards(sprite.transform.localScale.x, targetScaleX, Time.deltaTime * flipSpeed);
        sprite.transform.localScale = new Vector3(newScaleX, sprite.transform.localScale.y, sprite.transform.localScale.z);
    }

    public void Attack()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            attackSFX.Play();
        }
        startCooldown = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        health = Mathf.Clamp(health, 0f, maxHealth);

        healthBar.fillAmount = health / maxHealth;

        if (health > 0)
        {
            animator.SetTrigger("hit");
        }

        if (health <= 0)
        {
            BuildManager.instance.DeselectNode();
            BuildManager.instance.PlayRetreatSFX();
            animator.SetTrigger("sell");
            healthUI.SetActive(false);
            GetComponent<Turret>().enabled = false;
            tag = "Untagged";
        }
    }

    public void Heal(int amount)
    {
        health += amount;

        health = Mathf.Clamp(health, 0f, maxHealth);

        healthBar.fillAmount = health / maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void UpdateTarget()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemyTag != enemyTag2)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            GameObject[] enemies2 = GameObject.FindGameObjectsWithTag(enemyTag2);

            List<GameObject> allEnemies = new List<GameObject>(enemies);
            allEnemies.AddRange(enemies2);

            Enemies = allEnemies.ToArray();
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in Enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;
            Vector3 towerPosition = transform.position + new Vector3(offsetX, 0f, offsetZ);

            float distanceX = Mathf.Abs(enemyPosition.x - towerPosition.x);
            float distanceZ = Mathf.Abs(enemyPosition.z - towerPosition.z);

            if (distanceX <= rangeX && distanceZ <= rangeZ)
            {
                float distanceToEnemy = Vector3.Distance(towerPosition, enemyPosition);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        target = (nearestEnemy != null && shortestDistance <= Mathf.Max(rangeX, rangeZ)) ? nearestEnemy.transform : null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(rangeX * 2, 2f, rangeZ * 2);
        Vector3 center = transform.position + new Vector3(offsetX, 0f, offsetZ);
        Gizmos.DrawWireCube(center, size);
    }
}
