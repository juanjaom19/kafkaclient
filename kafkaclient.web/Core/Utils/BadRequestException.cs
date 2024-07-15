namespace kafkaclient.web.Core.Utils;

public class BadRequestException : Exception
{
    public BadRequestException() {  }

    public BadRequestException(string desc)
        : base(String.Format("Invalid: {0}", desc))
    {

    }
}