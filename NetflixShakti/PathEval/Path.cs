using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.PathEval
{
    public enum PathType { Videos, Genres, Person, Seasons, Lolomos, Search };
    public enum VideoSpecifier { Trailers, Genres, Tags, Summary };

    public class Path
    {
        public PathType Type { get; private set; }
        public List<Object> Specifiers { get { return _specifiers; } }
        private List<Object> _specifiers = new List<Object>();

        public List<Object> Attributes { get { return _attributes; } }
        private List<Object> _attributes = new List<Object>();

        public Path(PathType type, params object[] specifiers)
        {
            Type = type;
            foreach (object spec in specifiers)
            {
                _specifiers.Add(spec);
            }
        }

        public void AddAttribute(object attr)
        {
            _attributes.Add(attr);
        }

        public void AddAttribute(params object[] attributes)
        {
            foreach (object attr in attributes)
            {
                _attributes.Add(attr);
            }
        }

        public void AddSpecifier(object spec)
        {
            _specifiers.Add(spec);
        }

        public void AddSpecifier(params object[] specifiers)
        {
            foreach (object spec in specifiers)
            {
                _specifiers.Add(spec);
            }
        }

        public object[] Build()
        {
            List<object> objs = new List<object>();
            objs.Add(Type);

            foreach (var spec in _specifiers)
            {
                objs.Add(spec);
            }

            objs.Add(Attributes);

            return objs.ToArray();
        }

        public static string GetSpecifier(VideoSpecifier spec)
        {
            return spec.ToString().ToLower();
        }

        public static object[][] BuildPathList(List<Path> paths)
        {
            List<object[]> jsonpaths = new List<object[]>();

            foreach (Path path in paths)
            {
                jsonpaths.Add(path.Build());
            }
            return jsonpaths.ToArray();
        }
    }
}

namespace NetflixShakti.Json.PathEval
{
    public class PathEvalRequest
    {
        public object[][] paths;

        public PathEvalRequest(object[][] paths)
        {
            this.paths = paths;
        }
    }
}