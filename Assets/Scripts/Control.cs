using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Control : MonoBehaviour
{

    public Sprite startBlock;
    public Sprite midBlock;
    public Sprite endBlock;

    private int completeLevels = 0;

    public void Start() { }

        private IEnumerator OnGeneratingRoutine()
        {
            Vector2 size = new Vector2(1, 1);
            Vector2 position = new Vector2(0, 0);

            GameObject newBlock = new GameObject("Start block");
            newBlock.transform.position = position;
            newBlock.transform.localScale = size;
            SpriteRenderer renderer = newBlock.AddComponent<SpriteRenderer>();
            renderer.sprite = this.startBlock;

            int count = this.completeLevels + 5;
            for (int i = 0; i < count; i++)
            {
                newBlock = new GameObject("Middle block");
                renderer = newBlock.AddComponent<SpriteRenderer>();
                renderer.sprite = this.midBlock;

                newBlock.transform.localScale = size;
                position.x += size.x;
                position.y += size.y * Random.Range(-1, 2);
                newBlock.transform.position = position;
                newBlock.transform.localScale = size;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }

        public void CompleteLevel()
        {
            this.completeLevels += 1;
            StartCoroutine(OnGeneratingRoutine());
        }

    }
