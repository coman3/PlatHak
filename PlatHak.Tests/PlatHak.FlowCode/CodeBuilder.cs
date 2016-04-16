using System.Text;

namespace PlatHak.FlowCode
{
    public class CodeBuilder
    {
        private StringBuilder _builder;
        private const string IndentValue = "    ";
        public int Indent { get; set; }
        private bool _appending;
        public CodeBuilder()
        {
            _builder = new StringBuilder();
        }

        private string GetIndent()
        {
            var item = "";
            for (var i = 0; i < Indent; i++)
            {
                item += IndentValue;
            }
            return item;
        }

        public void OpenBracket()
        {
            _builder.Append(GetIndent());
            Indent += 1;
            _builder.Append("{");
            NewLine();
        }
        public void CloseBracket()
        {
            Indent -= 1;
            _builder.Append(GetIndent());
            _builder.Append("}");
            NewLine();
        }

        public void NewLine()
        {
            _builder.AppendLine();
            _appending = false;
        }

        public void Append(string value, bool addSpace = true, bool addStartSpace = false)
        {
            if (!_appending)
            {
                _builder.Append(GetIndent());
                _appending = true;
            }
            if (addStartSpace) _builder.Append(" ");
            _builder.Append(value);
            if (addSpace) _builder.Append(" ");
        }

        public void AppendLine(string value)
        {
            if (!_appending) _builder.Append(GetIndent());
            _builder.Append(value);
            NewLine();
        }


        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}