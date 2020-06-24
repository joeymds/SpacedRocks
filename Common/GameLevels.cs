using System.Collections.Generic;

namespace SpacedRocks.Common
{
    public class GameLevels
    {
        private readonly List<Level> levels = new List<Level>();
        private int levelIndex = -1;

        public GameLevels()
        {
            InitLevels();
        }
        
        private void InitLevels()
        {
            levels.Add(new Level()
            {
                LevelNumber = 1, 
                LevelName = "Level01",
                NumberOfRocks = 0,
                NumberOfPowerUps = 4,
                NumberOfMonsters = 2,
                NumberOfShieldOrbs = 1
            });
            levels.Add(new Level()
            {
                LevelNumber = 2, 
                LevelName = "Level02",
                NumberOfRocks = 2,
                NumberOfPowerUps = 1,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 3, 
                LevelName = "Level03",
                NumberOfRocks = 3,
                NumberOfPowerUps = 2,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 1
            });
            levels.Add(new Level()
            {
                LevelNumber = 4, 
                LevelName = "Level04",
                NumberOfRocks = 3,
                NumberOfPowerUps = 3,
                NumberOfMonsters = 1,
                NumberOfShieldOrbs = 1
            });
            levels.Add(new Level()
            {
                LevelNumber = 5, 
                LevelName = "Level05",
                NumberOfRocks = 4,
                NumberOfPowerUps = 3,
                NumberOfMonsters = 1,
                NumberOfShieldOrbs = 2
            });
            levels.Add(new Level()
            {
                LevelNumber = 6, 
                LevelName = "Level06",
                NumberOfRocks = 4,
                NumberOfPowerUps = 3,
                NumberOfMonsters = 2,
                NumberOfShieldOrbs = 3
            });
            levels.Add(new Level()
            {
                LevelNumber = 7, 
                LevelName = "Level07",
                NumberOfRocks = 7,
                NumberOfPowerUps = 3,
                NumberOfMonsters = 2,
                NumberOfShieldOrbs = 2
            });
        }

        public void ResetLevels()
        {
            levelIndex = -1;
        }

        public Level GetNextLevel()
        {
            levelIndex++;
            return levels[levelIndex];
        }
    }
}