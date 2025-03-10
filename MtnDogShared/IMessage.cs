﻿namespace MtnDogShared
{
    internal interface IMessage<T>
    {
        public string Prefix { get; set; }

        public T DecodeMessage(string message);
    }
}
