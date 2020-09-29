using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config params
    [SerializeField] private AudioClip[] breakSounds;
    [SerializeField] private GameObject blockSparklesVFX;
    [SerializeField] private Sprite[] hitSprites;

    // cached reference
    private Level _level;
    private GameSession gameSession;

    // state variables
    [SerializeField] private int timesHit; //TODO only serialized for debug purposes
    
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        _level = FindObjectOfType<Level>();
        if (CompareTag("Breakable"))
        {
            _level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag("Breakable"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];    
        }
        else
        {
            Debug.LogError("Block sprite is missing from " + gameObject.name);
        }
        
    }

    private void DestroyBlock()
    {
        gameSession.AddToScore();
        PlayBlockDestroySFX();
        TriggerSparklesVFX();
        Destroy(gameObject);
        _level.BlockDestroyed();
    }

    private void PlayBlockDestroySFX()
    {
        AudioClip soundEffect = breakSounds[UnityEngine.Random.Range(0, breakSounds.Length)];
        AudioSource.PlayClipAtPoint(soundEffect, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
    
}
