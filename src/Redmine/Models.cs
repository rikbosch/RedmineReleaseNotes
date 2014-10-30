using System;

namespace ReleaseNotesGenerator.Redmine
{

    public class GetIssuesResponse
    {
        public Issue[] issues { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }

    public class Issue
    {
        public int id { get; set; }
        public Project project { get; set; }
        public Tracker tracker { get; set; }
        public Status status { get; set; }
        public Priority priority { get; set; }
        public Author author { get; set; }
        public Assigned_To assigned_to { get; set; }
        public Fixed_Version fixed_version { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public int done_ratio { get; set; }
        public Custom_Fields[] custom_fields { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public DateTime closed_on { get; set; }
        public Category category { get; set; }
        public Parent parent { get; set; }
    }

    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }

        protected bool Equals(Project other)
        {
            return this.id == other.id && string.Equals(this.name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Project)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.id * 397) ^ (this.name != null ? this.name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Project left, Project right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Project left, Project right)
        {
            return !Equals(left, right);
        }
    }

    public class Tracker
    {
        public int id { get; set; }
        public string name { get; set; }

        protected bool Equals(Tracker other)
        {
            return this.id == other.id && string.Equals(this.name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Tracker)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.id * 397) ^ (this.name != null ? this.name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Tracker left, Tracker right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tracker left, Tracker right)
        {
            return !Equals(left, right);
        }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Priority
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Assigned_To
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Fixed_Version
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }

        protected bool Equals(Category other)
        {
            return this.id == other.id && string.Equals(this.name, other.name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Category)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.id * 397) ^ (this.name != null ? this.name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Category left, Category right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Category left, Category right)
        {
            return !Equals(left, right);
        }
    }

    public class Parent
    {
        public int id { get; set; }
    }

    public class Custom_Fields
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class GetVersionResponse
    {
        public Version[] versions { get; set; }
        public int total_count { get; set; }
    }

    public class Version
    {
        public int id { get; set; }
        public Project project { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string sharing { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public string due_date { get; set; }
    }
}