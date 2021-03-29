using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddles.ResponseModels
{
    public class ApiResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Json { get; set; }
    }
}
