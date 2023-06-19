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

            //Set current position
            Vector2 pos = transform.position;
            //Calculate how much to fall
            float fallAmount = fallSpeed * Time.deltaTime;
            //Set new position
            pos.y -= fallAmount;


            //If player exists, move the player position based on the ground position's y axis
            if (player != null)
            {
                player.groundHeight -= fallAmount;
                Vector2 playerPosition = player.transform.position;
                playerPosition.y -= fallAmount;
                player.transform.position = playerPosition;
            }

            transform.position = pos;

            //Move object position based on ground position on the y axis
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
