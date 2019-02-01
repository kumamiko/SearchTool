using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool.Service.Models
{
    public class Result
    {
        public string Keyword { get; set; }
        public string Word { get; set; }

        public List<Meaning> Meanings { get; set; }
    }

    public class Meaning
    {
        /// <summary>
        /// 读音相关
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        /// 解释
        /// </summary>
        public string Body { get; set; }
    }
}
