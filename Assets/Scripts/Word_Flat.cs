using UnityEngine;
using System.Collections;


public class Word_Flat : MonoBehaviour
{
    public WordType wordType;
    [HideInInspector] public bool isSolved { get; private set; } = false;
    [ReadOnly] public int wordID;
    [ReadOnly] public bool located = false;//وقتی مقیاسش بعد از قرار گیری روی سایه اش به اندازه عادیش رسید "درست" میشود


    [SerializeField] AudioClip wordAudio;

    public void SetMode(bool co)
    {
        isSolved = co;
    }

    public AudioClip GetWordAudio()
    {
        return wordAudio;
    }
}