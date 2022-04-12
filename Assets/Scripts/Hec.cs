using System;
using System.Collections.Generic;

namespace Hec
{
    public enum HecType
    {
        Unknown,
        Type1, // cout
        Type2, // strtok
        Type3  // while(true) cout
    }

    // We won't provide the error location because this "compiler" is purposely made to be horrible
    public class CompilationException : Exception { }

    // "Compiler". It's actually an extremely rudimentary parser that produces an AST while looking for patterns.
    public static class Compiler
    {
        // "Compiles" source code into a type of hec, aka looks for patterns and determines the type of hec that the source code is.
        public static HecType Compile(string src)
        {
            Parser parser = new Parser(src);
            parser.Parse();

            if(parser.isType3)
            {
                return HecType.Type3;
            }

            if(parser.isType2)
            {
                return HecType.Type2;
            }

            if(parser.isType1)
            {
                return HecType.Type1;
            }

            return HecType.Unknown;
        }

        private abstract class ASTNode
        {
            // Nothing. The AST is just for show.
            // Actually the AST could just not exist at all (except for the expr nodes). However, it's easier to work with.
        }

        private class PreprocessorNode : ASTNode
        {
            public string directive;
            public string arg;
        }

        private class FunctionDeclNode : ASTNode
        {
            public string name;
            public Block block;
        }

        private class Block
        {
            public List<Statement> children;

            public Block()
            {
                children = new List<Statement>();
            }
        }

        private abstract class Statement : ASTNode
        {

        }

        private class ReturnStatement : Statement
        {
            public Expression expr;
        }

        private class WhileStatement : Statement
        {
            public Expression condition;
            public Block block;

            public WhileStatement()
            {
                block = new Block();
            }
        }

        private abstract class Expression : Statement
        {
            public abstract int Eval();
        }

        private class NumberExpression : Expression
        {
            public int value;

            public override int Eval()
            {
                return value;
            }
        }

        private class StringExpression : Expression
        {
            public string value;

            public override int Eval()
            {
                return 0;
            }
        }

        private class IdentifierExpression : Expression
        {
            public string id;

            public override int Eval()
            {
                return 0;
            }
        }

        private class BinaryExpression : Expression
        {
            public enum Type
            {
                GREATER,
                GREATER_EQUAL,
                GREATER_GREATER,
                LESS,
                LESS_EQUAL,
            }

            public Type type;

            public Expression left;
            public Expression right;

            public override int Eval()
            {
                int leftResult = left.Eval();
                int rightResult = right.Eval();

                switch(type)
                {
                    case Type.GREATER:
                        return leftResult > rightResult ? 1 : 0;
                    case Type.GREATER_EQUAL:
                        return leftResult >= rightResult ? 1 : 0;
                    case Type.GREATER_GREATER:
                        return 0;
                    case Type.LESS:
                        return leftResult < rightResult ? 1 : 0;
                    case Type.LESS_EQUAL:
                        return leftResult <= rightResult ? 1 : 0;
                }

                return 0;
            }
        }

        private class FunctionCallExpression : Expression
        {
            public string id;
            public List<Expression> args;

            public FunctionCallExpression()
            {
                args = new List<Expression>();
            }

            public override int Eval()
            {
                return 0;
            }
        }

        private class Parser
        {
            private string source;
            private Lexer lexer;

            private Token previous;
            private Token current;

            public bool isType1;
            public bool isType2;
            public bool isType3;

            public Parser(string src)
            {
                source = src;
                lexer = new Lexer(src);

                previous = null;
                current = null;

                isType1 = false;
                isType2 = false;
                isType3 = false;
            }

            public List<ASTNode> Parse()
            {
                List<ASTNode> result = new List<ASTNode>();
                Advance();

                while(current.type != TokenType.EOF)
                {
                    if(Consume(TokenType.HASH))
                    {
                        result.Add(ParsePreprocessor());
                    }
                    else
                    {
                        result.Add(ParseFunctionDecl());
                    }
                }

                return result;
            }

            private ASTNode ParsePreprocessor()
            {
                if(!Consume(TokenType.IDENTIFIER))
                {
                    throw new CompilationException();
                }

                PreprocessorNode node = new PreprocessorNode();

                node.directive = Slice(previous);
                node.arg = Slice(current);

                if (!Consume(TokenType.STRING))
                {
                    throw new CompilationException();
                }

                return node;
            }

            private ASTNode ParseFunctionDecl()
            {
                if(Consume(TokenType.INT))
                {
                    if(!Consume(TokenType.IDENTIFIER))
                    {
                        throw new CompilationException();
                    }

                    FunctionDeclNode node = new FunctionDeclNode();
                    node.name = Slice(previous);

                    if(!Consume(TokenType.LEFT_PAREN))
                    {
                        throw new CompilationException();
                    }

                    if (!Consume(TokenType.RIGHT_PAREN))
                    {
                        throw new CompilationException();
                    }

                    if (!Consume(TokenType.LEFT_BRACE))
                    {
                        throw new CompilationException();
                    }

                    node.block = ParseBlock();

                    return node;
                }
                else
                {
                    return ParseStatement();
                }
            }

            private Block ParseBlock()
            {
                Block block = new Block();

                while(!Consume(TokenType.RIGHT_BRACE))
                {
                    if(Match(TokenType.EOF))
                    {
                        throw new CompilationException();
                    }

                    block.children.Add(ParseStatement());
                }

                return block;
            }

            private Statement ParseStatement()
            {
                if(Consume(TokenType.WHILE))
                {
                    return ParseWhile();
                }

                if(Consume(TokenType.RETURN))
                {
                    return ParseReturn();
                }

                Expression node = ParseExpression();

                if(!Consume(TokenType.SEMICOLON))
                {
                    throw new CompilationException();
                }

                return node;
            }

            private WhileStatement ParseWhile()
            {
                if(!Consume(TokenType.LEFT_PAREN))
                {
                    throw new CompilationException();
                }

                WhileStatement node = new WhileStatement();

                node.condition = ParseExpression();

                if(!Consume(TokenType.RIGHT_PAREN))
                {
                    throw new CompilationException();
                }

                bool type1Backup = isType1;
                isType1 = false; // For detecting if there is a Type1 inside the while body.

                if(!Consume(TokenType.LEFT_BRACE))
                {
                    if (!Consume(TokenType.SEMICOLON))
                    {
                        node.block.children.Add(ParseStatement());
                    }
                }
                else
                {
                    node.block = ParseBlock();
                }

                if(isType1)
                {
                    // Type1 inside infinite loop
                    if (node.condition.Eval() == 1)
                    {
                        isType3 = true;
                    }
                }
                else
                {
                    isType1 = type1Backup;
                }

                return node;
            }

            private ReturnStatement ParseReturn()
            {
                ReturnStatement node = new ReturnStatement();
                node.expr = ParseExpression();

                if(!Consume(TokenType.SEMICOLON))
                {
                    throw new CompilationException();
                }

                return node;
            }

            private Expression ParseExpression()
            {
                Expression left = null;

                if(Consume(TokenType.NUMBER))
                {
                    left = new NumberExpression();
                    ((NumberExpression)left).value = int.Parse(Slice(previous));
                }
                else if(Consume(TokenType.STRING))
                {
                    left = new StringExpression();
                    ((StringExpression)left).value = Slice(previous);
                }
                else if(Consume(TokenType.IDENTIFIER))
                {
                    left = new IdentifierExpression();
                    ((IdentifierExpression)left).id = Slice(previous);
                }
                else
                {
                    throw new CompilationException();
                }

                bool isBinOp = false;
                BinaryExpression.Type type = BinaryExpression.Type.GREATER; // Prevent 'uninitialized' errors

                if(Consume(TokenType.GREATER))
                {
                    isBinOp = true;
                }
                else if (Consume(TokenType.GREATER_EQUAL))
                {
                    isBinOp = true;
                    type = BinaryExpression.Type.GREATER_EQUAL;
                }
                else if (Consume(TokenType.GREATER_GREATER))
                {
                    isBinOp = true;
                    type = BinaryExpression.Type.GREATER_GREATER;

                    if(left is IdentifierExpression)
                    {
                        if(((IdentifierExpression)left).id == "cout")
                        {
                            isType1 = true;
                        }
                    }
                }
                else if (Consume(TokenType.LESS))
                {
                    isBinOp = true;
                    type = BinaryExpression.Type.LESS;
                }
                else if (Consume(TokenType.LESS_EQUAL))
                {
                    isBinOp = true;
                    type = BinaryExpression.Type.LESS_EQUAL;
                }
                else if(Match(TokenType.LEFT_PAREN))
                {
                    FunctionCallExpression node = new FunctionCallExpression();
                    node.id = Slice(previous);

                    if(node.id == "strtok")
                    {
                        isType2 = true;
                    }

                    Advance();

                    if(Consume(TokenType.RIGHT_PAREN))
                    {
                        return node;
                    }

                    do
                    {
                        Expression expr = ParseExpression();
                        node.args.Add(expr);
                    } while (Consume(TokenType.COMMA));

                    if (!Consume(TokenType.RIGHT_PAREN))
                    {
                        throw new CompilationException();
                    }

                    return node;
                }

                if (isBinOp)
                {
                    Expression right = ParseExpression();

                    BinaryExpression node = new BinaryExpression();
                    node.type = type;
                    node.left = left;
                    node.right = right;

                    return node;
                }
                else
                {
                    return left;
                }
            }

            private string Slice(Token token)
            {
                return source.Substring(token.start, token.end - token.start);
            }

            private void Advance()
            {
                previous = current;
                current = lexer.NextToken();
            }

            private bool Consume(TokenType type)
            {
                if (current.type == type)
                {
                    Advance();
                    return true;
                }

                return false;
            }

            private bool Match(TokenType type)
            {
                return current.type == type;
            }
        }

        private enum TokenType
        {
            LEFT_PAREN, RIGHT_PAREN,
            LEFT_BRACE, RIGHT_BRACE,
            COMMA, SEMICOLON,
            HASH,

            GREATER, GREATER_EQUAL, GREATER_GREATER, LESS, LESS_EQUAL,

            IDENTIFIER, STRING, NUMBER,

            INT, WHILE, RETURN,
            EOF
        }

        private class Token
        {
            public TokenType type;
            public int start;
            public int end;

            public Token(TokenType type, int start, int end)
            {
                this.type = type;
                this.start = start;
                this.end = end;
            }
        }

        private class Lexer
        {
            private static readonly Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>()
            {
                { "int", TokenType.INT },
                { "while", TokenType.WHILE },
                { "return", TokenType.RETURN }
            };

            private int start;
            private int current;

            private string source;

            public Lexer(string src)
            {
                start = 0;
                current = 0;
                source = src;
            }

            public Token NextToken()
            {
                start = current;

                bool whitespace = true;

                while(!AtEnd() && whitespace)
                {
                    switch(Peek())
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            Advance();
                            break;
                        case '/':
                            if (Peek(1) == '/')
                            {
                                Advance(2);

                                while (!AtEnd() && Peek() != '\n')
                                {
                                    Advance();
                                }
                            }
                            else if (Peek(1) == '*')
                            {
                                Advance(2);

                                while (!AtEnd())
                                {
                                    if (Peek() == '*' && !AtEnd(1) && Peek(1) == '/')
                                    {
                                        break;
                                    }
                                    Advance();
                                }

                                if (AtEnd())
                                {
                                    throw new CompilationException();
                                }

                                Advance(2);
                            }
                            else
                            {
                                whitespace = false;
                            }

                            break;
                        default:
                            whitespace = false;
                            break;
                    }
                }

                start = current;

                if(AtEnd())
                {
                    return Emit(TokenType.EOF);
                }

                char c = Next();

                if(char.IsLetter(c) || c ==  '_')
                {
                    return NextIdentifier();
                }

                if(char.IsDigit(c))
                {
                    return NextNumber();
                }

                switch(c)
                {
                    case '(': return Emit(TokenType.LEFT_PAREN);
                    case ')': return Emit(TokenType.RIGHT_PAREN);
                    case '{': return Emit(TokenType.LEFT_BRACE);
                    case '}': return Emit(TokenType.RIGHT_BRACE);
                    case ',': return Emit(TokenType.COMMA);
                    case ';': return Emit(TokenType.SEMICOLON);
                    case '#': return Emit(TokenType.HASH);
                    case '>':
                        if(Peek() == '>')
                        {
                            Advance();
                            return Emit(TokenType.GREATER_GREATER);
                        }
                        else if(Peek() == '=')
                        {
                            Advance();
                            return Emit(TokenType.GREATER_EQUAL);
                        }
                        return Emit(TokenType.GREATER);
                    case '<':
                        if (Peek() == '=')
                        {
                            Advance();
                            return Emit(TokenType.LESS_EQUAL);
                        }
                        return Emit(TokenType.LESS);
                    case '\"':
                        ++start;
                        return NextString();
                    default:
                        throw new CompilationException();
                }
            }

            private Token NextIdentifier()
            {
                while(!AtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
                {
                    Advance();
                }

                string id = source.Substring(start, current - start);

                if (KEYWORDS.ContainsKey(id))
                {
                    return Emit(KEYWORDS[id]);
                }

                return Emit(TokenType.IDENTIFIER);
            }

            private Token NextNumber()
            {
                while(!AtEnd() && char.IsDigit(Peek()))
                {
                    Advance();
                }

                return Emit(TokenType.NUMBER);
            }

            private Token NextString()
            {
                while(!AtEnd())
                {
                    if(Peek() == '"')
                    {
                        if(Peek(-1) != '\\')
                        {
                            break;
                        }
                    }
                    else if(Peek() == '\n')
                    {
                        throw new CompilationException();
                    }

                    Advance();
                }

                if(AtEnd())
                {
                    throw new CompilationException();
                }

                Token token = Emit(TokenType.STRING);
                Advance();

                return token;
            }

            private Token Emit(TokenType type)
            {
                return new Token(type, start, current);
            }

            private bool AtEnd(int lookahead = 0)
            {
                return current + lookahead >= source.Length;
            }
            
            private char Peek(int lookahead = 0)
            {
                if(AtEnd(lookahead))
                {
                    return '\0';
                }

                return source[current + lookahead];
            }

            private void Advance(int amount = 1)
            {
                current += amount;
            }

            private char Next()
            {
                if(AtEnd())
                {
                    return '\0';
                }

                return source[current++];
            }
        }
    }
}
