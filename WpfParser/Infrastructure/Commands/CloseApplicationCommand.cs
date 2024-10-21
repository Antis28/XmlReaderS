using System.Windows;
using WpfParser.Infrastructure.Commands.Base;

namespace WpfParser.Infrastructure.Commands
{
    internal class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
