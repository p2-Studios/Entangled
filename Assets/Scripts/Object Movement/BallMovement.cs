using UnityEngine;

namespace Object_Movement
{
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] float maxRotationSpeed = 300.0f;
        [SerializeField] float acceleration = 4.0f;
        void Update()
        {
            if (GetComponent<Entanglable>().IsEntangled())  // Moves only if object is Entangled
            {
                if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && GetComponent<Rigidbody2D>().angularVelocity > -maxRotationSpeed)
                {
                    GetComponent<Rigidbody2D>().angularVelocity -= acceleration;
                }

                else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && GetComponent<Rigidbody2D>().angularVelocity < maxRotationSpeed)
                {
                    GetComponent<Rigidbody2D>().angularVelocity += acceleration;
                }
            }
        }
    }
}
