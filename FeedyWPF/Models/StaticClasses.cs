﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedyWPF.Models
{
    public static class StaticMethods
    {
        public static  bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
                return true;

            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
