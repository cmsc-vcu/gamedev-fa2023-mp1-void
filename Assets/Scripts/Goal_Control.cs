using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Goal_Control : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreDisplay;

    // Start is called before the first frame update

    private void Start()
    {
        scoreDisplay = GameObject.Find("scoreDisplay").GetComponent<TextMeshProUGUI>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 newStartPosition = new Vector2(Random.Range(-19, 19), Random.Range(-9, 9));
            collision.transform.position = newStartPosition;
            collision.attachedRigidbody.velocity = Vector2.zero;

            Vector2 newGoalPosition = new Vector2(Random.Range(-19, 19), Random.Range(-9, 9));
            transform.position = newGoalPosition;

            score++;
            scoreDisplay.SetText("Score: " + score);
        }
    }
}
