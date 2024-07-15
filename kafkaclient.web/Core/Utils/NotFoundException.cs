namespace kafkaclient.web.Core.Utils;

public class NotFoundException : Exception
{
    public NotFoundException() {  }

    public NotFoundException(string desc)
        : base(String.Format("Invalid: {0}", desc))
    {

    }
}