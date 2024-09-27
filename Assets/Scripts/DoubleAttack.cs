using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoubleAttack : MonoBehaviour
{
    public Button doubleAttack;
    public Image buttonImage;
    public Color normalColor = Color.white;
    public Color cooldownColor = Color.gray;
    public Animator animator;
    public float cooldownTime = 2;
    public float Damage = 10;
    public float AttackRange = 2;
    public float AttackSpeed = 1;

    private bool isCooldown = false;

    private void Start()
    {
        buttonImage.color = normalColor;
        doubleAttack.onClick.AddListener(PerformDoubleAttack);
    }

    private void Update()
    {
        doubleAttack.interactable = !isCooldown;        
        

    }

    public void PerformDoubleAttack()
    {
        animator.SetTrigger("Double");

        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

        cooldownTime = Time.time;
        //closestEnemie.Hp -= Damage;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemie.transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }

        }

        if (closestEnemie != null)
        {
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= AttackRange)
            {
                if (Time.time - cooldownTime > AttackSpeed)
                {
                    transform.LookAt(closestEnemie.transform);
                    transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                    cooldownTime = Time.time;
                    closestEnemie.Hp -= Damage;
                    animator.SetTrigger("Double");

                }
            }
        }
        if (!isCooldown)
        {
            StartCoroutine(StartCooldown());
        }
    }
    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        float elapsed = 0;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            buttonImage.color = Color.Lerp(cooldownColor, normalColor, elapsed / cooldownTime);
            yield return null;
        }
        isCooldown = false;
    }
}
