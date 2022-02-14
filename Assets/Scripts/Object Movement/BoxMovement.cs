using UnityEngine;

namespace Object_Movement
{
    public class BoxMovement : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5.1f;
        [SerializeField] float acceleration = 0.03f;
        void Update()
        {
            if (GetComponent<Entanglable>().IsEntangled())  // Moves only if object is Entangled
            {
                if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && GetComponent<Rigidbody2D>().angularVelocity < maxSpeed)
                {
                    GetComponent<Rigidbody2D>().velocity += new Vector2(acceleration, 0.0f);
                }

                else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && GetComponent<Rigidbody2D>().angularVelocity > -maxSpeed)
                {
                    GetComponent<Rigidbody2D>().velocity -= new Vector2(acceleration, 0.0f);
                }
            }
        }
    }
}