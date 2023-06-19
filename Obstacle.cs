using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Obstacle : MonoBehaviour
{
    Player player;
    Ground ground;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        ground = GetComponent<Ground>();

    }



    private void FixedUpdate()
    {
        //Obstacles current position
        Vector2 position = transform.position;

        //Will move towards the player with the same velocity as the player
        position.x -= player.velocity.x * Time.fixedDeltaTime;

        //If the obstacle moves off the screen, then destroy the object
        if (position.x < -100)
        {
            Destroy(gameObject);
        }

        //Set objects position to calculated position
        transform.position = position;
    }
}
