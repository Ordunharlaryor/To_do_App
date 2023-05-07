using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace My_Todo_App
{
    public interface IDb
    {
        string ConnectionString { get; set; }
       
    }
}
