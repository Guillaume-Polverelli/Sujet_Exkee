using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputField : MonoBehaviour
{
    [SerializeField] private int _columNumber;

    private void OnMouseDown() 
    {
        GameManager.Instance.SelectColumn(_columNumber);
    }

    private void OnMouseOver()
    {
        GameManager.Instance.HoverColumn(_columNumber);
    }


}
