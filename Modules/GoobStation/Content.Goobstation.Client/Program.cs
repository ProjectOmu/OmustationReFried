// See https://aka.ms/new-console-template for more information

using Robust.Client;

namespace Content.Goobstation.Client;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ContentStart.Start(args);
    }
}
