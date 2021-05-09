using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships_Game_Samanta_0510
{
	public class XY
	{
		private char x; //x ašis - raidės (žodis respubika)
		private int y; // y ašis - skaičiai (nuo 1 iki 10)

		public XY(char x, int y) //konstruktorius
		{
			this.x = x; //this, nes x ir x t.y. toki patys užvadinimai
			this.y = y; //this, nes y ir y t.y. toki patys užvadinimai
		}

		public char getX() //grąžina x reikšmę
		{
			return x;
		}

		public int getY() //grąžina y reikšmę
		{
			return y;
		}

		public int CompareTo(XY other, string validLetters) //lygina dvi koordinates
		{
			int xIndex1 = validLetters.IndexOf(x); //jei paduotum r, butu 0, jei e, tai 1 ir t.t.
			int xIndex2 = validLetters.IndexOf(other.getX()); //paima antrosios koordinates

			if (xIndex1 == xIndex2) //lygina indeksus, kuriuos atrado
			{
				if (y == other.getY()) //tikrina ar y yra vienodi, jeigu x ir y yra vienodi reiškiasi, kad koordinatės yra lygios
				{
					return 0; //jei lygios grąžina 0
				}
				if (y > other.getY())
				{
					return 1; //jeigu pirmosios koordinatės y yra didesnis uz antrodios, tuomet grąžina 1, kitu atveju -1
				}
				else return -1;
			}
			else
			{
				if (xIndex1 > xIndex2) 
				{
					return 1;
				}
				else return -1;
			}
		}
	}
}
