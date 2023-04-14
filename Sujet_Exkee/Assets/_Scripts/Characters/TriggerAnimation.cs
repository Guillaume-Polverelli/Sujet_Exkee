using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TriggerAnimation : MonoBehaviour
{

    [SerializeField] private Animator _animator;

    private float _time = 9.0f;

    private void Awake()
    {
        Assert.IsNotNull(_animator);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TriggerThinkingAnimation());
    }

    private IEnumerator TriggerThinkingAnimation()
    {
        while (true)
        {
            _animator.SetTrigger("IsThinking");
            print("Doing Stuff" + Time.time);
            yield return new WaitForSeconds(_time);
        }
    }
}
