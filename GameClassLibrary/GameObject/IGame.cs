using System;

namespace GameClassLibrary.GameObject
{
    interface IGame
    {
        GameState GetGameState { get; }
        void MakeTurn(Guid PlayerGuid, int x, int y);
        Guid[] GetPlayersGuids { get; }
        Guid GetWinerGuid { get; }
        Guid GetCurrentPlayerGuid { get; }
        string GetMapString { get; }
        string GetQestion { get; }
        void SetAnswer(Guid playerGuid, int playerAnswer);
        void EndGame();
    }
}
