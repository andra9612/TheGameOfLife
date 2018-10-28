using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCommand : ICommand
{
    private GenerationReceiver receiver;

    public PlayCommand(GenerationReceiver r)
    {
        receiver = r;
    }

    public void Execute()
    {
        receiver.PlayLife();
    }
}
