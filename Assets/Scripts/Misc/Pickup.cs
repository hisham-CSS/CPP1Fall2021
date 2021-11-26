using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum CollectibleType
    {
        POWERUP = 0,
        SCORE = 1,
        LIFE = 2
    }

    [SerializeField] CollectibleType currentCollectible;
    public int ScoreValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player curPlayer = collision.gameObject.GetComponent<Player>();

            switch (currentCollectible)
            {
                case CollectibleType.POWERUP:
                    curPlayer.StartJumpForceChange();
                    break;
                case CollectibleType.SCORE:
                    GameManager.instance.score += ScoreValue;
                    break;
                case CollectibleType.LIFE:
                    Debug.Log("Life Collected");
                    GameManager.instance.lives++;
                    break;
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
