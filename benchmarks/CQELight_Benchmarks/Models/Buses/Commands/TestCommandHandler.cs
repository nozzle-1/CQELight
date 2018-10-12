﻿using CQELight.Abstractions.CQS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQELight_Benchmarks.Models
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public async Task HandleAsync(TestCommand command, ICommandContext context = null)
        {
            if(command.SimulateWork)
            {
                await Task.Delay(command.I % command.JobDuration); //Simulation of max 500ms job here
            }
        }
    }
}