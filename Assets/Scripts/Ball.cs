using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Paddle paddle1;
    [SerializeField] private float xPush = 2f;
    [SerializeField] private float yPush = 15f;
    [SerializeField] private AudioClip[] ballSounds;
    [SerializeField] private float randomFactor = 0.2f;
    
    private Vector2 paddleToBallVector; 
    bool hasStarted; 
    
    private AudioSource myAudioSource;
    private GameSession myGameSession;
    private Rigidbody2D myRigidbody2D;
    
   // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myGameSession = FindObjectOfType<GameSession>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockToPaddle();    
            LaunchToMouseClick();    
        }
    }

    private void LaunchToMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidbody2D.velocity = new Vector2(xPush, yPush);
        }
    }

    private void LockToPaddle()
    {
        Vector2 paddlePosition = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePosition + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 velocityTweak = new Vector2
        (
            Random.Range(0f, randomFactor), 
            Random.Range(0f, randomFactor)
        );
        if (hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidbody2D.velocity += velocityTweak;
            myGameSession.gameSpeed += 0.005f;
            
        }
    }
}
