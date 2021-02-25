using System;

namespace TourBooker.Logic
{
    public class CountryCode
    {
        public string Value { get; }

        public CountryCode(string value)
        {
            this.Value = value;
        }

        public override string ToString() => Value;

        #region custom implementations to support equality
        // Must be present on every custom type
        public override bool Equals(object obj)
        {
            if (obj is CountryCode other)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(this.Value, other.Value);

            }
            return false;
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(this.Value);



        public static bool operator == (CountryCode lhs, CountryCode rhs)
        {
            if (lhs != null)
            {
                return lhs.Equals(rhs);
            }
            else
            {
                return rhs == null;
            }
        }

        public static bool operator !=(CountryCode lhs, CountryCode rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

    }
}