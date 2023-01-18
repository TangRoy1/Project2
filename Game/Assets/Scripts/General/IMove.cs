using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    List<Vector3> ArrayPos { get; set; }
    void StartMove();
}
