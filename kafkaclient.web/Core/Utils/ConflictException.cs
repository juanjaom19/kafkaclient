namespace kafkaclient.web.Core.Utils;

public class ConflictException : Exception
{
    public ConflictException() {  }

    public ConflictException(string desc)
        : base(String.Format("Invalid: {0}", desc))
    {

    }
}

