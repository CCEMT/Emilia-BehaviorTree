using UnityEngine;

namespace Emilia.AI
{
    public class Player : MonoBehaviour
    {
        public float speed = 5.0f;

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0, vertical);
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}