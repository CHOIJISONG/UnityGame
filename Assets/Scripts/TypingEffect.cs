using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TMP_Text tx;
    private string tmp_text = "�׷��� ���� ������ ������ ����ϰ� ������ �ߴ�.";
    void Start()
    {
        StartCoroutine(_typing());
    }

    IEnumerator _typing()
    {

        for (int i = 0; i <= tmp_text.Length; i++)
        {
            tx.text = tmp_text.Substring(0, i); // text.Substring(������ġ, ����)

            yield return new WaitForSeconds(0.15f);
        }
    }
}
