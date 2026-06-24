using System;
using Microsoft.Extensions.DependencyInjection;
using QueueClient.ViewModels;

namespace QueueClient.Services;

/// <summary>Создаёт ViewModel'и через DI-контейнер (для навигации между экранами).</summary>
public interface IViewModelFactory
{
    T Create<T>() where T : ViewModelBase;
}

public class ViewModelFactory : IViewModelFactory
{
    private readonly IServiceProvider _provider;
    public ViewModelFactory(IServiceProvider provider) => _provider = provider;
    public T Create<T>() where T : ViewModelBase => _provider.GetRequiredService<T>();
}
