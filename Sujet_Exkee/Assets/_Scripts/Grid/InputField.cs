using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputField : MonoBehaviour
{
    [SerializeField] private int _columNumber;
    [SerializeField] private GameManager _gameManager;

    private void OnMouseDown() 
    {
        _gameManager.SelectColumn(_columNumber);
    }

    private void OnMouseOver()
    {
        _gameManager.HoverColumn(_columNumber);
    }


}
