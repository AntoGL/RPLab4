using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.GameObject;

namespace GameClassLibrary.ClientServer
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IServer           //Сервер
    {
        /// <summary>
        /// Пользоваетль получает свой ID по которому он будет распознаваться сервером
        /// </summary>
        /// <returns>Строка содержащая уникальный ключ</returns>
        [OperationContract()]
        Guid Start();

        /// <summary>
        /// Пользователь завершается сессию
        /// </summary>
        /// <param name="id">Id Пользователя</param>
        [OperationContract()]
        OperationResult Stop(Guid id);

        /// <summary>
        /// Пользователь запрашивает игру
        /// </summary>
        /// <returns>Id Игры</returns>
        [OperationContract()]
        void GetGame(Guid userId);

        /// <summary>
        /// Пользователь указывает на выбранную им клетку
        /// </summary>
        /// <param name="sessionId">Id сессии в которой пользователь делает ход</param>
        /// <param name="userId">Id Пользователя</param>
        /// <param name="x">координата x</param>
        /// <param name="y">координата y</param>
        [OperationContract()]
        OperationResult MakeTurn(Guid sessionId, Guid userId, int x, int y);

        /// <summary>
        /// Получение текущего состояния карты в игре
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [OperationContract()]
        string GetGameMap(Guid sessionId);

        /// <summary>
        /// Получение текущего состояния карты в игре
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [OperationContract()]
        GameState GetGameState(Guid sessionId);

        /// <summary>
        /// Получение guid победителя если его нет, то вернется default
        /// </summary>
        /// <returns>guid победителя</returns>
        [OperationContract]
        Guid GetWinerGuid(Guid Session);

        [OperationContract]
        string GetQuestion(Guid idSession);

        [OperationContract]
        void SetAnswer(Guid idSession, Guid idPlayer, int answer);

        /*
        /// <summary>
        /// Проверяем жив ли Сервер
        /// </summary>
        [OperationContract()]
        OperationResult Ping();*/
    }
}
