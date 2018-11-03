﻿using System;

namespace CheatTools
{
    internal class ReadonlyCacheEntry : CacheEntryBase
    {
        public readonly object Object;
        private readonly Type _type;
        private string _tostringCashe;

        public ReadonlyCacheEntry(string name, object obj) : base(name)
        {
            Object = obj;
            _type = obj.GetType();
        }

        public override object GetValueToCache()
        {
            return Object;
        }

        protected override bool OnSetValue(object newValue)
        {
            return false;
        }

        public override Type Type()
        {
            return _type;
        }

        public override bool CanSetValue()
        {
            return false;
        }

        public override string ToString()
        {
            return _tostringCashe ?? (_tostringCashe = Name() + " | " + Object);
        }
    }
}