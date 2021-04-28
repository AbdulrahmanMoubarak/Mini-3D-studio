using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class Model
    {
        public String name;
        public int index, V_start_idx, V_count;
        
        public Model(String name, int index, int start, int count)
        {
            this.name = name;
            this.index = index;
            this.V_start_idx = start;
            this.V_count = count;
        }
        
    }
}
