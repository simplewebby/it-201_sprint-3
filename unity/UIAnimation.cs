using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAnimation : MonoBehaviour
{
    private bool isHidden = false;
    private IEnumerator coroutine;
    public float animTime = 2f;

    private Vector3 showPos, hidePos, startPos;
    public Button hideButton;




    private void Start()
    {
        showPos = Vector3.zero;
        hidePos = new Vector3(-6, 0f, 0f);


        if (!Application.isEditor)
        {
            transform.position = hidePos;
            isHidden = true;
            MoveUI();
        }
    }

    public void MoveUI()
    {
        if (isHidden)
        {
            coroutine = MovingUI(showPos);
            isHidden = true;

        }
        else
        {
            coroutine = MovingUI(hidePos);
            isHidden = true;
        }
        StartCoroutine(coroutine);
    }

    private IEnumerator MovingUI(Vector3 endPos)
    {
        float elapsedTime = 0f;
        startPos = transform.position;
        hideButton.interactable = false;

        while (Vector3.Distance(transform.position, endPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / animTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hideButton.interactable = true;
    }

}




