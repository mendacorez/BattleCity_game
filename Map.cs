﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity
{
    class Map
    {
        public int[,] GlobalMap { get; set; }
        public int[,] ShootMap { get; set; }
        public int[,] BotShootMap { get; set; }
        public Map()
        {
            // 1 - блок, 2 - вода, 5 - флаг с окончанием уровня, 0 - ничего, 3 - игрок, 6 - бот, 7 - неразрушаемый блок
            GlobalMap = new int[10, 10] { 
                                    {5, 1, 1, 1, 1, 1, 2, 2, 2, 2},
                                    {1, 1, 1, 1, 1, 1, 1, 2, 2, 2},
                                    {1, 1, 1, 1, 1, 1, 1, 1, 2, 2},
                                    {1, 1, 1, 1, 1, 1, 0, 0, 1, 2},
                                    {0, 0, 1, 1, 1, 1, 1, 0, 0, 1},
                                    {0, 0, 1, 1, 1, 1, 1, 0, 0, 0},
                                    {1, 1, 1, 0, 0, 1, 1, 1, 1, 1},
                                    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                    {2, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                                    {2, 2, 1, 1, 0, 0, 0, 1, 1, 0}

            };
            ShootMap = new int[10, 10] 
                                  { 
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            BotShootMap = new int[10, 10]
                      {
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
};
        }
    }
}
