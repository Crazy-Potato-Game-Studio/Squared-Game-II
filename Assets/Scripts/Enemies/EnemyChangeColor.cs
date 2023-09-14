using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChangeColor : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;

    private void Start() {
        originalMaterial = spriteRenderer.material;
    }

    public void ChangeColor(){
        if(flashRoutine != null){
            StopCoroutine(flashRoutine);
        }

        flashRoutine =  StartCoroutine(FlashRoutine());
    }

    public IEnumerator FlashRoutine(){
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}
