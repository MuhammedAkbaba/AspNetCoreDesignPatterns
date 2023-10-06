using Microsoft.AspNetCore.Mvc;

namespace WebAp.Command.Commands
{
    public class FileCreateInvoker
    {
        private ITableActionCommand _tableActionCommand;

        /// <summary>
        /// Birden fazla dosya için 
        /// örnek : Pdf,Excel,Json,XML ...
        /// </summary>
        private List<ITableActionCommand> _tableActionCommands = new List<ITableActionCommand>();

        public void SetCommand(ITableActionCommand tableActionCommand)
        {
            _tableActionCommand = tableActionCommand;
        }

        /// <summary>
        /// Birden fazla dosya oluşturmak istendiğinde
        /// Örnek : Excel, Pdf, Json ...
        /// </summary>
        /// <param name="tableActionCommand"></param>
        public void AddCommand(ITableActionCommand tableActionCommand)
        {
            _tableActionCommands.Add(tableActionCommand);
        }

        public IActionResult CreateFile()
        {
            ///Loglama yapabilirsin
            
            return _tableActionCommand.Execute();
        }

        /// <summary>
        /// Birden fazla dosya create etmek için 
        /// </summary>
        /// <returns></returns>
        public List<IActionResult> CreateFiles()
        {
            ///Loglama yapabilirsin


            //var list = new List<IActionResult>();
            //_tableActionCommands.ForEach(command =>
            //{
            //    list.Add(command.Execute());
            //});

            //return list;


            return _tableActionCommands.Select(x => x.Execute()).ToList();

        }

    }
}
