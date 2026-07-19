### Grammar

The grammar for the language can be found at [GRAMMAR.ebnf](docs/GRAMMAR.ebnf). This is written in [Extended Backus Naur Form](docs/EBNF.MD) (EBNF) and describes the productions of the language. for more infortation, see [grammar/docs/](docs/).

To define the grammar so that it can be used by our parser, we also need a class containing the productins. To accomplish this we have implemented the [BackusNaurForm](BackusNaurForm.cs) class which can represent BNF notation in code. This can be translated by the parser into a [LRDFA](//src/parser/slr/LRDFA.cs) and an [LRTable](//src/parser/slr/LRTable.cs) which can parse token streams.

TODO: Still to do is to create a conversion from EBNF to BNF classes and define the productions in the [GRAMMAR.ebnf](docs/GRAMMAR.ebnf) file in code.It would be good to define some operator overloads so that the c# file would look like proper ebnf notation and we could just call on function to convert it to bnf.