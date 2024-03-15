using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TablePacker {
    int[] mg_pawn_table = {
        0,   0,   0,   0,   0,   0,  0,   0,
        98, 134,  61,  95,  68, 126, 34, -11,
        -6,   7,  26,  31,  65,  56, 25, -20,
        -14,  13,   6,  21,  23,  12, 17, -23,
        -27,  -2,  -5,  12,  17,   6, 10, -25,
        -26,  -4,  -4, -10,   3,   3, 33, -12,
        -35,  -1, -20, -23, -15,  24, 38, -22,
        0,   0,   0,   0,   0,   0,  0,   0,
    };

    int[] mg_knight_table = {
        -167, -89, -34, -49,  61, -97, -15, -107,
        -73, -41,  72,  36,  23,  62,   7,  -17,
        -47,  60,  37,  65,  84, 129,  73,   44,
        -9,  17,  19,  53,  37,  69,  18,   22,
        -13,   4,  16,  13,  28,  19,  21,   -8,
        -23,  -9,  12,  10,  19,  17,  25,  -16,
        -29, -53, -12,  -3,  -1,  18, -14,  -19,
        -105, -21, -58, -33, -17, -28, -19,  -23,
    };

    int[] mg_bishop_table = {
        -29,   4, -82, -37, -25, -42,   7,  -8,
        -26,  16, -18, -13,  30,  59,  18, -47,
        -16,  37,  43,  40,  35,  50,  37,  -2,
        -4,   5,  19,  50,  37,  37,   7,  -2,
        -6,  13,  13,  26,  34,  12,  10,   4,
        0,  15,  15,  15,  14,  27,  18,  10,
        4,  15,  16,   0,   7,  21,  33,   1,
        -33,  -3, -14, -21, -13, -12, -39, -21,
    };

    int[] mg_rook_table = {
        32,  42,  32,  51, 63,  9,  31,  43,
        27,  32,  58,  62, 80, 67,  26,  44,
        -5,  19,  26,  36, 17, 45,  61,  16,
        -24, -11,   7,  26, 24, 35,  -8, -20,
        -36, -26, -12,  -1,  9, -7,   6, -23,
        -45, -25, -16, -17,  3,  0,  -5, -33,
        -44, -16, -20,  -9, -1, 11,  -6, -71,
        -19, -13,   1,  17, 16,  7, -37, -26,
    };

    int[] mg_queen_table = {
        -28,   0,  29,  12,  59,  44,  43,  45,
        -24, -39,  -5,   1, -16,  57,  28,  54,
        -13, -17,   7,   8,  29,  56,  47,  57,
        -27, -27, -16, -16,  -1,  17,  -2,   1,
        -9, -26,  -9, -10,  -2,  -4,   3,  -3,
        -14,   2, -11,  -2,  -5,   2,  14,   5,
        -35,  -8,  11,   2,   8,  15,  -3,   1,
        -1, -18,  -9,  10, -15, -25, -31, -50,
    };

    int[] mg_king_table = {
        -65,  23,  16, -15, -56, -34,   2,  13,
        29,  -1, -20,  -7,  -8,  -4, -38, -29,
        -9,  24,   2, -16, -20,   6,  22, -22,
        -17, -20, -12, -27, -30, -25, -14, -36,
        -49,  -1, -27, -39, -46, -44, -33, -51,
        -14, -14, -22, -46, -44, -30, -15, -27,
        1,   7,  -8, -64, -43, -16,   9,   8,
        -15,  36,  12, -54,   8, -28,  24,  14,
    };

    const int TABLE_ROWS = 8;
    const int TABLE_COLUMNS = 8;

    /* PACKED
	ulong[,] PESTO_TABLES = new ulong[,] {
		{ 0x0, 0x31431e2f223f11fb, 0xfd030d0f201c0cf6, 0xf906030a0b0608f5, 0xf3fffe06080305f4, 0xf3fefefb010110fa, 0xef00f6f5f90c13f5, 0x0, },
		{ 0xadd4efe81ed0f9cb, 0xdcec24120b1f03f8, 0xe91e12202a402416, 0xfc08091a1222090b, 0xfa0208060e090afc, 0xf5fc060509080cf8, 0xf2e6faff0009f9f7, 0xccf6e3f0f8f2f7f5, },
		{ 0xf202d7eef4eb03fc, 0xf308f7fa0f1d09e9, 0xf8121514111912ff, 0xfe020919121203ff, 0xfd06060d11060502, 0x70707070d0905, 0x2070800030a1000, 0xf0fff9f6fafaedf6, },
		{ 0x101510191f040f15, 0xd101d1f28210d16, 0xfe090d1208161e08, 0xf4fb030d0c11fcf6, 0xeef3fa0004fd03f5, 0xeaf4f8f80100fef0, 0xeaf8f6fc0005fddd, 0xf7fa00080803eef3, },
		{ 0xf2000e061d161516, 0xf4edfe00f81c0e1b, 0xfaf803040e1c171c, 0xf3f3f8f80008ff00, 0xfcf3fcfbfffe01ff, 0xf901fbfffe010702, 0xeffc05010407ff00, 0xf7fc05f9f4f1e7, },
		{ 0xe00b08f9e4ef0106, 0xe00f6fdfcfeedf2, 0xfc0c01f8f6030bf5, 0xf8f6faf3f1f4f9ee, 0xe800f3ede9eaf0e7, 0xf9f9f5e9eaf1f9f3, 0x3fce0ebf80404, 0xf91206e504f20c07, },
	};
	*/

    public void packAndOutputAll() {
        halveTable(mg_pawn_table);
        halveTable(mg_knight_table);
        halveTable(mg_bishop_table);
        halveTable(mg_rook_table);
        halveTable(mg_queen_table);
        halveTable(mg_king_table);

        UInt64[] mg_pawn_table_packed = packTable(mg_pawn_table);
        UInt64[] mg_knight_table_packed = packTable(mg_knight_table);
        UInt64[] mg_bishop_table_packed = packTable(mg_bishop_table);
        UInt64[] mg_rook_table_packed = packTable(mg_rook_table);
        UInt64[] mg_queen_table_packed = packTable(mg_queen_table);
        UInt64[] mg_king_table_packed = packTable(mg_king_table);

        string output = "ulong[,] PESTO_TABLES = new ulong[,] {\n";
        output += formatTable(mg_pawn_table_packed);
        output += formatTable(mg_knight_table_packed);
        output += formatTable(mg_bishop_table_packed);
        output += formatTable(mg_rook_table_packed);
        output += formatTable(mg_queen_table_packed);
        output += formatTable(mg_king_table_packed);
        output += "};";
        Console.Write(output + '\n');
    }

    private void halveTable(int[] unpackedTable) {
        for (int i = 0; i < unpackedTable.Length; i++)
            unpackedTable[i] = unpackedTable[i] / 2;
    }

    private string formatTable(ulong[] packedTable) {
        string formattedTable = "{ ";
        for (int i = 0; i < packedTable.Length; i++)
            formattedTable += "0x" + Convert.ToString((long)packedTable[i], 16) + ", ";
        formattedTable += "},\n";
        return formattedTable;
    }

    private UInt64[] packTable(int[] unpackedTable) {
        UInt64[] packedTable = new UInt64[8];

        for (int i = 0; i < 8; i++) {
            UInt64 packedRow = 0;

            for (int j = 0; j < 8; j++) {
                byte packedSquare = (byte)unpackedTable[i * 8 + j];
                packedRow |= (UInt64)packedSquare << (56 - (j * 8));
            }

            packedTable[i] = packedRow;
        }

        return packedTable;
    }

    private int[] unpackTable(UInt64[] packedTable) {
        int[] unpackedTable = new int[64];

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                int unpackedSquare = (sbyte)(byte.MaxValue & packedTable[i] >> (56 - (j * 8)));
                unpackedTable[i * 8 + j] = unpackedSquare;
            }
        }

        return unpackedTable;
    }

    // To assist with debugging.
    private string ConvertByteToBinary(byte num) {
        return Convert.ToString(num, 2).PadLeft(8, '0');
    }

    private string ConvertIntToBinary(int num) {
        return Convert.ToString(num, 2).PadLeft(32, '0');
    }

    private string ConvertUInt64ToBinary(UInt64 num) {
        return Convert.ToString((long)num, 2).PadLeft(64, '0');
    }
}