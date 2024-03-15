using ChessChallenge.API;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

//TO DO NEXT!!!!!!!!!!!
//Implement tapered evaluation using the endgame pesto table
//Look into transposition table
//Quiescence Search
//Timer stuff
//Avoid draw by repetition
public class MyBot : IChessBot {
    // Pawn, Knight, Bishop, Rook, Queen, King
    readonly int[] PIECE_VALUES = { 1000, 3000, 3000, 5000, 9000, 0 };

    // Currently only has midgame scores
    readonly ulong[,] PESTO_TABLES = new ulong[,] {
        { 0x0, 0x31431e2f223f11fb, 0xfd030d0f201c0cf6, 0xf906030a0b0608f5, 0xf3fffe06080305f4, 0xf3fefefb010110fa, 0xef00f6f5f90c13f5, 0x0, },
        { 0xadd4efe81ed0f9cb, 0xdcec24120b1f03f8, 0xe91e12202a402416, 0xfc08091a1222090b, 0xfa0208060e090afc, 0xf5fc060509080cf8, 0xf2e6faff0009f9f7, 0xccf6e3f0f8f2f7f5, },
        { 0xf202d7eef4eb03fc, 0xf308f7fa0f1d09e9, 0xf8121514111912ff, 0xfe020919121203ff, 0xfd06060d11060502, 0x70707070d0905, 0x2070800030a1000, 0xf0fff9f6fafaedf6, },
        { 0x101510191f040f15, 0xd101d1f28210d16, 0xfe090d1208161e08, 0xf4fb030d0c11fcf6, 0xeef3fa0004fd03f5, 0xeaf4f8f80100fef0, 0xeaf8f6fc0005fddd, 0xf7fa00080803eef3, },
        { 0xf2000e061d161516, 0xf4edfe00f81c0e1b, 0xfaf803040e1c171c, 0xf3f3f8f80008ff00, 0xfcf3fcfbfffe01ff, 0xf901fbfffe010702, 0xeffc05010407ff00, 0xf7fc05f9f4f1e7, },
        { 0xe00b08f9e4ef0106, 0xe00f6fdfcfeedf2, 0xfc0c01f8f6030bf5, 0xf8f6faf3f1f4f9ee, 0xe800f3ede9eaf0e7, 0xf9f9f5e9eaf1f9f3, 0x3fce0ebf80404, 0xf91206e504f20c07, },
    };

	// PROBABLY REMOVE THIS METHOD LATER (for token space)
	private int getPositionScore(int type, Square square) {
		return 4 * (sbyte)(11111111 & PESTO_TABLES[type, 7 - square.Rank] >> (56 - square.File * 8));
    }

	Board board;
    Timer timer;

    public Move Think(Board boardOriginal, Timer timerOriginal) {
        board = boardOriginal;
        timer = timerOriginal;

		Move bestMove = default;
        int searchDepth = 5;

		int Search(int depth, int alpha, int beta, bool first) {
			if (depth == 0) {
				return Evaluate();
			}

            var moves = board.GetLegalMoves().OrderByDescending(move => move.IsCapture ? 10 * (int)move.MovePieceType - (int)move.CapturePieceType
                                                                      : move.IsPromotion ? 5
                                                                      : 0);

			if (!moves.Any()) {
                if (board.IsInCheck())
                    return 2147483647;
                return 0;
            }

            foreach (Move move in moves) {
                board.MakeMove(move);
				int score = -Search(depth - 1, -beta, -alpha, false);
				board.UndoMove(move);
				if (first) Console.WriteLine(beta + " | " + score);
				if (score >= beta)
					return beta;
                if (score > alpha) {
                    alpha = score;
                    if (first) bestMove = move;
                }
			}

			return alpha;
		}
        Search(searchDepth, -2147483647, 2147483647, true);

        return bestMove;
    }

    private int Evaluate() {
        int score = 0;
        
        foreach (bool isWhite in new[] { !board.IsWhiteToMove, board.IsWhiteToMove }) {
            score = -score;

			for (int i = 0; ++i < 7;) {
				PieceList pieces = board.GetPieceList((PieceType)i, isWhite);
				foreach (Piece piece in pieces) {
				 	if (!piece.IsNull)
						score += PIECE_VALUES[i - 1] + getPositionScore(i - 1, new Square(isWhite ? piece.Square.Index : piece.Square.Index ^ 56));
				}
			}

			if (board.IsInCheck())
				score += 700;

			if (board.IsInCheckmate())
				score += 2000000000;
		}

        return score;
    }
}