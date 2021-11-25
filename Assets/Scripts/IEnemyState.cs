using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void UpdateState();

    void ToInitialState();

    void ToNormalState();

    void ToChaseState();
}
