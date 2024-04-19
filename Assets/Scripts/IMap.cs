using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IMap
{
    void UpdateMap(int channel);
    bool MoveMarker(int channel, Vector2 position);
    Vector2 MarkerPosition(int channel);
}
