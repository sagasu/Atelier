using System.Collections.Generic;

namespace AtelierEntertainment
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public List<orderItem> Items { get; set; }
        public decimal Total { get; internal set; }
    }

    //Matt: Class names should start with capital letters
    //I would also extract this class to a separate file.
    public class orderItem
    {
        public string Code { get; set; }
        public string Description { get; set; }
        //Matt: Try not to use float, use Decimal whenever you can,
        //floating point arithmetic is a historical fallacy, from times when memory and hard drive space was expensive.
        public float Price { get; set; }
    }
}