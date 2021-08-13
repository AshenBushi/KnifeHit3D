#pragma warning disable 0649

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    
    private float offsetY = 2;
    private float speed = 5;
    private float minYPosition = 5;

    private Transform refTransform;

    private void Awake()
    {
        refTransform = GetComponent<Transform>();
    }

    public void Init()
    {
        refTransform.position = new Vector3(refTransform.position.x, target.position.y + offsetY, refTransform.position.z);
    }

    private void FixedUpdate()
    {
        if(refTransform.position.y > minYPosition)
            refTransform.position = Vector3.Lerp(refTransform.position, new Vector3(refTransform.position.x, target.position.y + offsetY, refTransform.position.z), UnityEngine.Time.fixedDeltaTime * speed);
    }
}
