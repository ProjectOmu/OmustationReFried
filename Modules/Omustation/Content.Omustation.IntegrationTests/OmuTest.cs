using Content.IntegrationTests.Fixtures;

namespace Content.Omustation.IntegrationTests;

[TestFixture]
public sealed partial class OmuTest : GameTest
{
    [Test]
    public async Task TestLoadAll()
    {
        Server.Log.Debug("Server: If you see this, test did launch!");
        Client.Log.Debug("Client: If you see this, test did launch!");
    }
}
