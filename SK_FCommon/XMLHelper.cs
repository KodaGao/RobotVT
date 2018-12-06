using System;
using System.Data;
using System.IO;
using System.Xml;

namespace SK_FCommon
{/// <summary>
 /// XmlHelper 的摘要说明。
 /// xml操作类
 /// </summary>
    public class XmlHelper
    {
        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();

        public XmlHelper(string XmlFile)
        {
            // 
            // TODO: 在这里加入建构函式的程序代码 
            // 
            try
            {
                if (!System.IO.File.Exists(XmlFile))
                    CreatNull(XmlFile);
                objXmlDoc.Load(XmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }

        public void  CreatNull(string XmlFile)
        {
            XmlDocument doc = new XmlDocument(); // 创建dom对象
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("SmartKoda"); // 创建根节点SerialModbus
            doc.AppendChild(root);    //  加入到xml document
            ComputerHelper computertool=new ComputerHelper();
            XmlElement ComputerSN = doc.CreateElement("ComputerSN");
            ComputerSN.InnerText = computertool.CpuID;
            root.AppendChild(ComputerSN);
            XmlElement Language = doc.CreateElement("Language");
            Language.InnerText = "zh-CHS";
            root.AppendChild(Language);

            string xmlString = doc.OuterXml;
            doc.Save(XmlFile);
        }

        public DataTable GetData(string XmlPathNode)
        {
            //查找数据。返回一个DataView 
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            return ds.Tables[0];
        }
        /// <summary>
        /// 新节点内容。
        /// 示例：xmlTool.Replace("Book/Authors[ISBN=\"0002\"]/Content","ppppppp"); 
        /// </summary>
        /// <param name="XmlPathNode"></param>
        /// <param name="Content"></param>
        public void Replace(string XmlPathNode, string Content)
        {
            //更新节点内容。 
            objXmlDoc.SelectSingleNode(XmlPathNode).InnerText = Content;
        }
        public void Replace(string parentNodePath, string childNodeName, string Content)
        {
            try
            {
                XmlNode nodes = objXmlDoc.DocumentElement[parentNodePath][childNodeName];
                nodes.InnerText = Content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// <summary>
        /// 删除一个指定节点的子节点。 
        /// 示例： xmlTool.DeleteChild("Book/Authors[ISBN=\"0003\"]"); 
        /// </summary>
        /// <param name="Node"></param>
        public void DeleteChild(string Node)
        {
            //删除一个节点。 
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
        }

        /// <summary>

        ///  * 使用示列:
        ///  示例： XmlHelper.Delete( "/Node", "")
        ///  XmlHelper.Delete( "/Node", "Attribute")
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        public void Delete(string node, string attribute)
        {
            try
            {
                XmlNode xn = objXmlDoc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);

            }
            catch { }
        }
        
        /// <summary>
        /// 插入一节点和此节点的一子节点。 
        /// 示例：xmlTool.InsertNode("Book","Author","ISBN","0004"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="ChildNode">子节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Content">内容</param>
        public void InsertNode(string MainNode, string ChildNode, string Element, string Content)
        {
            //插入一节点和此节点的一子节点。 
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个节点，带一属性。
        /// 示例： xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Title","Sex","man","iiiiiiii"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Attrib">属性</param>
        /// <param name="AttribContent">属性内容</param>
        /// <param name="Content">元素内容</param>
        public void InsertElement(string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            //插入一个节点，带一属性。 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }
        /// <summary>
        /// 插入一个节点，不带属性。
        /// 示例：xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Content","aaaaaaaaa"); 
        /// </summary>
        /// <param name="MainNode">主节点</param>
        /// <param name="Element">元素</param>
        /// <param name="Content">元素内容</param>
        public void InsertElement(string MainNode, string Element, string Content)
        {
            //插入一个节点，不带属性。 
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }

        public string ReadElement(string Element)
        {
            string outPut = string.Empty;
            try
            {
                XmlNode nodes = objXmlDoc.DocumentElement[Element];
                outPut = nodes.InnerText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outPut;
        }

        public string ReadElement(string parentNodePath, string childNodeName)
        {
            string outPut = string.Empty;
            XmlNodeList elemList = objXmlDoc.GetElementsByTagName(parentNodePath);
            if (elemList.Count == 0)
            {
                outPut = "1";
                if (childNodeName == "FansFixedFirst")
                    outPut = DateTime.Now.ToString();
                if (childNodeName == "Hosttype")
                    outPut = "NA1000S";
                if (childNodeName == "Probetype")
                    outPut = "NA1013";
            }
            else
            {
                XmlNode nodes = objXmlDoc.DocumentElement[parentNodePath][childNodeName];
                outPut = nodes.InnerText;
            }
            return outPut;
        }

        public int FindElementExist(string parentNodePath)
        {
            XmlNodeList elemList = objXmlDoc.GetElementsByTagName(parentNodePath);
            return elemList.Count;           
        }

        /// <summary>
        /// 对xml文件做插入，更新，删除后需做Save()操作，以保存修改
        /// </summary>
        public void Save()
        {
            //保存文檔。 
            try
            {
                objXmlDoc.Save(strXmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            objXmlDoc = null;
        }
    }

    //========================================================= 

    //实例应用： 

    //string strXmlFile = Server.MapPath("TestXml.xml"); 
    //XmlControl xmlTool = new XmlControl(strXmlFile); 

    // 数据显视 
    // dgList.DataSource = xmlTool.GetData("Book/Authors[ISBN=\"0002\"]"); 
    // dgList.DataBind(); 

    // 更新元素内容 
     //xmlTool.Replace("Book/Authors[ISBN=\"0002\"]/Content","ppppppp"); 
    // xmlTool.Save(); 

    // 添加一个新节点 
    // xmlTool.InsertNode("Book","Author","ISBN","0004"); 
    // xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Content","aaaaaaaaa"); 
    // xmlTool.InsertElement("Book/Author[ISBN=\"0004\"]","Title","Sex","man","iiiiiiii"); 
    // xmlTool.Save(); 

    // 删除一个指定节点的所有内容和属性 
    // xmlTool.Delete("Book/Author[ISBN=\"0004\"]"); 
    // xmlTool.Save(); 

    // 删除一个指定节点的子节点 
    // xmlTool.Delete("Book/Authors[ISBN=\"0003\"]"); 
    // xmlTool.Save();
}
