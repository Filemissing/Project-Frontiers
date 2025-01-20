using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FridgeInterface : MonoBehaviour
{
    public Fridge fridge;
    public GameObject buttonPrefab;

    private void Start()
    {
        for (int i = 0; i < fridge.ingredients.Length; i++)
        {
            Debug.Log(i);
            Button button = Instantiate(buttonPrefab, transform).GetComponent<Button>();
            int j = i; // store i in non changing variable so delaget void can use the unchanged value
            button.onClick.AddListener(delegate { fridge.SelectIngredient(j); });

            Image image = button.GetComponent<Image>();
            image.sprite = fridge.ingredients[i].icon;
        }
    }
}
