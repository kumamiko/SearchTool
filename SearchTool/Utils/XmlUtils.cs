using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SearchTool.Utils
{
    public class XmlUtils
    {
        private string xmlPath;

        /// <summary>
        /// xml文件路径
        /// </summary>
        public string XmlPath
        {
            get { return xmlPath; }
            set { xmlPath = value; }
        }

        private XDocument _document = null;

        public bool Load()
        {
            if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath)) return false;

            _document = XDocument.Load(xmlPath);

            if (_document != null) return true; else return false;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath)) return false;

            try
            {
                _document.Save(xmlPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获得单个节点
        /// </summary>
        public XElement GetNode(string name)
        {
            return _document?.Descendants(name).FirstOrDefault();
        }

        /// <summary>
        /// 获得某个节点下的同级节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<XElement> GetNodes(string name)
        {
            return _document?.Descendants(name).Elements().ToList();
        }
    }


    public class SettingUtils : XmlUtils
    {
        public List<XElement> Settings = new List<XElement>();

        public SettingUtils(string settingFilePath, string name)
        {
            XmlPath = settingFilePath;
            if (Load()) GetSettings(name);
        }

        public void GetSettings(string name)
        {
            Settings = GetNodes(name);
        }

        public string GetSettingValue(string name)
        {
            return Settings.Find(t => t.Attribute("name").Value == name)?.Value ?? string.Empty;
        }

        public bool SetSettingValue(string name, string value)
        {
            try
            {
                Settings.Find(t => t.Attribute("name").Value == name).Element("value").Value = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
