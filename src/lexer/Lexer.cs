using System.Text;

public record LexerResult(List<Token> Tokens, List<Error> Errors);

private record TokenResult(Token Token, Error Error);

public static class Lexer
{

    private static StreamReader _reader;
    private static Position _position;
    // only valid if _isEndOfFile is false
    private static char _currentChar;
    private static bool _isEndOfFile;

    private static void Advance(){
        _position.Advance(_currentChar);

        int charCode = _reader.Read();
        
        if (charCode == -1){
            _isEndOfFile = true;
        } else {
            _currentChar = (char)charCode;
        }
    }

    private static bool validFirstIdentifierChar(char c) => Char.IsLetter(c) || c == '_';

    private static bool validIdentifierChar(char c) => Char.IsLetterOrDigit(c) || c == '_';

    private static Token MakeIdentifier(){

        Position startPosition = _position;
        var stringBuilder = new StringBuilder(_currentChar);

        Advance();

        while (validIdentifierChar(_currentChar) && !_isEndOfFile){
            stringBuilder.Append(_currentChar);
            Advance();
        }

        string identifier = stringBuilder.ToString();

        TokenType? tokenType = Token.GetKeywordTokenType(identifier);

        if (tokenType is not null){
            return new Token(tokenType.Value, startPosition, _position);
        } else {
            return new Token(TokenType.Identifier, identifier, startPosition, _position);
        }
    }

    private static bool validNumberChar(char c) => char.IsNumber(c) || c == '.';

    private static TokenResult MakeNumber(){

        Position startPosition = _position;
        bool hasPoint = false;
        var stringBuilder = new StringBuilder(_currentChar);

        Advance();

        while (validNumberChar(_currentChar) && !_isEndOfFile){
            if (_currentChar == '.'){
                // We don't allow two points in a number
                if (hasPoint)
                    break;
                hasPoint = true;
            }
            stringBuilder.Append(_currentChar);
            Advance();
        }

        string number = stringBuilder.ToString();

        if (hasPoint){
            try{
                return new TokenResult(new DoubleToken(double.Parse(number), startPosition, _position), null);
            }
            catch (OverflowException){
                return new TokenResult(null, new Error(startPosition, _position, "Number Too Large", $"\"{number}\" cannot be represented as a double"));
            }
        }
        else{
            try{
                return new TokenResult(new IntToken(int.Parse(number), startPosition, _position), null);
            }
            catch (OverflowException){
                return new TokenResult(null, new Error(startPosition, _position, "Integer Overflow", $"\"{number}\" cannot be represented as an integer"));
            }
        }
        
    }

    private static Token MakeString(){

        var stringBuilder = new StringBuilder();
        Position startPosition = _position;

        Advance();

        while (_currentChar != '"' && !_isEndOfFile){
            if (_currentChar == '\\'){
                Advance();
                if(_isEndOfFile)
                    break;
            }
            stringBuilder.Append(_currentChar);
            Advance();
        }
        if(!_isEndOfFile){
            Advance();
        }
        return new Token(TokenType.String, stringBuilder.ToString(), startPosition, _position);
    }

    public static LexerResult MakeTokens(string fileName){

        // Initialize statics
        _reader = new StreamReader(fileName);
        // Start at negative row so that advance works properly
        _position = new Position(-1, 0, fileName);
        _isEndOfFile = false;
        _currentChar = '\0';
        Advance();

        var tokens = new List<Token>();
        Position positionStart = _position;
        var errors = new List<Error>();

        while (!_isEndOfFile){
            if (IsWhiteSpace(_currentChar)){
                Advance();
            }
            else if (_currentChar == '#'){ // Comment
                Advance();
                while (_currentChar != '#' && !_isEndOfFile){
                    Advance();
                }
                Advance();
            }
            else if (validFirstIdentifierChar(_currentChar)){
                tokens.Add(MakeIdentifier());
            }
            else if (IsNumber(_currentChar)){
                tokens.Add(MakeNumber());
            }
            else if (_currentChar == '"'){
                tokens.Add(MakeString());
            }
            else{
                switch (_currentChar){
                    case '+':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '+' && !_isEndOfFile){
                            Advance();
                            tokens.Add(new Token(TokenType.PP, positionStart, _position));
                        }
                        else{
                            tokens.Add(new Token(TokenType.Plus, positionStart, _position));
                        }
                        break;
                    case '-':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '-' && !_isEndOfFile){
                            Advance();
                            tokens.Add(new Token(TokenType.MM, positionStart, _position));
                        }
                        else{
                            tokens.Add(new Token(TokenType.Minus, positionStart, _position));
                        }
                        break;
                    case '*':
                        positionStart = _position;
                        tokens.Add(new Token(TokenType.Multiply, positionStart, _position));
                        Advance();
                        break;
                    case '/':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.Divide, positionStart, _position));
                        break;
                    case '^':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.Power, positionStart, _position));
                        break;
                    case '=':
                        positionStart = _position;
                        Advance();
                        if(_isEndOfFile){
                            tokens.Add(new Token(TokenType.EQ, positionStart, _position));
                            break;
                        }
                        switch (_currentChar){
                            case '+':
                                Advance();
                                tokens.Add(new Token(TokenType.EPlus, positionStart, _position));
                                break;
                            case '-':
                                Advance();
                                tokens.Add(new Token(TokenType.EMinus, positionStart, _position));
                                break;
                            case '*':
                                Advance();
                                tokens.Add(new Token(TokenType.EMult, positionStart, _position));
                                break;
                            case '/':
                                Advance();
                                tokens.Add(new Token(TokenType.EDiv, positionStart, _position));
                                break;
                            case '^':
                                Advance();
                                tokens.Add(new Token(TokenType.EPow, positionStart, _position));
                                break;
                            case '=':
                                Advance();
                                tokens.Add(new Token(TokenType.EE, positionStart, _position));
                                break;
                            default:
                                tokens.Add(new Token(TokenType.EQ, positionStart, _position));
                                break;
                        }
                        break;
                    case '<':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '=' && !_isEndOfFile){
                            Advance();
                            tokens.Add(new Token(TokenType.LTE, positionStart, _position));
                        }
                        else{
                            tokens.Add(new Token(TokenType.LT, positionStart, _position));
                        }
                        break;
                    case '>':
                        positionStart = _position;
                        Advance();
                        if (_currentChar == '=' && !_isEndOfFile){
                            Advance();
                            tokens.Add(new Token(TokenType.GTE, positionStart, _position));
                        }
                        else{
                            tokens.Add(new Token(TokenType.GT, positionStart, _position));
                        }
                        break;
                    case '(':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.LParen, positionStart, _position));
                        break;
                    case ')':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.RParen, positionStart, _position));
                        break;
                    case '{':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.LBrace, positionStart, _position));
                        break;
                    case '}':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.RBrace, positionStart, _position));
                        break;
                    case '[':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.LBrack, positionStart, _position));
                        break;
                    case ']':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.RBrack, positionStart, _position));
                        break;
                    case ',':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.Comma, positionStart, _position));
                        break;
                    case ';':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.SC, positionStart, _position));
                        break;
                    case ':':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.Collon, positionStart, _position));
                        break;
                    case '.':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.Point, positionStart, _position));
                        break;
                    case '!':
                        positionStart = _position;
                        Advance();
                        if(_isEndOfFile){
                            tokens.Add(new Token(TokenType.Not, positionStart, _position));
                            break;
                        }
                        switch(_currentChar){
                            case '=':
                                Advance();
                                tokens.Add(new Token(TokenType.NE, positionStart, _position));
                                break;
                            default:
                                tokens.Add(new Token(TokenType.Not, positionStart, _position));
                                break;
                        }
                        break;
                    case '|':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.Or, positionStart, _position));
                        break;
                    case '&':
                        positionStart = _position;
                        Advance();
                        tokens.Add(new Token(TokenType.And, positionStart, _position));
                        break;
                    default:
                        positionStart = _position;
                        char character = _currentChar;
                        Advance();
                        return new LexerResult(null , new Error(positionStart, _position, "Illegal Character", $"\"{character}\""));
                }
            }
        }

        positionStart = _position;
        Advance();
        tokens.Add(new Token(TokenType.EOF, positionStart, _position));
        return new LexerResult(tokens, errors);
    }
}