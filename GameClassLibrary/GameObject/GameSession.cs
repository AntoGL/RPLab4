using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameClassLibrary.GameObject
{
    class GameSession : IGame
    {
        private int IndexWinerGuid;
        private int IndexCurrentPlayerGuid;
        private GameMap Map;
        private bool Cross;
        private int answer;
        private Dictionary<Guid,int> playersAnswer;
        private Timer timer;
        int num;
        Action<Guid> check;
        bool flag;
        Guid myGuid;

        public GameSession(Guid[] PlayersGuids, string question, int answer, Action<Guid> ch, Guid guid)
        {
            if (PlayersGuids.Length != 2)
                throw new ArgumentException($"Count players not equal 2\n\tCount players:\t{PlayersGuids.Length}");

            myGuid = guid;
            timer = new Timer(CheckPlayer, num, 0, 2000);
            GetPlayersGuids = PlayersGuids;
            playersAnswer = new Dictionary<Guid, int>();
            IndexCurrentPlayerGuid = 0;
            IndexWinerGuid = -1;
            GetGameState = GameState.Active;
            Cross = true;
            GetQestion = question;
            this.answer = answer;
            check = ch;

            Map = new GameMap(3);
        }

        private void CheckPlayer(object a)
        {
            if (flag)
                EndGame();
            check(myGuid);
            flag = true;
        }

        public GameState GetGameState { get; private set; }
        public Guid[] GetPlayersGuids { get; private set; }
        public Guid GetWinerGuid => IndexWinerGuid==-1? default(Guid):GetPlayersGuids[IndexWinerGuid];
        public Guid GetCurrentPlayerGuid => GetPlayersGuids[IndexCurrentPlayerGuid];
        public string GetMapString => Map.ToString();

        public string GetQestion { get; private set; }

        public void MakeTurn(Guid PlayerGuid, int x, int y)
        {
            flag = false;
            CheckTurn(PlayerGuid, x, y);

            Map[x, y] = Cross ? TypeCell.Cross : TypeCell.Zero;
            CheckEndGame();
            if (GetGameState == GameState.Active)
            {
                Cross = !Cross;
                IndexCurrentPlayerGuid = IndexCurrentPlayerGuid == GetPlayersGuids.Length - 1 ? 0 : IndexCurrentPlayerGuid + 1;
            }
            num = 0;
        }

        public void SetAnswer(Guid playerGuid, int playerAnswer)
        {
            if (GetGameState != GameState.Unknown)
                return;

            if (playersAnswer.ContainsKey(playerGuid))
                return;

            playersAnswer.Add(playerGuid, playerAnswer);

        }

        private void CheckEndGame()
        {
            //Простая реализация проверки карты
            if (Map[0, 0] != TypeCell.None && Map[0, 0] == Map[0, 1] && Map[0, 0] == Map[0, 2])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[1, 0] != TypeCell.None && Map[1, 0] == Map[1, 1] && Map[1, 0] == Map[1, 2])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[2, 0] != TypeCell.None && Map[2, 0] == Map[2, 1] && Map[2, 0] == Map[2, 2])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[0, 0] != TypeCell.None && Map[0, 0] == Map[1, 0] && Map[0, 0] == Map[2, 0])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[0, 1] != TypeCell.None && Map[0, 1] == Map[1, 1] && Map[0, 1] == Map[2, 1])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[0, 2] != TypeCell.None && Map[0, 2] == Map[1, 2] && Map[0, 2] == Map[2, 2])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[0, 0] != TypeCell.None && Map[0, 0] == Map[1, 1] && Map[0, 0] == Map[2, 2])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (Map[2, 0] != TypeCell.None && Map[2, 0] == Map[1, 1] && Map[2, 0] == Map[0, 2])
                IndexWinerGuid = IndexCurrentPlayerGuid;

            if (IndexWinerGuid == -1)
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (Map[i, j] == TypeCell.None)
                            return;
                    }
                }
            else
            {
                GetGameState = GameState.End;
                return;
            }

            GetGameState = GameState.Unknown;
        }

        private void CheckTurn(Guid PlayerGuid, int x, int y)
        {
            if (GetGameState == GameState.End)
                throw new ArgumentException("Player can not perform actions in the completed session.");

            //if (GetPlayersGuids.FirstOrDefault(g => g == PlayerGuid) != default(Guid))
            //    throw new ArgumentException("Player can not perform actions in this session.");

            if (PlayerGuid != GetCurrentPlayerGuid)
                throw new ArgumentException("Player can not perform actions, now the turn of another player.");

            if (Map[x,y] != TypeCell.None)
                throw new ArgumentException("Player can not select this cage, it is occupied");
        }

        public void EndGame()
        {
            IndexWinerGuid = IndexCurrentPlayerGuid == GetPlayersGuids.Length - 1 ? 0 : IndexCurrentPlayerGuid + 1;
            GetGameState = GameState.End;
        }
    }
}
