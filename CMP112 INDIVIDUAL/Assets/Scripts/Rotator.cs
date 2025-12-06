using UnityEngine;

public class Rotator : MonoBehaviour
{
    public int num1, num2, num3;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(num1, num2, num3) * Time.deltaTime);
    }
}
