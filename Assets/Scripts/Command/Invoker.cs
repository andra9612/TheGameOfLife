using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker {

    private ICommand command;

    public void SetCommand(ICommand cmd)
    {
        command = cmd;
    }

    public void Execute()
    {
        command.Execute();
    }
}
