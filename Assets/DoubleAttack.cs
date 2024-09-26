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
    private float cooldownRemaining = 0;
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
        if (!isCooldown)
        {
            animator.SetTrigger("DoubleAttack");
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
