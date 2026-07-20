using System.Text;
using Compiler.Grammar;
using Compiler.utils;

namespace Compiler.Lexer;

/// <summary>
/// Represents the result of the lexical analysis performed by the lexer, containing the list of tokens and any errors encountered during the process.
/// </summary>
/// <param name="Tokens">The list of tokens produced by the lexer.</param>
/// <param name="Errors">The list of errors encountered during lexical analysis.</param>
public record LexerResult(List<Token> Tokens, List<Error> Errors);

/// <summary>
/// Represents the result of attempting to create a single token, containing either the token or an error if the token could not be created.
/// </summary>
/// <param name="Token">The token produced, if successful; otherwise, null.</param>
/// <param name="Error">The error encountered during token creation, if any; otherwise, null.</param>
public record TokenResult(Token Token, Error Error);

/// <summary>
/// Provides lexical analysis functionality for the compiler, converting an input stream into a sequence of tokens.
/// </summary>
public static class Lexer
{
    private static StreamReader _reader;
    private static Position _position;

    // only valid if _isEndOfFile is false
    private static char _currentChar;
    private static bool _isEndOfFile;

/// <summary>
/// Advances the lexer to the next character in the input stream, updating the current character and position information.
/// </summary>
/// <remarks>
/// This method reads the next character from the input stream and updates the current character and position.
/// If the end of the input stream is reached, it sets the end-of-file flag. After you advance, always make sure 
/// to check the `_isEndOfFile` flag to determine if the end of the input stream has been reached.
/// </remarks>
    private static void Advance()
    {
        _position.Advance(_currentChar);

        int charCode = _reader.Read();

        if (charCode == -1)
        {
            _isEndOfFile = true;
        }
        else
        {
            _currentChar = (char)charCode;
        }
    }

    /// <summary>
    /// Determines whether the specified character is a valid first character for an identifier.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>True if the character is a letter or an underscore; otherwise, false.</returns>
    private static bool validFirstIdentifierChar(char c) => Char.IsLetter(c) || c == '_';

    /// <summary>
    /// Determines whether the specified character is a valid character for an identifier (after the first character).
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>True if the character is a letter, digit, or underscore; otherwise, false.</returns>
    private static bool validIdentifierChar(char c) => Char.IsLetterOrDigit(c) || c == '_';

/// <summary>
/// Creates an identifier token from the current position in the input stream. It reads characters that form a valid identifier, checks if the identifier is a keyword, and returns the appropriate token.
/// </summary>
/// <returns>A <see cref="Token"/> representing the identifier or keyword found in the input stream.</returns>
    private static Token MakeIdentifier()
    {
        Position startPosition = _position;
        var stringBuilder = new StringBuilder(_currentChar);

        Advance();

        while (validIdentifierChar(_currentChar) && !_isEndOfFile)
        {
            stringBuilder.Append(_currentChar);
            Advance();
        }

        string identifier = stringBuilder.ToString();

        return Token.MakeTokenFromString(identifier, startPosition, _position);
    }

    /// <summary>
    /// Determines whether the specified character is a valid character for a number (digit or decimal point).
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>True if the character is a digit or a decimal point; otherwise, false.</returns>
    private static bool validNumberChar(char c) => char.IsNumber(c) || c == '.';

/// <summary>
/// Creates a number token from the current position in the input stream. It reads characters that form a valid number, determines whether it is an integer or a double, and returns the appropriate token result.
/// </summary>
/// <returns>A <see cref="TokenResult"/> containing the number token or an error if the number cannot be parsed.</returns>
    private static TokenResult MakeNumber()
    {
        Position startPosition = _position;
        bool hasPoint = false;
        var stringBuilder = new StringBuilder(_currentChar);

        Advance();

        while (validNumberChar(_currentChar) && !_isEndOfFile)
        {
            if (_currentChar == '.')
            {
                // We don't allow two points in a number
                if (hasPoint)
                    break;
                hasPoint = true;
            }
            stringBuilder.Append(_currentChar);
            Advance();
        }

        string number = stringBuilder.ToString();

        if (hasPoint)
        {
            try
            {
                return new TokenResult(
                    new DoubleToken(double.Parse(number), startPosition, _position),
                    null
                );
            }
            catch (OverflowException)
            {
                return new TokenResult(
                    null,
                    new Error(
                        startPosition,
                        _position,
                        "Number Too Large",
                        $"\"{number}\" cannot be represented as a double"
                    )
                );
            }
        }
        else
        {
            try
            {
                return new TokenResult(
                    new IntToken(int.Parse(number), startPosition, _position),
                    null
                );
            }
            catch (OverflowException)
            {
                return new TokenResult(
                    null,
                    new Error(
                        startPosition,
                        _position,
                        "Integer Overflow",
                        $"\"{number}\" cannot be represented as an integer"
                    )
                );
            }
        }
    }
/// <summary>
/// Creates a string token from the current position in the input stream. It reads characters enclosed in double quotes, handling escape sequences, and returns the corresponding string token.
/// </summary>
/// <returns>A <see cref="StringToken"/> representing the string found in the input stream.</returns>
    private static Token MakeString()
    {
        var stringBuilder = new StringBuilder();
        Position startPosition = _position;

        Advance();

        while (_currentChar != '"' && !_isEndOfFile)
        {
            if (_currentChar == '\\')
            {
                Advance();
                if (_isEndOfFile)
                    break;
            }
            stringBuilder.Append(_currentChar);
            Advance();
        }
        if (!_isEndOfFile)
        {
            Advance();
        }
        return new StringToken(
            stringBuilder.ToString(),
            startPosition,
            _position
        );
    }

//TODO: Update to reflect current grammar
/// <summary>
/// Reads the input file specified by the file name, tokenizes its contents, and returns the list of tokens and any errors encountered during lexing.
/// </summary>
/// <param name="fileName">The path to the input file to be tokenized.</param>
/// <returns>A <see cref="LexerResult"/> containing the list of tokens and any errors encountered during lexing.</returns>
    public static LexerResult MakeTokens(string fileName)
    {
        // Initialize statics
        _reader = new StreamReader(fileName);
        // Start at negative row so that advance works properly
        _position = new Position(-1, 0, fileName);
        _isEndOfFile = false;
        _currentChar = '\0';
        Advance();

        var tokens = new List<Token>();
        Position positionStart;
        var errors = new List<Error>();

        while (!_isEndOfFile)
        {
            if (char.IsWhiteSpace(_currentChar))
            {
                Advance();
            }
            else if (_currentChar == '#')
            { // Comment
                Advance();
                while (_currentChar != '#' && !_isEndOfFile)
                {
                    Advance();
                }
                Advance();
            }
            else if (validFirstIdentifierChar(_currentChar))
            {
                tokens.Add(MakeIdentifier());
            }
            else if (char.IsDigit(_currentChar))
            {
                TokenResult tokenResult = MakeNumber();

                if (tokenResult.Error != null)
                {
                    errors.Add(tokenResult.Error);
                }
                if (tokenResult.Token != null)
                {
                    tokens.Add(tokenResult.Token);
                }
            }
            else if (_currentChar == '"')
            {
                tokens.Add(MakeString());
            }
            else
            {
                switch (_currentChar)
                {
                    case '+':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '+' && !_isEndOfFile)
                        {
                            Advance();
                            tokens.Add(new Token(TerminalSymbol.Type.PP, positionStart, _position));
                        }
                        else
                        {
                            tokens.Add(new Token(TerminalSymbol.Type.Plus, positionStart, _position));
                        }
                        break;
                    case '-':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '-' && !_isEndOfFile)
                        {
                            Advance();
                            tokens.Add(new Token(TerminalSymbol.Type.MM, positionStart, _position));
                        }
                        else
                        {
                            tokens.Add(new Token(TerminalSymbol.Type.Minus, positionStart, _position));
                        }
                        break;
                    case '*':
                        positionStart = _position;
                        tokens.Add(new Token(TerminalSymbol.Type.Multiply, positionStart, _position));
                        Advance();
                        break;
                    case '/':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.Divide, positionStart, _position));
                        break;
                    case '^':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.Power, positionStart, _position));
                        break;
                    case '=':
                        positionStart = _position;
                        Advance();
                        if (_isEndOfFile)
                        {
                            tokens.Add(new Token(TerminalSymbol.Type.EQ, positionStart, _position));
                            break;
                        }
                        switch (_currentChar)
                        {
                            case '+':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.EPlus, positionStart, _position));
                                break;
                            case '-':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.EMinus, positionStart, _position));
                                break;
                            case '*':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.EMult, positionStart, _position));
                                break;
                            case '/':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.EDiv, positionStart, _position));
                                break;
                            case '^':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.EPow, positionStart, _position));
                                break;
                            case '=':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.EE, positionStart, _position));
                                break;
                            default:
                                tokens.Add(new Token(TerminalSymbol.Type.EQ, positionStart, _position));
                                break;
                        }
                        break;
                    case '<':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '=' && !_isEndOfFile)
                        {
                            Advance();
                            tokens.Add(new Token(TerminalSymbol.Type.LTE, positionStart, _position));
                        }
                        else
                        {
                            tokens.Add(new Token(TerminalSymbol.Type.LT, positionStart, _position));
                        }
                        break;
                    case '>':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '=' && !_isEndOfFile)
                        {
                            Advance();
                            tokens.Add(new Token(TerminalSymbol.Type.GTE, positionStart, _position));
                        }
                        else
                        {
                            tokens.Add(new Token(TerminalSymbol.Type.GT, positionStart, _position));
                        }
                        break;
                    case '(':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.LParen, positionStart, _position));
                        break;
                    case ')':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.RParen, positionStart, _position));
                        break;
                    case '{':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.LBrace, positionStart, _position));
                        break;
                    case '}':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.RBrace, positionStart, _position));
                        break;
                    case '[':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.LBrack, positionStart, _position));
                        break;
                    case ']':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.RBrack, positionStart, _position));
                        break;
                    case ',':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.Comma, positionStart, _position));
                        break;
                    case ';':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.SC, positionStart, _position));
                        break;
                    case ':':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.Collon, positionStart, _position));
                        break;
                    case '.':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.Point, positionStart, _position));
                        break;
                    case '!':
                        positionStart = _position;
                        Advance();
                        if (_isEndOfFile)
                        {
                            tokens.Add(new Token(TerminalSymbol.Type.Not, positionStart, _position));
                            break;
                        }
                        switch (_currentChar)
                        {
                            case '=':
                                Advance();
                                tokens.Add(new Token(TerminalSymbol.Type.NE, positionStart, _position));
                                break;
                            default:
                                tokens.Add(new Token(TerminalSymbol.Type.Not, positionStart, _position));
                                break;
                        }
                        break;
                    case '|':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.Or, positionStart, _position));
                        break;
                    case '&':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TerminalSymbol.Type.And, positionStart, _position));
                        break;
                    default:
                        positionStart = _position;
                        char character = _currentChar;
                        Advance();
                        errors.Add(
                            new Error(
                                positionStart,
                                _position,
                                "Illegal Character",
                                $"\"{character}\""
                            )
                        );
                        break;
                }
            }
        }

        positionStart = _position;
        Advance();
        tokens.Add(new Token(TerminalSymbol.Type.EOF, positionStart, _position));
        return new LexerResult(tokens, errors);
    }
}
