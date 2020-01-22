﻿using System;

namespace RestAirline.Web.Hypermedia
{
    public class Link<TResource>
    {
        public string Uri { get; set; }

        [Obsolete("For serialization")]
        public Link()
        {
            
        }

        public Link(string uri)
        {
            Uri = uri;
        }

        public override string ToString()
        {
            return Uri;
        }
    }
}