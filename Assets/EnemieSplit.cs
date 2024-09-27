using UnityEngine;

public class EnemieSplit : MonoBehaviour
{
    public GameObject splitEnemy;

    public void Split()
    {        
        Instantiate(splitEnemy, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        Instantiate(splitEnemy, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    }
}
