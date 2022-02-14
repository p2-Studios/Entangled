using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Object_Movement
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] float maxRotationSpeed = 300.0f;
        [SerializeField] float acceleration = 2.0f;
        void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow) && GetComponent<Rigidbody2D>().angularVelocity > -maxRotationSpeed)
            {
                GetComponent<Rigidbody2D>().angularVelocity -= acceleration;
            }

            else if (Input.GetKey(KeyCode.LeftArrow) && GetComponent<Rigidbody2D>().angularVelocity < maxRotationSpeed)
            {
                GetComponent<Rigidbody2D>().angularVelocity += acceleration;
            }
        }
    }
}
