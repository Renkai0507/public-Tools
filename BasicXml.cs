using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Tools;

namespace Tools
{
    public class BasicXml<T>  where T:new () 
    {
         XDocument document;
        string XmlPath = System.Environment.CurrentDirectory + $@"\Xmls\{typeof(T).Name}.xml";
        string rootstr;
        IEnumerable<XElement> root;        
        public string ErrorMsg { get; private set; }
        public BasicXml()
        {
            document = new XDocument();
            rootstr = typeof(T).Name;
            if (Directory.Exists(System.Environment.CurrentDirectory + @"\Xmls") == false)
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\Xmls");
            }
            if (!IOhelper.FileExists(XmlPath))
            {
                document.Declaration = new XDeclaration("1.0", "UTF-8", "");
                //頂層root
                XElement root = new XElement(rootstr + "s", "");
                //計數自動儲存用
                document.Add(root);
                document.Save(XmlPath);
            }
            else 
            {
                try
                {
                    document = XDocument.Load(XmlPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            root = from c in document.Elements()
                   select c;
            
        }
        /// <summary>
        /// Model有減去欄位可用這個方法修正XML檔案
        /// </summary>
        public void RemoveXmlElement()
        {
            //var Xmls = root.Descendants(rootstr);
            //T entity = new T();
            //string[] propertys = new string[entity.GetType().GetProperties().Length];
            //for (int i = 0; i < propertys.Length; i++)
            //{
            //    propertys[i] = entity.GetType().GetProperties()[i].Name;
                
            //}
            //var Element = Xmls.Descendants(propertys[i]);
            //var propertyXml = Element.Nodes().ToList();
        }
        /// <summary>
        /// Model有增加欄位可用這個方法修正XML檔案
        /// </summary>
        public void AddXmlElement()
        {
            var Xmls = root.Descendants(rootstr);
            T entity = new T();
            string[] propertys = new string[entity.GetType().GetProperties().Length];
            for (int i = 0; i < propertys.Length; i++)
            {
                propertys[i] = entity.GetType().GetProperties()[i].Name;
            }
            for (int i = 0; i < Xmls.Count(); i++)
            {
                var Element = Xmls.ElementAt(i);
                for (int j = 0; j < propertys.Length; j++)
                {
                    var abc= Element.Nodes().ToList();
                    if (Element.Descendants(propertys[j]).Count() == 0)
                    {
                        Element.Add(new XElement(propertys[j], ""));

                        document.Save(XmlPath);
                    }
                }
            }
        }
        public List<T> GetAll()
        {
            if (root == null) return null;
            List<T> result = TurnXMLsToList();
            return result;
        }
        public List<T> Search(Expression<Func<T, bool>> expression)
        {
            if (root == null) return null;            
            List<T> result = TurnXMLsToList();
            return result.Where(expression.Compile()).ToList();
        }
        public bool Delete(T entity)
        {
            //先轉XML元件 再比對
            XElement entityXml = FindXElementByKey(entity);//            
            if (entityXml == null)
            {
                ErrorMsg = "找不到資料";
                return false;
            }                
            entityXml.Remove();            
            ErrorMsg = "";
            return true;
        }
        public bool Update( T entity)
        {
            //先轉XML元件 再比對
            XElement entityXml = FindXElementByKey(entity);//            
            if (entityXml == null)
            {
                ErrorMsg = "找不到資料";
                return false;
            } 
            foreach (var property in entity.GetType().GetProperties())
            {
                    entityXml.SetElementValue(property.Name, property.GetValue(entity).ToString());
            }
            ErrorMsg = "";
            return true;
        }
        public bool Update(Expression<Func<T, bool>> expression,T entity)
        {
            var exists= Search(expression);
            if (exists == null)
            {
                ErrorMsg = "找不到資料";
                return false;
            }
            //先轉XML元件 再比對
            XElement entityXml = FindXElementByKey(entity);//           
            if (entityXml == null) return false;
            foreach (var property in entity.GetType().GetProperties())
            {
                bool IsKey = false;
                var attrubuteArray = property.CustomAttributes.ToList();
                if (attrubuteArray != null && attrubuteArray.Count > 0)
                {
                    attrubuteArray.ForEach(X =>
                    {
                        if (X.AttributeType.Name == "KeyAttribute")
                            IsKey = true;
                    });
                }
                if (!IsKey)                
                    entityXml.SetElementValue(property.Name,property.GetValue(entity).ToString());
            }
            return true;
        }

        private XElement FindXElementByKey(T entity)
        {
            var result = root.Descendants(rootstr);
            foreach (var property in entity.GetType().GetProperties())
            {
                bool IsKey = false;
                var attrubuteArray = property.CustomAttributes.ToList();
                if (attrubuteArray != null&& attrubuteArray.Count>0)
                {
                    attrubuteArray.ForEach(X=> 
                    {
                        if(X.AttributeType.Name == "KeyAttribute")
                            IsKey=true;
                    });
                }
                if (IsKey)
                {
                    result = result.Where
                    (
                        X => X.Element(property.Name).Value == property.GetValue(entity).ToString()
                    );
                }                
            }
            return result.FirstOrDefault();
        }

        public bool Insert(T entity)
        {
            XElement entityXml = FindXElementByKey(entity);//            
            if (entityXml != null)
            {
                ErrorMsg = "已有相同KEY值";
                return false;
            }
            var element = new XElement(entity.GetType().Name);
            foreach (var Proper in entity.GetType().GetProperties())
            {
                element.Add(new XElement(Proper.Name,Proper.GetValue(entity)));
            }
            document.Root.Add(element);
            document.Save(XmlPath);
            ErrorMsg = "";
            return true;
        }
        private List<T> TurnXMLsToList()
        {
            List<T> result = new List<T>();
            foreach (var elemnt in root.Elements())
            {
                if (elemnt != null)
                {
                    T tt = new T();
                    foreach (var Proper in tt.GetType().GetProperties())
                    {
                        var vlu= elemnt.Element(Proper.Name).Value;
                        if (vlu!=null)
                        {
                            Proper.SetValue(tt, Convert.ChangeType(vlu, Proper.PropertyType) );
                        }
                    }
                    result.Add(tt);
                }
            }
            return result;
        }        
    }
}
