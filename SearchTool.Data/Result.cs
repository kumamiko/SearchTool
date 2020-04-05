using System.Collections.Generic;

namespace SearchTool.Data
{
    public class Result
    {
        public string Keyword { get; set; }
        
        public List<Meaning> Meanings { get; set; }
    }

    public class Meaning
    {
        public string Info { get; set; }
        public string Word { get; set; }

        /// <summary>
        /// 读音
        /// </summary>
        public string Pronounce { get; set; }

        /// <summary>
        /// 解释
        /// </summary>
        public string Content { get; set; }
    }
}
