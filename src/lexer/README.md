### Lexer

The Lexer, or the tokenizer, is the part of the compiler which takes the byte array from the raw file input, and converts it to a series of tokens. In theory the raw characters could be treated as tokens too, but this simplifies the contruction for tokens like strings and identifiers, which fit much more nicely into our SLR parser using this model.

The [Lexer](Lexer.cs) itself takes the input stream and converts it using a series of switch cases and longest match convention. TODO: Note that this does not reflect the current state of the language, and is outdated in more than a few aspects. The general scheme however is correct, with only the symantics of the grammar differing.

In [Token.cs](Token.cs) the actual tokens are defined. These have types that derrive from the [Symbols](//src/grammar//Symbols.cs) defined in the grammar. Because some tokens need additional information, i.e. strings and integers, we have derived classes which hold extra fields to store those data. There is some redundancy here, as the Terminal Symbol should always be the same as the class type for these classes. 