namespace Lambda3.WorkItemFieldHistory
{
    using System;

    public interface ITfsContext
    {
        event EventHandler ProjectChanged;

        string SelectedProject { get; }

        string ActiveConnection { get; }

    }
}