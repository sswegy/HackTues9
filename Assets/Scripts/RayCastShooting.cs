using UnityEngine;


public class RayCastShooting : MonoBehaviour
{
    void FixedUpdate()
    {
        int layerMask = 1 << 8;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (Input.GetMouseButtonDown(0))
                if (Random.Range(0f, 5f) <= 2f)
                    Destroy(hit.collider.transform.parent.gameObject);
        }
    }
}
