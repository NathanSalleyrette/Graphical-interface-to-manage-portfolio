using System;

public class WarningWREException : Exception
{
    public WarningWREException()
    {
    }

    public WarningWREException(string message)
        : base(message)
    {
    }

    public WarningWREException(string message, Exception inner)
        : base(message, inner)
    {
    }
}