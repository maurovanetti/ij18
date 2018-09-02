using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Curtain
{
    None,
    StartGame,
    GameOver,
    Victory
}

public interface ICurtains
{
    void ChangeTo(Curtain curtain);
}
