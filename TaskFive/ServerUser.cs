﻿using System.ServiceModel;

namespace TaskFive
{
    internal class ServerUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public OperationContext operationContext { get; set; }
    }
}
