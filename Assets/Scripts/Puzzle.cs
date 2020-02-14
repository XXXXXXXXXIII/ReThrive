using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a puzzle
public interface Puzzle
{
    void InitPuzzle();
    void StartPuzzle();
    void ResetPuzzle();
    void StopPuzzle();
    void DestroyPuzzle();    
}
