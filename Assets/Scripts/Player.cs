using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float Hp = 50;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 1;
    public float moveSpeed = 5;
    public float attackCooldown = 1;
    private float lastAttackTime = 0;

    private bool isDead = false;
    public Animator AnimatorController;
    public Transform cameraTransform;
    public HealthBar healthBar;
    public UnityEvent stroke;

    private void Start()
    {
        healthBar.SetMaxHealth(Hp);
    }
    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            return;
        }

        Move();

        Attack();

        healthBar.SetHealth(Hp);
    }

    private void Attack()
    {
        var enemies = SceneManager.Instance.Enemies;
        Enemie closestEnemie = null;

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

        if (closestEnemie != null && Input.GetKeyDown(KeyCode.Space))
        {
            AnimatorController.SetTrigger("Attack");
            var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
            if (distance <= AttackRange)
            {
                if (Time.time - lastAttackTime > AtackSpeed)
                {
                    transform.LookAt(closestEnemie.transform);
                    transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                    lastAttackTime = Time.time;
                    closestEnemie.Hp -= Damage;
                    Hp += 4;
                    AnimatorController.SetTrigger("Attack");
                    stroke.Invoke();
                }
            }
        }
        //if (closestEnemie != null && Input.GetKeyDown(KeyCode.K))
        //{
        //    var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
        //    if (distance <= AttackRange)
        //    {
        //        if (Time.time - lastAttackTime > AtackSpeed)
        //        {
        //            transform.LookAt(closestEnemie.transform);
        //            transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

        //            lastAttackTime = Time.time;
        //            closestEnemie.Hp -= Damage;
        //            AnimatorController.SetTrigger("DoubleAttack");
        //            stroke.Invoke();
        //        }
        //    }
        //}
    }
    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveY).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
            AnimatorController.SetBool("run", true);
        }
        else
        {
            AnimatorController.SetBool("run", false);
        }
    }


    private void Die()
    {
        isDead = true;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }


}
