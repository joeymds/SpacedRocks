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
                NumberOfRocks = 1,
                NumberOfPowerUps = 2
            });
            levels.Add(new Level()
            {
                LevelNumber = 2, 
                LevelName = "Level02",
                NumberOfRocks = 2,
                NumberOfPowerUps = 2
            });
            levels.Add(new Level()
            {
                LevelNumber = 3, 
                LevelName = "Level03",
                NumberOfRocks = 3,
                NumberOfPowerUps = 3
            });
            levels.Add(new Level()
            {
                LevelNumber = 4, 
                LevelName = "Level04",
                NumberOfRocks = 4,
                NumberOfPowerUps = 3
            });
            levels.Add(new Level()
            {
                LevelNumber = 5, 
                LevelName = "Level05",
                NumberOfRocks = 5,
                NumberOfPowerUps = 3
            });
            levels.Add(new Level()
            {
                LevelNumber = 6, 
                LevelName = "Level06",
                NumberOfRocks = 6,
                NumberOfPowerUps = 3
            });
            levels.Add(new Level()
            {
                LevelNumber = 7, 
                LevelName = "Level07",
                NumberOfRocks = 7,
                NumberOfPowerUps = 4
            });
            levels.Add(new Level()
            {
                LevelNumber = 8, 
                LevelName = "Level08",
                NumberOfRocks = 8,
                NumberOfPowerUps = 4
            });
            levels.Add(new Level()
            {
                LevelNumber = 9, 
                LevelName = "Level09",
                NumberOfRocks = 9,
                NumberOfPowerUps = 4
            });
            levels.Add(new Level()
            {
                LevelNumber = 10, 
                LevelName = "Level10",
                NumberOfRocks = 10,
                NumberOfPowerUps = 4
            });
            levels.Add(new Level()
            {
                LevelNumber = 11, 
                LevelName = "Level11",
                NumberOfRocks = 11,
                NumberOfPowerUps = 5
            });
            levels.Add(new Level()
            {
                LevelNumber = 12, 
                LevelName = "Level12",
                NumberOfRocks = 12,
                NumberOfPowerUps = 5
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