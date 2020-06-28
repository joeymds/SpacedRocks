using System.Collections.Generic;

namespace SpacedRocks.Common
{
    public class GameLevels
    {
        private readonly List<Level> levels = new List<Level>();

        public int LevelIndex { get; set; }
        
        public GameLevels()
        {
            LevelIndex = -1;
            InitLevels();
        }
        
        private void InitLevels()
        {
            levels.Add(new Level()
            {
                LevelNumber = 1, 
                LevelName = "Minor Rubble",
                NumberOfRocks = 1,
                NumberOfPowerUps = 0,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 2, 
                LevelName = "More Rubble",
                NumberOfRocks = 2,
                NumberOfPowerUps = 0,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 3, 
                LevelName = "A Little Hope",
                NumberOfRocks = 3,
                NumberOfPowerUps = 1,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 4, 
                LevelName = "A Strange Thing",
                NumberOfRocks = 3,
                NumberOfPowerUps = 2,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 5, 
                LevelName = "Its Getting Worse",
                NumberOfRocks = 4,
                NumberOfPowerUps = 3,
                NumberOfMonsters = 0,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 6, 
                LevelName = "There's Two of Them!",
                NumberOfRocks = 2,
                NumberOfPowerUps = 2,
                NumberOfMonsters = 1,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 7, 
                LevelName = "This is getting tough.",
                NumberOfRocks = 3,
                NumberOfPowerUps = 3,
                NumberOfMonsters = 1,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 8, 
                LevelName = "Hang on",
                NumberOfRocks = 3,
                NumberOfPowerUps = 4,
                NumberOfMonsters = 2,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 9, 
                LevelName = "This is hard",
                NumberOfRocks = 3,
                NumberOfPowerUps = 4,
                NumberOfMonsters = 3,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 10, 
                LevelName = "Nightmare",
                NumberOfRocks = 5,
                NumberOfPowerUps = 5,
                NumberOfMonsters = 5,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 11, 
                LevelName = "Two Nightmares",
                NumberOfRocks = 4,
                NumberOfPowerUps = 5,
                NumberOfMonsters = 6,
                NumberOfShieldOrbs = 0
            });
            levels.Add(new Level()
            {
                LevelNumber = 12, 
                LevelName = "Pure Death",
                NumberOfRocks = 3,
                NumberOfPowerUps = 5,
                NumberOfMonsters = 8,
                NumberOfShieldOrbs = 0
            });
        }

        public void ResetLevels()
        {
            LevelIndex = -1;
        }

        public Level GetNextLevel()
        {
            LevelIndex++;
            return levels[LevelIndex];
        }

        public int NumberOfLevels()
        {
            return levels.Count;
        }
    }
}