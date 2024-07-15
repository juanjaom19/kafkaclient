namespace kafkaclient.web.Core.Utils;

public class UnexpectedException : Exception
{
    public UnexpectedException() {  }

    public UnexpectedException(string desc)
        : base(String.Format("Unexpected: {0}", desc))
    {

    }
}