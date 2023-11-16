using UnityEngine;

public class Shot : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.right * 4f * Time.deltaTime);
    }
}
