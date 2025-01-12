﻿using WarriorSalesAPI.Models;

namespace WarriorSalesAPI.Services
{
    public class TeamsService
    {
        public static List<Team> CreateSeedList()
        {
            List<Team> list = new();

            Team TeamPlatinum = new()
            {
                Name = "Team Platinum",
                Description = "A team defined by it's resilience.",
                LicensePlate = "PLA-71N4"
            };
            Team TeamDiamond = new()
            {
                Name = "Team Diamond",
                Description = "A team defined by it's thoughness.",
                LicensePlate = "DIA-M4N7"
            };
            Team TeamGold = new()
            {
                Name = "Team Gold",
                Description = "A team defined by it's flexibility.",
                LicensePlate = "OUR-0505"
            };

            list.Add(TeamPlatinum);
            list.Add(TeamDiamond);
            list.Add(TeamGold);

            return list;
        }
    }
}
