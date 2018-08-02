﻿namespace OpenCensus.Tags
{
    using System;
    using OpenCensus.Utils;

    public sealed class TagValue : ITagValue
    {
        public const int MAX_LENGTH = 255;

        public string AsString { get; }

        internal TagValue(string asString)
        {
            if (asString == null)
            {
                throw new ArgumentNullException(nameof(asString));
            }

            this.AsString = asString;
        }

        public static ITagValue Create(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return new TagValue(value);
        }

        public override string ToString()
        {
            return "TagValue{"
                + "asString=" + AsString
                + "}";
        }

        public override bool Equals(object o)
        {
            if (o == this)
            {
                return true;
            }

            if (o is TagValue)
            {
                TagValue that = (TagValue)o;
                return this.AsString.Equals(that.AsString);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int h = 1;
            h *= 1000003;
            h ^= this.AsString.GetHashCode();
            return h;
        }

        private static bool IsValid(string value)
        {
            return value.Length <= MAX_LENGTH && StringUtil.IsPrintableString(value);
        }
    }
}
