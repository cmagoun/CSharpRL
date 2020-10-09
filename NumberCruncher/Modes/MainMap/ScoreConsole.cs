using CsEcs;
using NumberCruncher.Components;
using SadSharp.Game;
using SadSharp.Helpers;
using System;

namespace NumberCruncher.Modes.MainMap
{
    public class ScoreConsole : GameConsole
    {
        private IGameData _gameData;
        Label _levelValue;
        Label _scoreValue;
        Label _refreshValue;

       

        public override string MyKey => "SCORE_CONSOLE";

        public ScoreConsole(IGameData gameData):base(18, 10, 0, 0)
        {
            _gameData = gameData;

            var levelLabel = new Label("Level", 0, 0);
            var scoreLabel = new Label("Score").Under(levelLabel, 1);
            var refreshLabel = new Label("Refresh").Under(scoreLabel, 1);

            Children.Add(levelLabel);
            Children.Add(scoreLabel);
            Children.Add(refreshLabel);            

            _levelValue = (Label)new Label("  0").RightOf(levelLabel, 3);
            _scoreValue = (Label)new Label("  0").Under(_levelValue, 1);
            _refreshValue = (Label)new Label("  0").Under(_scoreValue, 1);

            Children.Add(_levelValue);
            Children.Add(_scoreValue);
            Children.Add(_refreshValue);
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var score = _gameData.Ecs.Get<ScoreComponent>(Program.Player);

            _levelValue.Text = _gameData.Level.Pad(3);
            _scoreValue.Text = score.Score.Pad(3);
            _refreshValue.Text = (score.Refresh % 10).Pad(3);

            base.Draw(timeElapsed);
        }
    }
}
