using Blauhaus.DeviceServices.Abstractions.Thread;

namespace Blauhaus.MVVM.Blazor.Services;

public class BlazorThreadService : IThreadService
{
    public bool IsOnMainThread { get; } = true;

    public void InvokeOnMainThread(Action action)
    {
        action.Invoke();
    }

    public async void InvokeOnMainThread(Task task)
    {
        await task;
    }

    public async Task<T> InvokeOnMainThreadAsync<T>(Func<T> task)
    {
        return task.Invoke();
    }

    public Task InvokeOnMainThreadAsync(Action action)
    {
        action.Invoke();
        return Task.CompletedTask;
    }

    public async Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> task)
    {
        return await task.Invoke();
    }

    public async Task InvokeOnMainThreadAsync(Func<Task> task)
    {
        await task.Invoke();
    }

    public async Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync()
    {
        return null;
    }
}