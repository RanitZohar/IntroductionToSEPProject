﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace BLAPI
{
    public static class BLFactory
    {
        public static IBl getBL()
        {
            return BL.BLimplementation.Instance;
        }
    }
}
