using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Transform pfDamagePopup, Vector3 position, int damageAmount, bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(pfDamagePopup, position, quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount,isCriticalHit );
        return damagePopup;
    }
    private static int sortingOrder;

    private TextMeshPro textMesh;
    private Color textColor;

    private float dissapearTime;
    private const float DISSAPEAR_TIMER_MAX = 1f;
    private Vector3 moveVector;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();

    }
    public void Setup(int damageAmount, bool isCriticalHit)
    {

        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            // normal hit
            textMesh.fontSize = 5;
            textColor = Color.yellow;
        }
        else
        {
            //critical hit
            textColor = Color.red;
            textMesh.fontSize = 6;

        }

        // textColor = textMesh.color;
        textMesh.color = textColor;
        dissapearTime = DISSAPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(1, 1) * 5f;

    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;
        if (dissapearTime > DISSAPEAR_TIMER_MAX * .5f)
        {
            // first half popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;

        }
        else
        {
           // second half popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
 
        }
        dissapearTime -= Time.deltaTime;

        if (dissapearTime < 0)
        {
            // start dissapearing
            float dissapearSpeed = 1.4f;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    
}
