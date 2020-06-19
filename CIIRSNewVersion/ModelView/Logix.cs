using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CiirsDotnet.ModelView
{
    public class Logix
    {
        public Logix()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int get_brwrType(long sales, long Exposure, long Clnt_prof_type, int NofEmp)
        {
            if (sales > 2500000000 && Exposure > 300000000 && Clnt_prof_type == 1)
                return 1;
            else if (sales > 2500000000 && Exposure > 300000000 && Clnt_prof_type != 1)
                return 2;
            else if (sales <= 2500000000 && Exposure > 300000000)
                return 2;
            else if (sales > 2500000000 && Exposure <= 300000000)
                return 2;
            else if ((sales > 800000000 && sales <= 2500000000) || (Exposure > 200000000 && Exposure <= 300000000) || (NofEmp > 250))
                return 2;
            else if ((sales > 150000000 && sales <= 800000000) || (Exposure > 25000000 && Exposure <= 200000000) || (NofEmp > 50 && NofEmp <= 250))
                return 8;
            else if ((sales <= 150000000 || Exposure <= 25000000) && (NofEmp <= 50))
                return 3;
            else
                return 3;
        }
    }
}