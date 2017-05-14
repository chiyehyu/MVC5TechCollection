namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using MVC5Course.Models.Validation;

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product : IValidatableObject
    {
        [DisplayName("訂單數量")]
        public int OrderNum
        {
            get{
                //return OrderLine.Count;
                return OrderLine.Count(p => p.Qty > 400);//使用Linq，則Count可以變成函數
            }
        }

        // 利用IValidatableObject 按下 Ctrl+. 來選擇實作介面
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Price > 100 && this.Stock < 5)
            {
                //yield return new ValidationResult("Price & Stock Relationship Fail", new string[] {"Price","Stock"}); //第二個參數可以控制顯示的位置出現在那些欄位
                yield return new ValidationResult("Price & Stock Relationship Fail");
            }
            if (this.Price > 101 && this.Stock < 4)
            {
                //yield return new ValidationResult("Price & Stock Relationship Fail", new string[] {"Price","Stock"}); //第二個參數可以控制顯示的位置出現在那些欄位
                yield return new ValidationResult("Price & Stock Relationship2 Fail");
            }

            yield break;
        }
    }
    
    public partial class ProductMetaData
    {
        [Required(ErrorMessage = "請輸入商品名稱")]
        //[MinLength(3), MaxLength(30)]
        //[RegularExpression("(.+)-(.+)", ErrorMessage = "商品名稱格式錯誤")]
        [DisplayName("商品名稱")]
        [MyProductNameAttribute(ErrorMessage = "商品名稱須包含black")]
        public string ProductName { get; set; }
        [Required]
        //[Range(0, 9999, ErrorMessage = "請設定正確的商品價格範圍")]
        //[DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        [DisplayName("商品價格")]
        public Nullable<decimal> Price { get; set; }
        [Required]
        [DisplayName("是否上架")]
        public Nullable<bool> Active { get; set; }
        [Required]
        //[Range(0, 100, ErrorMessage = "請設定正確的商品庫存數量")]
        [DisplayName("商品庫存")]
        public Nullable<decimal> Stock { get; set; }
    }
}
