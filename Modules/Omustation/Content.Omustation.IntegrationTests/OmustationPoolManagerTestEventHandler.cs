// Notice, File contains code not licensed under MIT.
// see licenses.

using Content.Benchmarks;
using Content.IntegrationTests;

namespace Content.Omustation.IntegrationTests;

[SetUpFixture]
public sealed class OmustationPoolManagerTestEventHandler
{
    [OneTimeSetUp]
    public void Setup()
    {
        IntegrationTestHelpers.ChangeRootDir("../../../");
        PoolManagerHelpers.Setup();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        PoolManager.Shutdown();
    }
}
