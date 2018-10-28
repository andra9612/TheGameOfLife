using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreCommand : ICommand {

    private GenerationReceiver receiver;


    public PreCommand(GenerationReceiver r)
    {
        receiver = r;
    }

    public void Execute()
    {
        receiver.GetPreGeneration();
    }


}
