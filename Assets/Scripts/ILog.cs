using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ILog
{
    void NotifyTuned(int channel, string frequency, int noise);
    void NotifyReady(int channel);
    void Send(int channel, string sentence);
    void Ask(int channel, string[] options);
}
