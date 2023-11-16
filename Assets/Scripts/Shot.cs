using UnityEngine;

public class Shot : MonoBehaviour
{
    private float shotVelocity = 4f;
    
    private void Update()
    {
        if (this.gameObject.tag.Contains("NormalShot"))
        {
            transform.Translate(Vector3.right * shotVelocity * Time.deltaTime);
        }
        else if (this.gameObject.tag.Contains("ChargedShot"))
        {
            transform.Translate(Vector3.right * (shotVelocity * 1.5f) * Time.deltaTime);
        }
    }
}
