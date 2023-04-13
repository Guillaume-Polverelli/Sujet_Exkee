using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFieldPvP : MonoBehaviour
{
    [SerializeField] private int _columNumber;

    private void OnMouseDown()
    {
        GameManagerPvP.Instance.SelectColumn(_columNumber);
    }

    private void OnMouseOver()
    {
        GameManagerPvP.Instance.HoverColumn(_columNumber);
    }


}
