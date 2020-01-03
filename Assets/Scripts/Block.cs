using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    // config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX; //Visual Effect
    [SerializeField] int maxHits;
    [SerializeField] Sprite[] hitSprites;
    
    // cached reference
    Level level;

    //state variables
    [SerializeField] int timesHit; //TODO only serialized for debug.

    private void Start()
    {
        CountBrekableBlocks();
    }

    private void CountBrekableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
        {
            level.CountBlocks(); // //Tüm blocklar için bu script çalışacağından bu fonksiyon bize sahnedki toplam block sayısını verecek.

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         //OnCollisionEnter'ın altında fazla kod satırı yazmamak pratikler açısından önemlidir.Method haline çağırmak programı daha stabil halde çalıştıracaktır.
         if(tag == "Breakable")
        {
            HandleHit();

        }
    }

    private void HandleHit()
    {
        timesHit++;
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
        int spriteIndex = timesHit-1;
        GetComponent<SpriteRenderer>().sprite =hitSprites[spriteIndex];
    }

    private void DestroyBlock()
    {
        PlayBlockDestroySFX();
        Destroy(gameObject);
        TriggerSparklesVFX();
        level.BlockDestroyed();
        
    }

    private void PlayBlockDestroySFX()
    {
        FindObjectOfType<GameSession>().AddToScore();
        //Block destroy edilse bile pointClipAtPoint ile clip çalacak.
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position); //Ana kameranın olduğu yerde clibi oynat.
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX,transform.position,transform.rotation); //Block'un olduğu yer de oluş.
        Destroy(sparkles,1f);
    }
}
