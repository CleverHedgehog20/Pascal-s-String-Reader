using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_auto
{
    class Analyzer
    {
        enum State
    {
        S, A1, A2, A3, A4, A5,
        C1, B1, B2, B22, B3,
        B4, B44, B5, B7, B8, Z, F
    };
 
        public static Tuple<string, string, int> create_tpl(string a, string b, int c)
        {
            var gof = new Tuple<string, string, int>(a, b, c);
            return gof;
        }
 
        public static string[] Parse(string s)
    {

            string a="", b="";
            int c=0, cnt=1, c1=0, c2=0;
            string up = s.ToUpper();
            int pos = 0;
            State status = State.S;
            string[] result = { "", "" };
            string id = "";
            string upid = "";
            string num = "";
            Int16 inum = 0;

            List<string> ident = new List<string>();
            List<int> nums = new List<int>();
            HashSet<string> names = new HashSet<string>();
            HashSet<Tuple<string, string, int>> res = new HashSet<Tuple<string, string, int>>();

            while (pos < s.Length)
        {
                switch (status)
                {
                    case State.S:
                        {
                            if (up[pos] == ' ')
                            {
                                pos++;
                            }
                            else if (up.Length - pos >= 3 && up[pos] == 'V' && up[pos + 1] == 'A' && up[pos + 2] == 'R')
                            {
                                status = State.A1;
                                pos += 3;
                            }
                            else throw new ExceptionWithPosition("Здесь нужны пробел или ключевое слово VAR", pos, s);
                        }
                        break;
                    case State.A1:
                        {
                            if (s.Length-pos+2==0)
                            {
                                pos++;
                                throw new ExceptionWithPosition("Цепочка не окончена, далее нужен пробел.", pos, s);
                            }
                            else if(s.Length - pos + 1 == 0)
                            {
                                throw new ExceptionWithPosition("Цепочка не окончена, далее нужен один из символов: пробел|_|A|...|Z", pos, s);
                            }
                            else if (s[pos] == ' ' && s.Length - pos + 1 != 0)
                            {
                                status = State.A2;
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь нужен пробел", pos, s);
                        }
                        break;
                    case State.A2:
                        {
                            if (up[pos] == ' ')
                            {
                                pos++;
                            }
                            else if (up[pos] >= 'A' && up[pos] <= 'Z' || up[pos] == '_')
                            {
                                status = State.A3;
                                id += s[pos];
                                upid += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Допустимы: пробел|_|A|...|Z", pos, s);
                        }
                        break;
                    case State.A3:
                        {
                            if (up[pos] == ' ')
                            {
                                if (names.Contains(id))
                                {
                                    throw new ExceptionWithPosition("Такой идентификатор уже существует", pos, s);
                                }
                                if (id.Length > 8) throw new ExceptionWithPosition("Длина идентификатора должны быть <= 8", pos, s);
                                if (upid == "VAR" || upid == "REAL" || upid == "BYTE" || upid == "INTEGER" || upid == "ARRAY" || upid == "OF" || upid == "WORD" || upid == "DOUBLE") throw new ExceptionWithPosition("Идентификатор не должен совпадать с ключевым словом", pos, s);
                                else
                                {
                                    status = State.A4;
                                    ident.Add(id);
                                    names.Add(id);
                                    id = "";
                                    upid = "";
                                    pos++;
                                }
                            }
                            else if(up[pos] == ',')
                            {
                                if (names.Contains(id))
                                {
                                    throw new ExceptionWithPosition("Такой идентификатор уже существует", pos, s);
                                }
                                if (id.Length > 8) throw new ExceptionWithPosition("Длина идентификаторая должна быть <= 8", pos, s);
                                if (upid == "VAR" || upid == "REAL" || upid == "BYTE" || upid == "INTEGER" || upid == "ARRAY" || upid == "OF" || upid == "WORD" || upid == "DOUBLE") throw new ExceptionWithPosition("Идентификатор не должен совпадать с ключевым словомб", pos, s);
                                status = State.A2;
                                ident.Add(id);
                                names.Add(id);
                                id = "";
                                upid = "";
                                cnt++;
                                pos++;
                            }
                            else if (up[pos] >= 'A' && up[pos] <= 'Z' || up[pos] >= '0' && up[pos] <= '9')
                            {
                                id += s[pos];
                                upid += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Допустимы: пробел|A|..|Z|0|..|9", pos, s);
                        }
                        break;
                    case State.A4:
                        {
                            if(up[pos] == ',')
                            {
                                status = State.A2;
                                cnt++;
                                pos++;
                            }
                            else if (up[pos] == ' ')
                            {
                                pos ++;
                            }
                            else if(up[pos] == ':')
                            {
                                status = State.A5;
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Допустимы: пробел|,|:", pos, s);
                        }
                        break;
                    //Развилка
                    case State.A5:
                        {
                            if (s[pos] == ' ')
                            {
                                pos++;
                            }
                            //ARRAY
                            else if(s.Length-pos>=5 && up[pos]=='A' && up[pos+1] == 'R' && up[pos+2] == 'R' && up[pos+3] == 'A' && up[pos+4] == 'Y')
                            {
                                status = State.B1;
                                pos += 5;
                            }
                            //CHAR
                            else if(s.Length-pos>=4 && up[pos]=='C' && up[pos+1]=='H' && up[pos+2]=='A' && up[pos + 3] == 'R')
                            {
                                status = State.C1;
                                b = "CHAR"; c = 1;
                                foreach (var i in ident)
                                {
                                    a=i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                ident.Clear();
                                pos += 4;
                            }
                            //REAL
                            else if (s.Length - pos >= 4 && up[pos] == 'R' && up[pos + 1] == 'E' && up[pos + 2] == 'A' && up[pos + 3] == 'L')
                            {
                                status = State.C1;
                                b = "REAL"; c = 8;
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                ident.Clear();
                                pos += 4;
                            }
                            //INTEGER
                            else if (s.Length - pos >= 7 && up[pos] == 'I' && up[pos + 1] == 'N' && up[pos + 2] == 'T' && up[pos + 3] == 'E' && up[pos + 4] == 'G' && up[pos + 5] == 'E' && up[pos + 6] == 'R')
                            {
                                status = State.C1;
                                b = "INTEGER"; c = 4;
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                ident.Clear();
                                pos += 7;
                            }
                            //BYTE
                            else if (s.Length - pos >= 4 && up[pos] == 'B' && up[pos + 1] == 'Y' && up[pos + 2] == 'T' && up[pos + 3] == 'E')
                            {
                                status = State.C1;
                                b = "BYTE"; c = 1;
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                ident.Clear();
                                pos += 4;
                            }
                            //WORD
                            else if (s.Length - pos >= 4 && up[pos] == 'W' && up[pos + 1] == 'O' && up[pos + 2] == 'R' && up[pos + 3] == 'D')
                            {
                                status = State.C1;
                                b = "WORD"; c = 2;
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                ident.Clear();
                                pos += 4;
                            }
                            //DOUBLE
                            else if (s.Length - pos >= 6 && up[pos] == 'D' && up[pos + 1] == 'O' && up[pos + 2] == 'U' && up[pos + 3] == 'B' && up[pos + 4] == 'L' && up[pos + 5] == 'E')
                            {
                                status = State.C1;
                                b = "DOUBLE"; c = 16;
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                ident.Clear();
                                pos += 6;
                            }
                            else throw new ExceptionWithPosition("Здесь нужен: простой тип(CHAR, BYTE, INTEGER, WORD, REAL, DOUBLE)|ARRAY)", pos, s);
                        }
                        break;
                    //ARRAY
                    case State.B1:
                        {
                            if (up[pos] == ' ')
                            {
                                pos++;
                            }
                            else if (up[pos] == '[')
                            {
                                status = State.B2;
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь допустимы: пробел|[", pos, s);
                        }
                        break;
                    case State.B2:
                        {
                            if (up[pos] == '0')
                            {
                                status = State.B3;
                                num += up[pos];
                                pos++;
                            }
                            else if (up[pos] == '-')
                            {
                                num += up[pos];
                                status = State.B22;
                                pos++;
                            }
                            else if (up[pos] >= '1' && up[pos] <= '9')
                            {
                                status = State.B3;
                                num += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь допустимы: 0|..|9|-", pos, s);
                        }
                        break;
                    case State.B22:
                        {
                            if(up[pos]>='1' && up[pos] <= '9')
                            {
                                status = State.B3;
                                num += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь можно: 1|..|9", pos, s);
                        }
                        break;
                    case State.B3:
                        {
                            if (up.Length - pos >= 2 && up[pos] == '.' && up[pos+1] == '.')
                            {
                                bool key = short.TryParse(num, out inum);
                                if (key == true)
                                {
                                    status = State.B4;
                                    c1 = Convert.ToInt32(num);
                                    num = "";
                                    pos += 2;
                                }
                                else throw new ExceptionWithPosition("Целая константа должна быть: -32768< const <32767", pos, s);
                            }
                            else if (up[pos] >= '0' && up[pos] <= '9')
                            {
                                num += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь нужны: .. или 0|..|9", pos, s);
                        }
                        break;
                    case State.B4:
                        {
                            if (up[pos] == '0')
                            {
                                num += up[pos];
                                pos++;
                            }
                            else if (up[pos] == '-')
                            {
                                num += up[pos];
                                status = State.B44;
                                pos++;
                            }
                            else if (up[pos] >= '1' && up[pos] <= '9')
                            {
                                status = State.B5;
                                num += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь допустимы: 0|..|9|-", pos, s);
                        }
                        break;
                    case State.B44:
                        {
                            if (up[pos] >= '1' && up[pos] <= '9')
                            {
                                status = State.B5;
                                num += up[pos];
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь возможны: 0|..|9", pos, s);
                        }
                        break;
                    case State.B5:
                        {
                            if (up[pos] == ']')
                                {
                                bool key = short.TryParse(num, out inum);
                                if (key == true)
                                {
                                    status = State.B7;
                                    c2 = Convert.ToInt32(num);
                                    if (c2 < c1) throw new ExceptionWithPosition("Неверно определён диапазон массива. Второе число должно быть строго больше первого", pos, s);
                                    else
                                    {
                                        c = c2 - c1 + 1;
                                        nums.Add(c);
                                        num = "";
                                        pos++;
                                    }
                                }
                                else throw new ExceptionWithPosition("Целая константа должна быть: -32768< const <32767", pos, s);
                            }
                            else if(up[pos]>='0' && up[pos] <= '9')
                            {
                                num += up[pos];
                                pos++;
                            }
                            else if (up[pos] == ',')
                            {
                                bool key = short.TryParse(num, out inum);
                                if (key == true)
                                {
                                    status = State.B2;
                                    c2 = Convert.ToInt32(num);
                                    if (c2 < c1) throw new ExceptionWithPosition("Неверно определён диапазон массива. Второе число должно быть строго больше первого", pos, s);
                                    else
                                    {
                                        c = c2 - c1 + 1;
                                        nums.Add(c);
                                        num = "";
                                        pos++;
                                    }
                                }
                                else throw new ExceptionWithPosition("Целая константа должна быть: -32768< const <32767", pos, s);
                            }
                            else throw new ExceptionWithPosition("Здесь допустимо: ]|,", pos, s);
                        }
                        break;
                    case State.B7:
                        {
                            if (up[pos] == ' ')
                            {
                                pos++;
                            }
                            else if (up.Length - pos >= 2 && up[pos] == 'O' && up[pos+1] == 'F')
                            {
                                status = State.B8;
                                pos += 2;
                            }
                            else throw new ExceptionWithPosition("Допустимы: пробел|OF", pos, s);
                        }
                        break;
                    case State.B8:
                        {
                            if(up[pos]==' ')
                            {
                                pos++;
                            }
                            //CHAR
                            else if (s.Length - pos >= 4 && up[pos] == 'C' && up[pos + 1] == 'H' && up[pos + 2] == 'A' && up[pos + 3] == 'R')
                            {
                                status = State.C1;
                                b = "ARRAY OF CHAR"; 
                                c = 1;
                                foreach(int i in nums)
                                {
                                    c *= i;
                                }
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                nums.Clear();
                                ident.Clear();
                                pos += 4;
                            }
                            //REAL
                            else if (s.Length - pos >= 4 && up[pos] == 'R' && up[pos + 1] == 'E' && up[pos + 2] == 'A' && up[pos + 3] == 'L')
                            {
                                status = State.C1;
                                b = "ARRAY OF REAL";
                                c = 8;
                                foreach (int i in nums)
                                {
                                    c *= i;
                                }
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                nums.Clear();
                                ident.Clear();
                                pos += 4;
                            }
                            //INTEGER
                            else if (s.Length - pos >= 7 && up[pos] == 'I' && up[pos + 1] == 'N' && up[pos + 2] == 'T' && up[pos + 3] == 'E' && up[pos + 4] == 'G' && up[pos + 5] == 'E' && up[pos + 6] == 'R')
                            {
                                status = State.C1;
                                b = "ARRAY OF INTEGER";
                                c = 4;
                                foreach (int i in nums)
                                {
                                    c *= i;
                                }
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                nums.Clear();
                                ident.Clear();
                                pos += 7;
                            }
                            //BYTE
                            else if (s.Length - pos >= 4 && up[pos] == 'B' && up[pos + 1] == 'Y' && up[pos + 2] == 'T' && up[pos + 3] == 'E')
                            {
                                status = State.C1;
                                b = "ARRAY OF BYTE";
                                c = 1;
                                foreach (int i in nums)
                                {
                                    c *= i;
                                }
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                nums.Clear();
                                ident.Clear();
                                pos += 4;
                            }
                            //WORD
                            else if (s.Length - pos >= 4 && up[pos] == 'W' && up[pos + 1] == 'O' && up[pos + 2] == 'R' && up[pos + 3] == 'D')
                            {
                                status = State.C1;
                                b = "ARRAY OF WORD";
                                c = 2;
                                foreach (int i in nums)
                                {
                                    c *= i;
                                }
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                nums.Clear();
                                ident.Clear();
                                pos += 4;
                            }
                            //DOUBLE
                            else if (s.Length - pos >= 6 && up[pos] == 'D' && up[pos + 1] == 'O' && up[pos + 2] == 'U' && up[pos + 3] == 'B' && up[pos + 4] == 'L' && up[pos + 5] == 'E')
                            {
                                status = State.C1;
                                b = "ARRAY OF DOUBLE";
                                c = 16;
                                foreach (int i in nums)
                                {
                                    c *= i;
                                }
                                foreach (var i in ident)
                                {
                                    a = i;
                                    var gof = new Tuple<string, string, int>(a, b, c);
                                    res.Add(gof);
                                }
                                nums.Clear();
                                ident.Clear();
                                pos += 6;
                            }
                        }
                        break;
                    //Simple types
                    case State.C1:
                        {
                            if (up[pos] == ' ')
                            {
                                pos++;
                            }
                            else if (up[pos] == ';')
                            {
                                status = State.Z;
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь допустимы: пробел|;", pos, s);
                        }
                        break;
                    case State.Z:
                        {
                            if (up[pos] == ' ')
                            {
                                pos++;
                            }
                            else if (up[pos] >= 'A' && up[pos] <= 'Z' || up[pos] == '_')
                            {
                                status = State.A3;
                                id += s[pos];
                                upid +=up[pos];
                                pos++;
                            }
                            else if(up[pos]== '⊥')
                            {
                                status = State.F;
                                pos++;
                            }
                            else throw new ExceptionWithPosition("Здесь допустимы: пробел|A|..|Z|_", pos, s);
                        }
                        break;
                    case State.F:{ pos++; }
                        break;
                }
        }
        if (status != State.F) throw new ExceptionWithPosition("Цепочка не окончена. В конце должна быть швабра(скопируйте и вставьте этот символ в конец строки, если его там нет: ⊥)", pos, s);
        return ResultToString(res);
    }

        private static string[] ResultToString(HashSet<Tuple<string, string, int>> now)
        {
            string[] result = { "", "", "" };
            SortedSet<Tuple<string, string, int>> sort = new SortedSet<Tuple<string, string, int>>();
            foreach (Tuple<string, string, int> i in now)
            {
                sort.Add(i);
            }
            foreach (Tuple<string, string, int> i in sort)
            {
                result[0] += i.Item1 + Environment.NewLine;
                result[1] += i.Item2 + Environment.NewLine;
                result[2] += i.Item3 + Environment.NewLine;

                //Console.WriteLine(i.Item1 + "\t\t" + i.Item2 + "\t\t" + i.Item3);
            }
            result[0] = result[0].Substring(0, result[0].Length - 1);
            result[1] = result[1].Substring(0, result[1].Length - 1);
            result[2] = result[2].Substring(0, result[2].Length - 1);
            return result;
        }
    }

    class ExceptionWithPosition : Exception
    {
        public int Position;
        public string Stext;

        public ExceptionWithPosition() : base() { }
        public ExceptionWithPosition(string message) : base(message) { }
        public ExceptionWithPosition(string message, System.Exception inner) : base(message, inner) { }
        public ExceptionWithPosition(string message, int pos) : base(message)

        {
            Position = pos;
        }
        public ExceptionWithPosition(string message, int pos, string so) : base(message) {
            Position = pos;
            Stext = so;
        }
        protected ExceptionWithPosition(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        { }
    }
}