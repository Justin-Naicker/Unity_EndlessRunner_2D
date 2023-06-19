using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;
    public float groundHeight;
    public float rightOfGround;
    public float rightOfScreen;
    BoxCollider2D boxCollider;
    public bool hasGroundGenerated = false;

    public Obstacle obstacle;
    public Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        hasGroundGenerated = false;
        boxCollider = GetComponent<BoxCollider2D>();
        //Get edge of camera
        rightOfScreen = Camera.main.transform.position.x * 2;
    }

    private void Update()
    {
        //The players current height + half of the ground height to account for the origin
        groundHeight = transform.position.y + (boxCollider.size.y / 2);

        animator = GameObject.Find("Main Camera").GetComponent<Animator>();

        if (gameObject.GetComponent<GroundFall>() != null)
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
            LayerMask groundMask = LayerMask.GetMask("Ground");
            if ((player.transform.position.x > transform.position.x - collider.bounds.size.x) &&
            (player.transform.position.x < transform.position.x + collider.bounds.size.x))
            {
                animator.SetBool("CameraShake", true);
            }
        }
        else
        {
            animator.SetBool("CameraShake", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Players current position
        Vector2 pos = transform.position;
        //Ground generated is travelling towards the player at the same velocity
        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        //Same as groundHeight
        rightOfGround = transform.position.x + (boxCollider.size.x / 2);

        //If ground goes behind the camera, destroy this object
        if (rightOfGround < 0)
        {

            Destroy(gameObject);
            return;
        }

        //If ground object has not been generated and the previous ground is past the treshold of the screen, generate a new one
        if (!hasGroundGenerated)
        {
            if (rightOfGround < rightOfScreen)
            {
                GenerateGround();
                hasGroundGenerated = true;
            }
        }

        //Sets position of the ground which is moving towards the player
        transform.position = pos;
    }

    void GenerateGround()
    {

        //Instantiate new copy of the ground
        GameObject groundObject = Instantiate(gameObject);
        BoxCollider2D groundCollider = groundObject.GetComponent<BoxCollider2D>();
        Vector2 pos;

        //Maximum potential player can jump
        float horizontal = player.jumpVelocity * player.maximumJumpTime;
        float time = player.jumpVelocity / -player.gravity;
        //Kinematic to calculate the movement of an object in a parabolic arc (vt + (1/2)gt^2)
        float horizontalArc = player.jumpVelocity * time + (0.5f * (player.gravity * (time * time)));
        float maxJumpHeight = horizontal + horizontalArc;
        //Ground should be generated partial of max jump height of the player
        float maximumY = maxJumpHeight * 0.7f;
        //Set the maximum height to that of the current groundHeight
        maximumY += groundHeight;
        //Minimum ground height
        float minimumY = 1;
        //Generate a random y value between a minimum value and the partial value of the max jump height, or maximumY
        float generatedY = Random.Range(minimumY, maximumY);

        //Account for how large the ground object is on the y axis
        pos.y = generatedY - groundCollider.size.y / 2;

        //If the ground exceeds this height, reset it 
        if (pos.y > 1f)
            pos.y = 1f;


        float time1 = time + player.maximumJumpTime;
        float time2 = Mathf.Sqrt((2.0f * (maximumY - generatedY)) / -player.gravity);
        float totalTime = time1 + time2;
        float maxX = (totalTime * player.velocity.x);
        maxX *= 0.7f;
        maxX += rightOfGround;
        //Ground starts to be generated before the main camera
        float minX = rightOfScreen + 5;
        float generatedX = Random.Range(minX, maxX);

        pos.x = generatedX + groundCollider.size.x / 2;

        //Sets the position to the calculated values
        groundObject.transform.position = pos;

        Ground newGround = groundObject.GetComponent<Ground>();
        newGround.groundHeight = groundObject.transform.position.y + (groundCollider.size.y / 2);

        GroundFall fall = groundObject.GetComponent<GroundFall>();
        if (fall != null)
        {
            Destroy(fall);
            fall = null;
        }

        if (fall != null)
        {
            Destroy(fall);
        }
        if (Random.Range(0, 3) == 0)
        {

            fall = groundObject.AddComponent<GroundFall>();
            fall.fallSpeed = Random.Range(1f, 3f);
        }

        //Obstacle Generation
        int obstacleAmount = Random.Range(0, 4);
        for (int i = 0; i < obstacleAmount; i++)
        {
            GameObject obstacleObject = Instantiate(obstacle.gameObject);
            //Obstacles can only be generated at the current ground height, but is dynamic and will change with each new ground object
            float y = newGround.groundHeight;

            float width = groundCollider.size.x / 2 - 1;
            //Largest position to the left side where object can spawn
            float left = groundObject.transform.position.x - width;
            //Largest position to the right side where object can spawn
            float right = groundObject.transform.position.x + width;
            float x = Random.Range(left, right);
            //Sets variable to random values
            Vector2 objectPosition = new Vector2(x, y);
            //Assigns object position to variable holding random values
            obstacleObject.transform.position = objectPosition;

            if (fall != null)
            {
                Obstacle obj = obstacleObject.GetComponent<Obstacle>();
                fall.obstacles.Add(obj);
            }

        }

    }
}
