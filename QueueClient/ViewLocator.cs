using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using QueueClient.ViewModels;

namespace QueueClient;

/// <summary>VM → View по имени: ...ViewModels[.X].YViewModel → ...Views[.X].YView.</summary>
public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        if (data is null)
            return new TextBlock { Text = "null" };

        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        return type is not null
            ? (Control)Activator.CreateInstance(type)!
            : new TextBlock { Text = "Не найдено: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
