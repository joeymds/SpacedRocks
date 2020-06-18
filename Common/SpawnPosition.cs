using System;
using System.Collections.Generic;
using Godot;

namespace SpacedRocks.Common
{
    public class SpawnPosition
    {
        private int numberOfItems = 0;
        private readonly List<int> availableItems = new List<int>();
        
        public SpawnPosition(int NumberOfItems)
        {
            numberOfItems = NumberOfItems;
            for(var i = 0; i < numberOfItems; i++)
                availableItems.Add(i);
        }

        public int GetNextPositionIndex()
        {
            if (availableItems.Count == 0)
                return 0;
            
            Random random = new Random();
            var nextIndex = random.Next(0, availableItems.Count);
            GD.Print($"nextIndex = {nextIndex}");
            availableItems.RemoveAt(nextIndex);
            return nextIndex;
        }
    }
}