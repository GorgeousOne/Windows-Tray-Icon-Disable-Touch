using System.Windows.Forms;

namespace ToggleTouch.Commands
{
    public class NotifyCommand : BaseCommand
    {
        private readonly NotifyIcon _notifyIcon;

        public NotifyCommand(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public override void Execute(object parameter)
        {
            _notifyIcon.ShowBalloonTip(1000, "Toggle Touch", "Be sure to subscribe.", ToolTipIcon.Info);
        }
    }
}
