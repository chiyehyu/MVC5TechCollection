using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MVC5Course.Models.Validation
{
    public class MyProductNameAttribute : DataTypeAttribute
    {
        //使用ctor + tab + tab 來自動產生建構子
        public MyProductNameAttribute() : base(DataType.Text)
        {

        }

        //使用override + 空格 來自動產生覆寫函數
        public override bool IsValid(object value)
        {
            var str = (string)value;
            return str.Contains("black");
        }
    }
}