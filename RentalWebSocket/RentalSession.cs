using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperWebSocket;

namespace RentalWebSocket
{
    public class RentalSession:JsonWebSocketSession<RentalSession>
    {
        protected override void OnSessionStarted()
        {
            Log.Warn(this.SessionID + ",连接成功");
        }
        protected override void OnSessionClosed(SuperSocket.SocketBase.CloseReason reason)
        {
            Log.Warn(this.SessionID + ",断开连接");
        }
    }
}
