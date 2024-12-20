//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from d:/EU4 Grammar/PDXLanguageTokens.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class PDXLanguageTokens : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		EQ=1, LPAR=2, RPAR=3, AND=4, OR=5, YES=6, NO=7, NOT=8, MPOWER=9, TAG=10, 
		INT=11, STRING=12, FLOAT=13, DATE=14, MONTHS=15, DAYS=16, COLOR=17, PLUS=18, 
		MINUS=19, COMMA=20, DOT=21, WHITESPACE=22, SINGLE_LINE_COMMENT=23, IF=24, 
		ELSE=25, ELSE_IF=26, LIMIT=27, WHILE=28, IDENTIFIER=29, STRING_TOOLTIP=30;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"EQ", "LPAR", "RPAR", "AND", "OR", "YES", "NO", "NOT", "MPOWER", "TAG", 
		"INT", "STRING", "FLOAT", "DATE", "MONTHS", "DAYS", "COLOR", "PLUS", "MINUS", 
		"COMMA", "DOT", "WHITESPACE", "SINGLE_LINE_COMMENT", "IF", "ELSE", "ELSE_IF", 
		"LIMIT", "WHILE", "IDENTIFIER", "STRING_TOOLTIP"
	};


	public PDXLanguageTokens(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public PDXLanguageTokens(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'='", "'{'", "'}'", "'AND'", "'OR'", null, null, null, null, null, 
		null, null, null, null, "'months'", "'days'", "'color'", "'+'", "'-'", 
		"','", "'.'", null, null, "'if'", "'else'", "'else_if'", "'limit'", "'while'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "EQ", "LPAR", "RPAR", "AND", "OR", "YES", "NO", "NOT", "MPOWER", 
		"TAG", "INT", "STRING", "FLOAT", "DATE", "MONTHS", "DAYS", "COLOR", "PLUS", 
		"MINUS", "COMMA", "DOT", "WHITESPACE", "SINGLE_LINE_COMMENT", "IF", "ELSE", 
		"ELSE_IF", "LIMIT", "WHILE", "IDENTIFIER", "STRING_TOOLTIP"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "PDXLanguageTokens.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static PDXLanguageTokens() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,30,271,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,1,0,1,0,1,1,1,1,1,2,1,2,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,5,
		1,5,1,5,1,5,1,5,1,5,3,5,81,8,5,1,6,1,6,1,6,1,6,3,6,87,8,6,1,7,1,7,1,7,
		1,7,1,7,1,7,3,7,95,8,7,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,1,8,
		1,8,1,8,1,8,1,8,1,8,1,8,3,8,115,8,8,1,9,1,9,1,9,1,9,1,10,3,10,122,8,10,
		1,10,4,10,125,8,10,11,10,12,10,126,1,11,1,11,1,11,1,11,1,11,1,11,1,11,
		1,11,1,11,1,11,3,11,139,8,11,5,11,141,8,11,10,11,12,11,144,9,11,1,11,1,
		11,1,12,3,12,149,8,12,1,12,4,12,152,8,12,11,12,12,12,153,1,12,1,12,4,12,
		158,8,12,11,12,12,12,159,1,13,4,13,163,8,13,11,13,12,13,164,1,13,1,13,
		4,13,169,8,13,11,13,12,13,170,1,13,1,13,4,13,175,8,13,11,13,12,13,176,
		1,14,1,14,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,16,1,16,
		1,16,1,16,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,1,20,1,21,4,21,
		206,8,21,11,21,12,21,207,1,21,1,21,1,22,1,22,5,22,214,8,22,10,22,12,22,
		217,9,22,1,22,1,22,1,23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,1,25,1,25,1,
		25,1,25,1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,26,1,27,1,27,1,
		27,1,27,1,27,1,27,1,28,4,28,250,8,28,11,28,12,28,251,1,28,1,28,4,28,256,
		8,28,11,28,12,28,257,1,28,4,28,261,8,28,11,28,12,28,262,3,28,265,8,28,
		1,29,4,29,268,8,29,11,29,12,29,269,0,0,30,1,1,3,2,5,3,7,4,9,5,11,6,13,
		7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,
		39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,1,0,
		10,1,0,65,90,2,0,48,57,65,90,2,0,43,43,45,45,1,0,48,57,1,0,34,34,1,0,49,
		57,3,0,9,10,13,13,32,32,2,0,10,10,13,13,4,0,48,57,65,90,95,95,97,122,5,
		0,36,36,48,57,65,90,95,95,97,122,299,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,
		0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,
		1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,
		0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,
		1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,
		0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,1,61,
		1,0,0,0,3,63,1,0,0,0,5,65,1,0,0,0,7,67,1,0,0,0,9,71,1,0,0,0,11,80,1,0,
		0,0,13,86,1,0,0,0,15,94,1,0,0,0,17,114,1,0,0,0,19,116,1,0,0,0,21,121,1,
		0,0,0,23,128,1,0,0,0,25,148,1,0,0,0,27,162,1,0,0,0,29,178,1,0,0,0,31,185,
		1,0,0,0,33,190,1,0,0,0,35,196,1,0,0,0,37,198,1,0,0,0,39,200,1,0,0,0,41,
		202,1,0,0,0,43,205,1,0,0,0,45,211,1,0,0,0,47,220,1,0,0,0,49,223,1,0,0,
		0,51,228,1,0,0,0,53,236,1,0,0,0,55,242,1,0,0,0,57,264,1,0,0,0,59,267,1,
		0,0,0,61,62,5,61,0,0,62,2,1,0,0,0,63,64,5,123,0,0,64,4,1,0,0,0,65,66,5,
		125,0,0,66,6,1,0,0,0,67,68,5,65,0,0,68,69,5,78,0,0,69,70,5,68,0,0,70,8,
		1,0,0,0,71,72,5,79,0,0,72,73,5,82,0,0,73,10,1,0,0,0,74,75,5,121,0,0,75,
		76,5,101,0,0,76,81,5,115,0,0,77,78,5,89,0,0,78,79,5,69,0,0,79,81,5,83,
		0,0,80,74,1,0,0,0,80,77,1,0,0,0,81,12,1,0,0,0,82,83,5,110,0,0,83,87,5,
		111,0,0,84,85,5,78,0,0,85,87,5,79,0,0,86,82,1,0,0,0,86,84,1,0,0,0,87,14,
		1,0,0,0,88,89,5,78,0,0,89,90,5,79,0,0,90,95,5,84,0,0,91,92,5,110,0,0,92,
		93,5,111,0,0,93,95,5,116,0,0,94,88,1,0,0,0,94,91,1,0,0,0,95,16,1,0,0,0,
		96,97,5,65,0,0,97,98,5,68,0,0,98,115,5,77,0,0,99,100,5,77,0,0,100,101,
		5,73,0,0,101,115,5,76,0,0,102,103,5,68,0,0,103,104,5,73,0,0,104,115,5,
		80,0,0,105,106,5,97,0,0,106,107,5,100,0,0,107,115,5,109,0,0,108,109,5,
		100,0,0,109,110,5,105,0,0,110,115,5,112,0,0,111,112,5,109,0,0,112,113,
		5,105,0,0,113,115,5,108,0,0,114,96,1,0,0,0,114,99,1,0,0,0,114,102,1,0,
		0,0,114,105,1,0,0,0,114,108,1,0,0,0,114,111,1,0,0,0,115,18,1,0,0,0,116,
		117,7,0,0,0,117,118,7,1,0,0,118,119,7,1,0,0,119,20,1,0,0,0,120,122,7,2,
		0,0,121,120,1,0,0,0,121,122,1,0,0,0,122,124,1,0,0,0,123,125,7,3,0,0,124,
		123,1,0,0,0,125,126,1,0,0,0,126,124,1,0,0,0,126,127,1,0,0,0,127,22,1,0,
		0,0,128,142,5,34,0,0,129,141,8,4,0,0,130,139,5,32,0,0,131,132,5,92,0,0,
		132,139,5,110,0,0,133,134,5,92,0,0,134,139,5,116,0,0,135,136,5,92,0,0,
		136,139,5,34,0,0,137,139,5,92,0,0,138,130,1,0,0,0,138,131,1,0,0,0,138,
		133,1,0,0,0,138,135,1,0,0,0,138,137,1,0,0,0,139,141,1,0,0,0,140,129,1,
		0,0,0,140,138,1,0,0,0,141,144,1,0,0,0,142,140,1,0,0,0,142,143,1,0,0,0,
		143,145,1,0,0,0,144,142,1,0,0,0,145,146,5,34,0,0,146,24,1,0,0,0,147,149,
		7,2,0,0,148,147,1,0,0,0,148,149,1,0,0,0,149,151,1,0,0,0,150,152,7,3,0,
		0,151,150,1,0,0,0,152,153,1,0,0,0,153,151,1,0,0,0,153,154,1,0,0,0,154,
		155,1,0,0,0,155,157,5,46,0,0,156,158,7,3,0,0,157,156,1,0,0,0,158,159,1,
		0,0,0,159,157,1,0,0,0,159,160,1,0,0,0,160,26,1,0,0,0,161,163,7,5,0,0,162,
		161,1,0,0,0,163,164,1,0,0,0,164,162,1,0,0,0,164,165,1,0,0,0,165,166,1,
		0,0,0,166,168,5,46,0,0,167,169,7,3,0,0,168,167,1,0,0,0,169,170,1,0,0,0,
		170,168,1,0,0,0,170,171,1,0,0,0,171,172,1,0,0,0,172,174,5,46,0,0,173,175,
		7,3,0,0,174,173,1,0,0,0,175,176,1,0,0,0,176,174,1,0,0,0,176,177,1,0,0,
		0,177,28,1,0,0,0,178,179,5,109,0,0,179,180,5,111,0,0,180,181,5,110,0,0,
		181,182,5,116,0,0,182,183,5,104,0,0,183,184,5,115,0,0,184,30,1,0,0,0,185,
		186,5,100,0,0,186,187,5,97,0,0,187,188,5,121,0,0,188,189,5,115,0,0,189,
		32,1,0,0,0,190,191,5,99,0,0,191,192,5,111,0,0,192,193,5,108,0,0,193,194,
		5,111,0,0,194,195,5,114,0,0,195,34,1,0,0,0,196,197,5,43,0,0,197,36,1,0,
		0,0,198,199,5,45,0,0,199,38,1,0,0,0,200,201,5,44,0,0,201,40,1,0,0,0,202,
		203,5,46,0,0,203,42,1,0,0,0,204,206,7,6,0,0,205,204,1,0,0,0,206,207,1,
		0,0,0,207,205,1,0,0,0,207,208,1,0,0,0,208,209,1,0,0,0,209,210,6,21,0,0,
		210,44,1,0,0,0,211,215,5,35,0,0,212,214,8,7,0,0,213,212,1,0,0,0,214,217,
		1,0,0,0,215,213,1,0,0,0,215,216,1,0,0,0,216,218,1,0,0,0,217,215,1,0,0,
		0,218,219,6,22,0,0,219,46,1,0,0,0,220,221,5,105,0,0,221,222,5,102,0,0,
		222,48,1,0,0,0,223,224,5,101,0,0,224,225,5,108,0,0,225,226,5,115,0,0,226,
		227,5,101,0,0,227,50,1,0,0,0,228,229,5,101,0,0,229,230,5,108,0,0,230,231,
		5,115,0,0,231,232,5,101,0,0,232,233,5,95,0,0,233,234,5,105,0,0,234,235,
		5,102,0,0,235,52,1,0,0,0,236,237,5,108,0,0,237,238,5,105,0,0,238,239,5,
		109,0,0,239,240,5,105,0,0,240,241,5,116,0,0,241,54,1,0,0,0,242,243,5,119,
		0,0,243,244,5,104,0,0,244,245,5,105,0,0,245,246,5,108,0,0,246,247,5,101,
		0,0,247,56,1,0,0,0,248,250,7,8,0,0,249,248,1,0,0,0,250,251,1,0,0,0,251,
		249,1,0,0,0,251,252,1,0,0,0,252,253,1,0,0,0,253,255,5,45,0,0,254,256,7,
		8,0,0,255,254,1,0,0,0,256,257,1,0,0,0,257,255,1,0,0,0,257,258,1,0,0,0,
		258,265,1,0,0,0,259,261,7,8,0,0,260,259,1,0,0,0,261,262,1,0,0,0,262,260,
		1,0,0,0,262,263,1,0,0,0,263,265,1,0,0,0,264,249,1,0,0,0,264,260,1,0,0,
		0,265,58,1,0,0,0,266,268,7,9,0,0,267,266,1,0,0,0,268,269,1,0,0,0,269,267,
		1,0,0,0,269,270,1,0,0,0,270,60,1,0,0,0,23,0,80,86,94,114,121,126,138,140,
		142,148,153,159,164,170,176,207,215,251,257,262,264,269,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
