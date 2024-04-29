﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Response
{
    public class ListResponse<T> where T : class
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }
}
