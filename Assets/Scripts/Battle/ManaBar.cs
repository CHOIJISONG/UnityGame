using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    [SerializeField] GameObject mana;

    public void SetMana(float manaNormalized)
    {
        mana.transform.localScale = new Vector3(manaNormalized, 1f);
    }

    public IEnumerator SetMANASmooth(float newMANA)
    {
        float curMana = mana.transform.localScale.x;
        float changeAmt = curMana - newMANA;

        while (curMana - newMANA > Mathf.Epsilon)
        {
            curMana -= changeAmt * Time.deltaTime;
            mana.transform.localScale = new Vector3(curMana, 1f);
            yield return null;
        }
        mana.transform.localScale = new Vector3(newMANA, 1f);
    }
}
