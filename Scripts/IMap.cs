using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IMap
{
    void UpdateMap(int channel);
    void MoveMarker(int channel, Vector2 position);
}
