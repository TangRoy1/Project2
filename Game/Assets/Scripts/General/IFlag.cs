using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlag
{
     TypeFlag TypeFlag { get; }
     void SetFlag(TypeFlag typeFlag);
}
