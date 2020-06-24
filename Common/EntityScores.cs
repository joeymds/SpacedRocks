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
                    return 18;
                case Rock.RockSizes.Medium:
                    return 9;
                case Rock.RockSizes.Small:
                    return 5;
                case Rock.RockSizes.Tiny:
                    return 2;
                default:
                    return 0;
            }
        }

        public int getOogShotDamage()
        {
            return 15;
        }
        
    }
}