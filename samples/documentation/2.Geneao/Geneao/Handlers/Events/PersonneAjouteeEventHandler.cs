﻿using CQELight.Abstractions.Events.Interfaces;
using CQELight.Abstractions.IoC.Interfaces;
using Geneao.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Geneao.Handlers.Events
{
    class PersonneAjouteeEventHandler : IDomainEventHandler<PersonneAjoutee>, IAutoRegisterType
    {
        public Task HandleAsync(PersonneAjoutee domainEvent, IEventContext context = null)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine($"{domainEvent.Prenom} a correctement été ajouté(e) à la famille {domainEvent.NomFamille.Value}.");

            Console.ForegroundColor = color;

            return Task.CompletedTask;
        }
    }

}
