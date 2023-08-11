using System;
using System.Text;

namespace everyone
{
    /// <summary>
    /// An argument that is parsed from a command line.
    /// </summary>
    public class CommandLineArgument
    {
        private CommandLineArgument(string namePrefix, string name, string nameValueSeparator, string value)
        {
            if (namePrefix == null)
            {
                throw new ArgumentNullException(nameof(namePrefix));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (nameValueSeparator == null)
            {
                throw new ArgumentNullException(nameof(nameValueSeparator));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(namePrefix))
                {
                    throw new ArgumentException($"{nameof(name)} cannot be empty if {nameof(namePrefix)} is not empty.");
                }
                if (!string.IsNullOrEmpty(nameValueSeparator))
                {
                    throw new ArgumentException($"{nameof(name)} cannot be empty if {nameof(nameValueSeparator)} is not empty.");
                }
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException($"{nameof(name)} or {nameof(value)} must be non-empty.");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(namePrefix))
                {
                    throw new ArgumentException($"{nameof(namePrefix)} cannot be empty if {nameof(name)} is not empty.");
                }

                if (!string.IsNullOrEmpty(value) && string.IsNullOrEmpty(nameValueSeparator))
                {
                    throw new ArgumentException($"{nameof(nameValueSeparator)} cannot be empty if {nameof(name)} and {nameof(value)} are not empty.");
                }
            }

            this.NamePrefix = namePrefix;
            this.Name = name;
            this.NameValueSeparator = nameValueSeparator;
            this.Value = value;
        }

        /// <summary>
        /// Create a new <see cref="CommandLineArgument"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="CommandLineArgument"/>. This may be empty
        /// if the <see cref="CommandLineArgument"/> was a raw value and therefore didn't have a
        /// name.</param>
        /// <param name="value">The value of the <see cref="CommandLineArgument"/>. This may be
        /// empty if the <see cref="CommandLineArgument"/> was just a name, such as a switch/flag
        /// <see cref="CommandLineArgument"/>.</param>
        public static CommandLineArgument Create(string namePrefix, string name, string nameValueSeparator, string value)
        {
            return new CommandLineArgument(namePrefix, name, nameValueSeparator, value);
        }

        /// <summary>
        /// The prefix before this <see cref="CommandLineArgument"/>'s <see cref="Name"/>.
        /// </summary>
        public string NamePrefix { get; }

        /// <summary>
        /// The name of the <see cref="CommandLineArgument"/>. This may be empty if the
        /// <see cref="CommandLineArgument"/> was a raw value and therefore didn't have a name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="string"/> that separates the <see cref="Name"/> from the
        /// <see cref="Value"/>. This may be reduced to just a single space (' ') character if it
        /// was whitespace on the original command line.
        /// </summary>
        public string NameValueSeparator { get; }

        /// <summary>
        /// The value of the <see cref="CommandLineArgument"/>. This may be empty if the
        /// <see cref="CommandLineArgument"/> was just a name, such as a switch/flag
        /// <see cref="CommandLineArgument"/>.
        /// </summary>
        public string Value { get; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Name))
            {
                builder.Append(this.NamePrefix);
                builder.Append(this.Name);
                if (!string.IsNullOrEmpty(this.Value))
                {
                    builder.Append(this.NameValueSeparator);
                    builder.Append(this.Value);
                }
            }
            else
            {
                builder.Append(this.Value);
            }
            return builder.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is CommandLineArgument rhs &&
                this.NamePrefix.Equals(rhs.NamePrefix) &&
                this.Name.Equals(rhs.Name) &&
                this.NameValueSeparator.Equals(rhs.NameValueSeparator) &&
                this.Value.Equals(rhs.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Get(this.NamePrefix, this.Name, this.NameValueSeparator, this.Value);
        }
    }
}
