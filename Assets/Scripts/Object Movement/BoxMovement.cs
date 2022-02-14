using UnityEngine;

namespace Object_Movement
{
    public class BoxMovement : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5.1f;
        [SerializeField] float acceleration = 0.03f;
        void Update()
        {
            if (Input.GetKey(KeyCode.RightArrow) && GetComponent<Rigidbody2D>().angularVelocity < maxSpeed)
            {
                GetComponent<Rigidbody2D>().velocity += new Vector2(acceleration, 0.0f);
            }

            else if (Input.GetKey(KeyCode.LeftArrow) && GetComponent<Rigidbody2D>().angularVelocity > -maxSpeed)
            {
                GetComponent<Rigidbody2D>().velocity -= new Vector2(acceleration, 0.0f);
            }
        }
    }
}