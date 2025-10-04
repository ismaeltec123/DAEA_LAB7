using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entity;

namespace Business
{
    public class BProduct
    {
        public List<Product> Read()
        {
            //Llamar a la capa de datos
            var products = (new DProduct()).Read();
            return products;
        }

        public void Create(Product product)
        {
            //Llamar a la capa de datos
            (new DProduct()).Create(product);
        }
    }
}
