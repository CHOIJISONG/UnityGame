using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlighttedColor;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> moveTexts;
    [SerializeField] List<Image> moveImages;
    [SerializeField] Text manaText;

    public SpriteRenderer renderer;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedMove)
                moveTexts[i].color = highlighttedColor;
            else
                moveTexts[i].color = Color.white;
        }

        manaText.text = $"MP : {move.Mana}";
    }


    public void SetMovesName(List<Move> moves)
    {


        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
                moveImages[i].sprite = moves[i].Base.Image;
            }
            else
            {
                Color color = moveImages[i].color;
                color.a = 0f;

                moveTexts[i].text = "-";
                moveImages[i].color = color;
            }
        }
    }
}
