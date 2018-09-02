using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IEngine
{
    bool TuneTo(float frequency, int noise);
    bool Answer(int optionNumber);
}
