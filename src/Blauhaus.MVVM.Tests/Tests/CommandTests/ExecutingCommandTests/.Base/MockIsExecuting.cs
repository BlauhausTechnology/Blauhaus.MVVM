using Blauhaus.MVVM.Abstractions.Commands;
using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.MVVM.Tests.Tests.CommandTests.ExecutingCommandTests.Base;

public interface IViewModel : IIsExecuting
{
    public IExecutingCommand Command { get; }
}


public class MockIsExecuting : MockBuilder<IViewModel>
{
    public Mock<IExecutingCommand> MockCommand = new();

    public MockIsExecuting()
    {
        With(x => x.Command, MockCommand.Object);
    }
}