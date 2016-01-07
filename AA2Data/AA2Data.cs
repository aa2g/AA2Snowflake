using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Data
{
    public abstract class BaseData
    {
        public static implicit operator byte[] (BaseData x) => x.raw;
        public abstract int dataLength { get; }
        public abstract byte[] raw { get; set; }
    }

    public class AA2Data : BaseData
    {
#warning finish implementation
        public override int dataLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override byte[] raw
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }


}
