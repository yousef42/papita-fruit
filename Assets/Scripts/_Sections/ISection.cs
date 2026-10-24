using UnityEngine;
using System.Collections;
using System;

public interface ISection
{
    Action onSectionCompleted { get; set; }

    void EnableSection(Action onSectionComplete);
    void DisableSection();
    void OnPressed(Vector2 position);
    void OnUpdated(Vector2 position);
    void OnReleased(Vector2 position);
    
}
