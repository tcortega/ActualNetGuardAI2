using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace NetGuardAI.App.Shared;

public class BlazorComponentBase : OwningComponentBase
{
    protected override void OnInitialized()
    {
        var properties = this.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<InjectScopedAttribute>() != null);

        foreach (var p in properties)
            p.SetValue(this, ScopedServices.GetService(p.PropertyType));
    }
}

/// <summary>
/// Indicates that the associated property should have a value injected from the
/// scoped service provider during initialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectScopedAttribute : Attribute
{
}