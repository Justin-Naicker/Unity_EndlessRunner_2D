using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{

    bool shouldFall = false;
    public float fallSpeed = 1f;

    public Player player;
    public List<Obstacle> obstacles = new List<Obstacle>();



    // Update is called once per frame
    void Update()
    {


        if (shouldFall)
        {
            Vector2 pos = transform.position;
            float fallAmount = fallSpeed * Time.deltaTime;
            pos.y -= fallAmount;




            if (player != null)
            {
                player.groundHeight -= fallAmount;
                Vector2 playerPosition = player.transform.position;
                playerPosition.y -= fallAmount;
                player.transform.position = playerPosition;
            }

            transform.position = pos;

            foreach (Obstacle obj in obstacles)
            {

                if (obj != null)
                {
                    Vector2 position = obj.transform.position;
                    position.y -= fallAmount;
                    obj.transform.position = position;

                }
            }
        }

        else
        {
            if (player != null)
            {
                shouldFall = true;
            }
        }
    }
}
