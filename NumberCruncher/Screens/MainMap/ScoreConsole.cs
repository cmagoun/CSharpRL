using CsEcs;
using NumberCruncher.Components;
using SadSharp.Game;
using SadSharp.Helpers;
using System;

namespace NumberCruncher.Screens.MainMap
{
    public class ScoreConsole : GameConsole
    {
        private IGameData _gameData;
        Label _levelValue;
        Label _scoreValue;
        Label _refreshValue;
        Label _hitsValue;
        Label _turnValue;

        public override string MyKey => "SCORE_CONSOLE";

        public ScoreConsole(IGameData gameData) : base(18, 10, 0, 0)
        {
            _gameData = gameData;

            var levelLabel = new Label("Level", 0, 0);
            var turnLabel = new Label("Turn").Under(levelLabel, 1);
            var scoreLabel = new Label("Score").Under(turnLabel, 1);
            var refreshLabel = new Label("Refresh").Under(scoreLabel, 1);
            var hitLabel = new Label("Hits").Under(refreshLabel, 1);

            Children.Add(levelLabel);
            Children.Add(turnLabel);
            Children.Add(scoreLabel);
            Children.Add(refreshLabel);
            Children.Add(hitLabel);

            _levelValue = (Label)new Label("  0").RightOf(levelLabel, 3);
            _turnValue = (Label)new Label("  0").Under(_levelValue, 1);
            _scoreValue = (Label)new Label("  0").Under(_turnValue, 1);
            _refreshValue = (Label)new Label("  0").Under(_scoreValue, 1);
            _hitsValue = (Label)new Label("  0").Under(_refreshValue, 1);

            Children.Add(_levelValue);
            Children.Add(_turnValue);
            Children.Add(_scoreValue);
            Children.Add(_refreshValue);
            Children.Add(_hitsValue);
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var score = _gameData.Ecs.Get<ScoreComponent>(Program.Player);
            var hits = _gameData.Ecs.Get<HitPointComponent>(Program.Player);

            _levelValue.Text = _gameData.Level.Pad(3);
            _turnValue.Text = _gameData.Turn.Pad(3);
            _scoreValue.Text = score.Score.Pad(3);
            _refreshValue.Text = (score.Refresh % 10).Pad(3);
            _hitsValue.Text = hits.CurrentHits.Pad(3);

            base.Draw(timeElapsed);
        }
    }
}
