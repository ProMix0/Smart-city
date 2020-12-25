using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Control : MonoBehaviour
{

    public Material startBlock;
    public Material midBlock;
    public Material endBlock;

    private int completeLevels = 0;

    public void Start()
    {
        CompleteLevel();
    }
    void Update(){
    }


        private IEnumerator OnGeneratingRoutine()
        {
            Vector2 size = new Vector2(1, 1);
            Vector2 position = new Vector2(0, 0);

            GameObject newBlock = new GameObject("Start block");
            newBlock.transform.position = position;
            newBlock.transform.localScale = size;
            MeshRenderer renderer = newBlock.AddComponent<MeshRenderer>();
            renderer.material = this.startBlock;

            int count = this.completeLevels + 5;
            for (int i = 0; i < count; i++)
            {
                newBlock = new GameObject("Middle block");
                renderer = newBlock.AddComponent<MeshRenderer>();
                renderer.material = this.midBlock;

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