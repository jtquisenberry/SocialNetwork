using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetworkGraph.Models
{
    public class MyEdge
    {
        public Data data = new Data();
        public Style style = new Style();

        public MyEdge()
        {

        }
        

        public class Data
        {
            public string id = "myID";
            //public string label = "myLabel";
            public long source = 666;
            public long target = 777;
            public string group = "myGroup";
        }

        public class Style
        {
            //public int width = 20;
            //public int height = 20;
            public string font_size = "8px";
            public string label = "myLabel";
        }

    }
}