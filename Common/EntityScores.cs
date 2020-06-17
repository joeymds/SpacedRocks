using Godot;

namespace SpacedRocks.Common
{
    public class EntityScores
    {
        public int getRockScore(Rock.RockSizes rockSize)
        {
            switch (rockSize)
            {
                case Rock.RockSizes.Large:
                    return 5;
                case Rock.RockSizes.Medium:
                    return 10;
                case Rock.RockSizes.Small:
                    return 15;
                case Rock.RockSizes.Tiny:
                    return 20;
                default:
                    return 0;
            }
        }

        public int getRockDamage(Rock.RockSizes rockSize)
        {
            switch (rockSize)
            {
                case Rock.RockSizes.Large:
                    return 25;
                case Rock.RockSizes.Medium:
                    return 14;
                case Rock.RockSizes.Small:
                    return 7;
                case Rock.RockSizes.Tiny:
                    return 3;
                default:
                    return 0;
            }
        }
        
        
    }
}