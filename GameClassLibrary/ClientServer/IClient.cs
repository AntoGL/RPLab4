using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary.ClientServer
{
    public interface IClient
    {
        /// <summary>
        /// Сообщает клиенту о начале хода
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Turn(Guid sessionGuid);

        /// <summary>
        /// Проверяем жив ли клиент
        /// </summary>
        [OperationContract]
        bool Ping();

        [OperationContract]
        string GetUserName();
    }
}
