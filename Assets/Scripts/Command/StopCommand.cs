using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCommand : ICommand {

    private GenerationReceiver receiver;

    public StopCommand(GenerationReceiver r)
    {
        receiver = r;
    }

    public void Execute()
    {
        receiver.StopGeneration();
    }
}
