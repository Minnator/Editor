//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from d:/EU4 Grammar/PDXLanguageParser.g4 by ANTLR 4.13.1

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
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class PDXLanguageParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		EQ=1, LPAR=2, RPAR=3, AND=4, OR=5, YES=6, NO=7, NOT=8, MPOWER=9, TAG=10, 
		INT=11, STRING=12, FLOAT=13, DATE=14, MONTHS=15, DAYS=16, COLOR=17, PLUS=18, 
		MINUS=19, COMMA=20, DOT=21, WHITESPACE=22, SINGLE_LINE_COMMENT=23, IF=24, 
		ELSE=25, ELSE_IF=26, LIMIT=27, WHILE=28, IDENTIFIER=29, STRING_TOOLTIP=30;
	public const int
		RULE_areaFile = 0, RULE_intList = 1, RULE_color = 2, RULE_area = 3, RULE_rpar = 4;
	public static readonly string[] ruleNames = {
		"areaFile", "intList", "color", "area", "rpar"
	};

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

	public override string GrammarFileName { get { return "PDXLanguageParser.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static PDXLanguageParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public PDXLanguageParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public PDXLanguageParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class AreaFileContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public AreaContext[] area() {
			return GetRuleContexts<AreaContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public AreaContext area(int i) {
			return GetRuleContext<AreaContext>(i);
		}
		public AreaFileContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_areaFile; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IPDXLanguageParserVisitor<TResult> typedVisitor = visitor as IPDXLanguageParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAreaFile(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public AreaFileContext areaFile() {
		AreaFileContext _localctx = new AreaFileContext(Context, State);
		EnterRule(_localctx, 0, RULE_areaFile);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 13;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==IDENTIFIER) {
				{
				{
				State = 10;
				area();
				}
				}
				State = 15;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class IntListContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] INT() { return GetTokens(PDXLanguageParser.INT); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INT(int i) {
			return GetToken(PDXLanguageParser.INT, i);
		}
		public IntListContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_intList; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IPDXLanguageParserVisitor<TResult> typedVisitor = visitor as IPDXLanguageParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitIntList(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public IntListContext intList() {
		IntListContext _localctx = new IntListContext(Context, State);
		EnterRule(_localctx, 2, RULE_intList);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 17;
			ErrorHandler.Sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					State = 16;
					Match(INT);
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				State = 19;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,1,Context);
			} while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ColorContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COLOR() { return GetToken(PDXLanguageParser.COLOR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EQ() { return GetToken(PDXLanguageParser.EQ, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LPAR() { return GetToken(PDXLanguageParser.LPAR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode[] INT() { return GetTokens(PDXLanguageParser.INT); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INT(int i) {
			return GetToken(PDXLanguageParser.INT, i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode RPAR() { return GetToken(PDXLanguageParser.RPAR, 0); }
		public ColorContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_color; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IPDXLanguageParserVisitor<TResult> typedVisitor = visitor as IPDXLanguageParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitColor(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ColorContext color() {
		ColorContext _localctx = new ColorContext(Context, State);
		EnterRule(_localctx, 4, RULE_color);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 21;
			Match(COLOR);
			State = 22;
			Match(EQ);
			State = 23;
			Match(LPAR);
			State = 24;
			Match(INT);
			State = 25;
			Match(INT);
			State = 26;
			Match(INT);
			State = 27;
			Match(RPAR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class AreaContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode IDENTIFIER() { return GetToken(PDXLanguageParser.IDENTIFIER, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EQ() { return GetToken(PDXLanguageParser.EQ, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LPAR() { return GetToken(PDXLanguageParser.LPAR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public RparContext rpar() {
			return GetRuleContext<RparContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public IntListContext[] intList() {
			return GetRuleContexts<IntListContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public IntListContext intList(int i) {
			return GetRuleContext<IntListContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ColorContext color() {
			return GetRuleContext<ColorContext>(0);
		}
		public AreaContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_area; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IPDXLanguageParserVisitor<TResult> typedVisitor = visitor as IPDXLanguageParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitArea(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public AreaContext area() {
		AreaContext _localctx = new AreaContext(Context, State);
		EnterRule(_localctx, 6, RULE_area);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 29;
			Match(IDENTIFIER);
			State = 30;
			Match(EQ);
			State = 31;
			Match(LPAR);
			State = 33;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,2,Context) ) {
			case 1:
				{
				State = 32;
				intList();
				}
				break;
			}
			State = 36;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==COLOR) {
				{
				State = 35;
				color();
				}
			}

			State = 39;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==INT) {
				{
				State = 38;
				intList();
				}
			}

			State = 41;
			rpar();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class RparContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode RPAR() { return GetToken(PDXLanguageParser.RPAR, 0); }
		public RparContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_rpar; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IPDXLanguageParserVisitor<TResult> typedVisitor = visitor as IPDXLanguageParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitRpar(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public RparContext rpar() {
		RparContext _localctx = new RparContext(Context, State);
		EnterRule(_localctx, 8, RULE_rpar);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 45;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case RPAR:
				{
				State = 43;
				Match(RPAR);
				}
				break;
			case Eof:
			case IDENTIFIER:
				{
				 NotifyErrorListeners("Missing closing '}'"); 
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,30,48,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,1,0,5,0,12,8,0,10,0,
		12,0,15,9,0,1,1,4,1,18,8,1,11,1,12,1,19,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,
		2,1,3,1,3,1,3,1,3,3,3,34,8,3,1,3,3,3,37,8,3,1,3,3,3,40,8,3,1,3,1,3,1,4,
		1,4,3,4,46,8,4,1,4,0,0,5,0,2,4,6,8,0,0,48,0,13,1,0,0,0,2,17,1,0,0,0,4,
		21,1,0,0,0,6,29,1,0,0,0,8,45,1,0,0,0,10,12,3,6,3,0,11,10,1,0,0,0,12,15,
		1,0,0,0,13,11,1,0,0,0,13,14,1,0,0,0,14,1,1,0,0,0,15,13,1,0,0,0,16,18,5,
		11,0,0,17,16,1,0,0,0,18,19,1,0,0,0,19,17,1,0,0,0,19,20,1,0,0,0,20,3,1,
		0,0,0,21,22,5,17,0,0,22,23,5,1,0,0,23,24,5,2,0,0,24,25,5,11,0,0,25,26,
		5,11,0,0,26,27,5,11,0,0,27,28,5,3,0,0,28,5,1,0,0,0,29,30,5,29,0,0,30,31,
		5,1,0,0,31,33,5,2,0,0,32,34,3,2,1,0,33,32,1,0,0,0,33,34,1,0,0,0,34,36,
		1,0,0,0,35,37,3,4,2,0,36,35,1,0,0,0,36,37,1,0,0,0,37,39,1,0,0,0,38,40,
		3,2,1,0,39,38,1,0,0,0,39,40,1,0,0,0,40,41,1,0,0,0,41,42,3,8,4,0,42,7,1,
		0,0,0,43,46,5,3,0,0,44,46,6,4,-1,0,45,43,1,0,0,0,45,44,1,0,0,0,46,9,1,
		0,0,0,6,13,19,33,36,39,45
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
