using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
using ThreadAlert.Entities;
using WebApplication6.Hubs;

namespace WebApplication6.SubscribeTableDependencies
{
    public class SubscribeNotificationTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Message> _tableDependency = null!;
        NotificationHub _notificationHub;

        public SubscribeNotificationTableDependency(NotificationHub notificationHub)
        {
            this._notificationHub = notificationHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            _tableDependency = new SqlTableDependency<Message>(connectionString, "Messages");
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();

        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Message)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<Message> e)
        {
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            {
                var notification = e.Entity;

                await _notificationHub.SendNotificationToClient(notification);
            }
        }
    }
}
