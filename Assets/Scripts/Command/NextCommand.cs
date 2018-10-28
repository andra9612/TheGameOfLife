using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCommand : ICommand
{
    private GenerationReceiver receiver;

    public NextCommand(GenerationReceiver r)
    {
        receiver = r;
    }

    public void Execute()
    {
        receiver.GetNextGeneration();
    }
}
