using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetworkGraph.Models
{
    public class MyNode
    {

        public Data data = new Data();
        public Style style = new Style();


        public MyNode()
        {

        }



        public class Data
        {
            public string id = "myID";
            public string label = "myLabel";
            public string group = "myGroup";
        }

        public class Style
        {
            public string width = "20";
            public string height = "20";
            public string font_size = "8px";
        }












    }
}