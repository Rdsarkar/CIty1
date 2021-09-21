using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIty1.DTOs
{
    public class ResponseDto
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public object Payload { get; set; }
    }
}
