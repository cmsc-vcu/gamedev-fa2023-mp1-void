using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow_Control : MonoBehaviour
{
    private Transform ball;
    private Vector3 ballLocation;
    private Vector3 mousePosition;
    private Vector3 arrowTarget;
    private GameObject arrowHead;
    private Transform arrowHeadTransform;
    private LineRenderer arrow;
    private Rigidbody2D ballCollision;
    private float forceMultiplier = 150f;
    private Vector3 arrowOffset;
    private Vector2 flatArrowOffset;
    private Color arrowColor;
    private Vector3 startPosition;
    private int strikes = 0;
    private int score;
    private int par;
    private float scoreMultiplier = 1;
    private int highScore = 0;
    public TMP_Text scoreDisplay;
    public TMP_Text starBoostDisplay;
    private int stars = 0;

    // Start is called before the first frame update

    void Start()
    {
        ball = GetComponent<Transform>();
        arrowHead = GameObject.Find("Arrow_Head");
        arrowHeadTransform = arrowHead.GetComponent<Transform>();
        arrow = arrowHead.AddComponent<LineRenderer>();
        arrow.material = Resources.Load("Line_Base", typeof(Material)) as Material;
        arrow.startWidth = 0.2f;
        arrow.endWidth = 0.2f;
        ballCollision = GetComponent<Rigidbody2D>();
        arrowColor = arrowHead.GetComponent<SpriteRenderer>().color;
        arrow.startColor = arrowColor;
        arrow.endColor = arrowColor;
        startPosition = ball.position;
        par = 2;
        scoreDisplay = GameObject.Find("ScoreDisplay").GetComponent<TextMeshProUGUI>();
        starBoostDisplay = GameObject.Find("StarBoostDisplay").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BlackHole"))
        {
            GameObject blackHole = collision.gameObject;
            ballCollision.AddForce((blackHole.transform.position - ball.position) * 12f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlackHoleCenter"))
        {
            ball.position = startPosition;
            ballCollision.velocity = Vector3.zero;
            stars = 0;
            scoreMultiplier = 1;
            starBoostDisplay.SetText("Star Boost: x" + scoreMultiplier);
        }
        if (collision.CompareTag("Goal"))
        {
            startPosition = new Vector2(Random.Range(-17, 17), Random.Range(-8, 8));
            ball.position = startPosition;
            ballCollision.velocity = Vector2.zero;

            Vector2 newGoalPosition = new Vector2(Random.Range(-18, 18), Random.Range(-8, 8));
            collision.transform.position = newGoalPosition;
            score = 1000 + (par * 100) - (strikes * 100);
            if (strikes == 1)
            {
                scoreMultiplier *= 1.5f;
            }
            score = (int)(score * scoreMultiplier);

            if (score > highScore)
            {
                highScore = score;
            }
            scoreDisplay.SetText("High Score: " + highScore);
            scoreMultiplier = 1;
            resetStars();
            randomizeBlackHole();
            //randomizeAsteroids();
        }
        if (collision.CompareTag("Star"))
        {
            collision.gameObject.transform.GetComponent<Renderer>().enabled = false;
            stars++;
            scoreMultiplier++;
            starBoostDisplay.SetText("Star Boost: x" + scoreMultiplier);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ballLocation = ball.position;
        ballLocation.z = 0f;
        if (ballCollision.velocity == Vector2.zero)
        {
            if (Input.GetMouseButtonDown(0))
            {
                arrowHead.GetComponent<Renderer>().enabled = true;
                arrow.enabled = true;
            }
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                arrowOffset = -(mousePosition - ballLocation);
                arrowTarget = ballLocation + arrowOffset;

                arrowHeadTransform.position = arrowTarget;
                arrowHeadTransform.up = arrowOffset;

                arrow.positionCount = 2;
                Vector3 zOffset = new Vector3(0, 0, -0.1f);
                arrow.SetPosition(0, ballLocation + zOffset);
                arrow.SetPosition(1, arrowTarget + zOffset);
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                arrowHead.GetComponent<Renderer>().enabled = false;
                arrow.enabled = false;
                flatArrowOffset = arrowOffset;
                ballCollision.AddForce(flatArrowOffset * forceMultiplier);
                strikes++;
            }
            
        }
        if (ballCollision.velocity.magnitude < 1)
        {
            ballCollision.velocity = Vector2.zero;
        }
    }

    void resetStars()
    {
        stars = 0;
        GameObject starGroup = GameObject.Find("Stars");
        Transform[] starList = starGroup.GetComponentsInChildren<Transform>();
        foreach(Transform star in starList) 
        {
            star.position = new Vector2(Random.Range(-18, 18), Random.Range(-9, 9));
            star.GetComponent<Renderer>().enabled = true;
        }
        starGroup.transform.position = Vector2.zero;
    }
    void randomizeBlackHole()
    {
        GameObject blackHole = GameObject.FindGameObjectWithTag("BlackHoleCenter");
        blackHole.transform.position = new Vector2(Random.Range(-15, 15), Random.Range(-5, 5));

    }
    void randomizeAsteroids()
    {
        GameObject asteroidGroup = GameObject.Find("Asteroids");
        Transform[] asteroidList = asteroidGroup.GetComponentsInChildren<Transform>();
        foreach (Transform asteroid in asteroidList)
        {
            asteroid.position = new Vector2(Random.Range(-15, 15), Random.Range(-5, 5));
        }
        asteroidGroup.transform.position = Vector2.zero;
    }
}
