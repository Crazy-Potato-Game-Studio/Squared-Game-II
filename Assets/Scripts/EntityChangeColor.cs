using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChangeColor : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Material originalMaterial;
    private Coroutine flashRoutine;

    private void Start() {
        originalMaterial = spriteRenderer.material;
    }

    public void ChangeColor(float resistanceTime){
        if(flashRoutine != null){
            StopCoroutine(flashRoutine);
        }
        flashRoutine =  StartCoroutine(FlashRoutine(resistanceTime));
    }

    public IEnumerator FlashRoutine(float resistanceTime){
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(resistanceTime);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}
