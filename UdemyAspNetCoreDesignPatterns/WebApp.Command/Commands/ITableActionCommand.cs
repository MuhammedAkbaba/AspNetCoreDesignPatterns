using Microsoft.AspNetCore.Mvc;

namespace WebAp.Command.Commands
{
    /// <summary>
    /// UML deki Command  interface
    /// </summary>
    public interface ITableActionCommand
    {
        IActionResult Execute();
    }
}
