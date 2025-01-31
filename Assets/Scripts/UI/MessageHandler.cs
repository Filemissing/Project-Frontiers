using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] TMP_Text textLabel;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] CanvasGroup nextPanelCanvasGroup;
    [SerializeField] RectTransform nextPanelRectTransform;

    [SerializeField] Image comicImageLabel;

    [SerializeField] float characterSpeed = .2f;
    [SerializeField] float lineSpeed = .5f;

    [SerializeField] float startPosition = -240;
    [SerializeField] float endPosition = 40;

    [SerializeField] Vector3 startScale = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] Vector3 endScale = new Vector3(1, 1, 1);
    [SerializeField] Vector3 clickScale = new Vector3(.95f, .95f, .95f);
    [SerializeField] Vector3 nextPanelStartScale = new Vector3(.7f, .7f, .7f);

    [SerializeField] float clickTransitionTime = .1f;
    [SerializeField] float nextPanelTransitionTime = .6f;

    public bool isDialogueVisible;
    bool canNext;
    Message currentDialogueMessage;


    [Header("Review")]
    [SerializeField] Transform reviewPanelParent;
    [SerializeField] GameObject reviewPanel;

    [SerializeField] float transitionTime = .3f;


    [Header("Dialogues")]
    public Message[] dialogues;


    [Header("Reviews")]
    public Message[] reviews;



    public void SayMessage(Message message, string customerName)
    {
        if (message.messageType == Message.MessageType.Dialogue)
        {
            if (!isDialogueVisible)
                isDialogueVisible = true;

            canNext = false; // Disable nextPanel

            currentDialogueMessage = message;
            nameLabel.text = message.name;
            comicImageLabel.sprite = message.comicSprite;
            //textLabel.text = message.text;

            StartCoroutine(ApplyTextWithEffect(message.text));
            IEnumerator ApplyTextWithEffect(string text)
            {
                string tempText = "";

                for (int i = 0; i < text.Length; i++)
                {
                    char character = text[i];
                    bool isLineEnd = character == '.' || character == '!' || character == '?' || character == ',' || character == '-';
                    float waitTime = isLineEnd ? lineSpeed : characterSpeed;

                    tempText += character;
                    textLabel.text = tempText;

                    yield return new WaitForSeconds(waitTime);
                }

                textLabel.text = text;
                canNext = true; // Enable nextPanel
            }
        }
        else if (message.messageType == Message.MessageType.Review)
        {
            GameObject review = Instantiate<GameObject>(reviewPanel);

            review.SetActive(true);
            review.transform.SetParent(reviewPanelParent);

            RectTransform rectTransform = review.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = review.GetComponent<CanvasGroup>();

            TMP_Text nameLabel = review.transform.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text textLabel = review.transform.GetChild(1).GetComponent<TMP_Text>();
            Image ratingLabel = review.transform.GetChild(2).GetChild(0).GetComponent<Image>();

            canvasGroup.alpha = 0;
            rectTransform.localScale = new Vector3(.1f, .1f, .1f);

            nameLabel.text = customerName;
            textLabel.text = message.text;
            ratingLabel.fillAmount = message.rating / 5f;

            canvasGroup.DOFade(1, transitionTime);
            rectTransform.DOScale(endScale, transitionTime);


            StartCoroutine(Wait(10, canvasGroup, rectTransform));
            IEnumerator Wait(float seconds, CanvasGroup canvasGroup, RectTransform rectTransform)
            {
                yield return new WaitForSeconds(seconds);

                canvasGroup.DOFade(0, 1);

                yield return new WaitForSeconds(1);

                rectTransform.DOSizeDelta(new Vector3(0, 0, 0), .2f);
                Destroy(review, .25f);
            }
        }
    }

    void Next()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canNext)
        {
            int nextDialogueMessageIndex = 0;
            for (int i = 0; i < dialogues.Length; i++)
            {
                Message message = dialogues[i];
                if (message.text == currentDialogueMessage.text)
                    nextDialogueMessageIndex = i + 1;
            }

            if (nextDialogueMessageIndex < dialogues.Length)
                SayMessage(dialogues[nextDialogueMessageIndex], "");
            else
            {
                isDialogueVisible = false;
                GameManager.instance.isDayCyling = true;
            }
            
            rectTransform.localScale = clickScale;
            rectTransform.DOScale(endScale, clickTransitionTime);
            canNext = false;
        }
    }

    bool previousCanNext = true;
    void CanNextChange() // Might cause bad performance due to tweening called many times, but there are no such warnings.
    {
        if (canNext == previousCanNext)
            return;

        if (canNext)
        {
            DOTween.Kill(nextPanelCanvasGroup);
            DOTween.Kill(nextPanelRectTransform);
            nextPanelCanvasGroup.DOFade(1, nextPanelTransitionTime);
            nextPanelRectTransform.DOScale(new Vector3(1, 1, 1), nextPanelTransitionTime);
        }
        else
        {
            DOTween.Kill(nextPanelCanvasGroup);
            DOTween.Kill(nextPanelRectTransform);
            nextPanelCanvasGroup.DOFade(0, 0);
            nextPanelRectTransform.DOScale(nextPanelStartScale, 0);
        }

        previousCanNext = canNext;
    }

    bool previousisDialogueVisible = false;
    void VisibilityChange(float transitionTime)
    {
        if (isDialogueVisible == previousisDialogueVisible)
            return;

        if (isDialogueVisible)
        {
            DOTween.Kill(rectTransform);
            DOTween.Kill(canvasGroup);
            rectTransform.DOMoveY(endPosition, transitionTime);
            rectTransform.DOScale(endScale, transitionTime);
            canvasGroup.DOFade(1, transitionTime);
        }
        else
        {
            DOTween.Kill(rectTransform);
            DOTween.Kill(canvasGroup);
            rectTransform.DOMoveY(startPosition, transitionTime);
            rectTransform.DOScale(startScale, transitionTime);
            canvasGroup.DOFade(0, transitionTime);
        }

        previousisDialogueVisible = isDialogueVisible;
    }

    void Update()
    {
        CanNextChange();
        VisibilityChange(transitionTime);
        Next();

        if (GameManager.instance.isEndlessMode) // Disables DayCycling on Endless Mode
            GameManager.instance.isDayCyling = false;
    }

    void Awake()
    {
        rectTransform.position = new Vector3(rectTransform.position.x, startPosition, rectTransform.position.z);
        rectTransform.localScale = startScale;
    }
}
