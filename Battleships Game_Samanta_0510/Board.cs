using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Battleships_Game_Samanta_0510
{
	class Board
	{

		private List<Ship> ships;
		private string validLetters;
		private List<XY> hit;
		private List<XY> missed;

		public Board(string fileName, int minShipSize, int maxShipSize, string validLetters) //pirmiausia patikrina ar visos žodžio raidės yra skirtingos (šiuo atveju respublika), jei būtų kelios tos pačios raidės, tieisog nesukurtų laivų masyvo
		{
			try
			{
				validateValidLetters(validLetters); //raides tikrina ar nesikartoja
				this.validLetters = validLetters; //prirašytas this, nes kartojasi validLetters žodis
				ships = readShipFile(fileName, minShipSize, maxShipSize); //sukuria visą laivų masyvą iš txt failo
				hit = new List<XY>(); //sukuria tusčius masyvus, kur bus laikomi pataikyti šūviai
				missed = new List<XY>(); //sukuria tusčius masyvus, kur bus laikomi nepataikyti šūviai
			}
			catch (FormatException)
			{
				Console.WriteLine("Netinkamas lentos formatas"); //jei try kūne bus rasta klaida, tuomet ją išves kaip exception'ą - "Netinkamas lentos formatas"
			}
		}

		public void startGame() //pradeda žaidimą, jei tik txt failas yra sėkmingai sukurtas
		{
			if (ships == null)
			{
				return;
			}

			while (ships.Count > 0) //loopina kol yra laivų, kai baigiasi laivai baigiasi ir ciklas
			{
				this.drawBoard();
				Console.WriteLine("Iveskite raide ir skaiciu");
				try
				{
					string line = Console.ReadLine();
					string[] input = line.Split(); //splitina raidę ir skaičių (pvz. r 1)
					if (checkIfHit(input)) //tikrina ar laivų masyve yra laivas toje koordinatėje, į kurią šovė žaidėjas
					{
						Console.WriteLine("Pataikete!");
					}
					else Console.WriteLine("Nepataikete");

					removeDestroyedShips(); //ištrina tuos laivus, kurie nebeturi koordinačių
				}
				catch (Exception)
				{
					Console.WriteLine("Neteisingai ivestos koordinates"); //jei try kūne bus rasta klaida, tuomet ją išves kaip exception'ą - "Neteisingai ivestos koordinates"
				}


			};

			Console.WriteLine("Laimejote! Visi laivai sunaikinti"); //ciklo  pabaiga
		}

		private void removeDestroyedShips() //kai laivo visos koordinatės sunaikintos
		{
			for (int i = 0; i < ships.Count; i++)
			{
				if (ships[i].getShipCoordinates().Count == 0)
				{
					ships.RemoveAt(i);
					Console.WriteLine("Sunaikinote laiva");
					break;
				}
			}
		}

		private bool checkIfHit(string[] input)
		{
			XY coord = new XY(input[0].ToLower()[0], int.Parse(input[1])); //sukuria laikiną koordinatė. Laikiną, nes egzistuos tik šiame metode
			if (!checkIfValidCoordinate(coord)) //tirkina ar tokia koordinatė gali iš viso egzistuoti
			{
				throw new FormatException(); //jeigu neegzistuoja išmeta exception'ą
			}

			bool found = false;
			foreach (Ship ship in ships) //eina per visus laivus
			{
				foreach (XY shipCoord in ship.getShipCoordinates()) //eina per vieno laivo koordinates - pakol galiausiai pereina per visas
				{
					if (shipCoord.CompareTo(coord, validLetters) == 0) //tirkina ar laivo koordinatė yra lygi pasirinktai žaidėjo laivo koordinatei
					{
						found = true; //jei lygi true
						ship.removeCoordinate(coord); //ištrina tą koordinatę iš laivo masyvo
						break;
					}
				}
				if (found)
				{
					break;
				}
			}

			if (found)
			{
				hit.Add(coord); //jeigu rado laivo koordinatę
				return true;
			}
			else
			{
				missed.Add(coord); //jeigu nerado laivo koordinatės
				return false;
			}
		}

		private bool checkIfValidCoordinate(XY coord) //patikrina ar tokia koordinatė egzistuoja plokštumoje
		{
			if (validLetters.IndexOf(coord.getX()) >= 0 && (coord.getY() < validLetters.Length + 1 && coord.getY() > 0)) //kairė pusė tikrina ar egzistuoja raidė (respublika stringe), o dešnė tikrina skaičius, ar tokie egzistuoja (raides išreikštos per skaičius)
			{
				return true; //galima koordinatė
			}
			return false; //negalima koordinatė
		}

		private void drawBoard() //žymėjimai
		{
			Console.Write("   "); //neleidžia padėti enter
			for (int i = 1; i < validLetters.Length + 1; i++)
			{
				Console.Write("{0} ", validLetters[i - 1]); //surašo visas raides - šiuo atveju žodį respublika
			}
			Console.WriteLine(""); //leidžia padėti enter
			for (int i = 0; i < validLetters.Length; i++)
			{
				Console.Write("{0,2} ", i + 1); //kiekvienam skaičiui nepriklausomai nuo jo raidžių kiekio palieka du tarpus (pvz. 5 (tarpas) 10 (tarpas))
				for (int j = 0; j < validLetters.Length; j++) //nupiešia žymėjimo simbolius
				{
					XY temp = new XY(validLetters[j], i + 1); //sukuria koordinatę (pvz. r 1, e 1, s 1 ir t.t. einant į dešnę pusę kas eilutę)
					if (listContainsCoordinate(hit, temp)) //patikrina ar sukurta koordinatė auksčiau yra hit masyve
					{
						Console.Write("X "); //jeigu pataikė - laivas arba pašautas arba nušautas
					}
					else if (listContainsCoordinate(missed, temp)) //patikrina ar sukurta koordinatė aukščiau yra miss masyve
					{
						Console.Write("* "); //jeigu nepataikė arba kitaip jei pataikė į tusčią koordinatę be laivo
					}
					else Console.Write("# "); //lentos langelių žymėjimas
				}
				Console.WriteLine("");
			}
		}

		private bool listContainsCoordinate(List<XY> list, XY coordinate) //patikrina ar koordinatės reikšmė yra masyve (naudojama tikrinti ar egzistuoja hit and miss masyve)
		{
			if (list == null) 
			{
				return false;
			}
			foreach (XY xy in list) //eina per kiekvieną koordinatę
			{
				if (xy.CompareTo(coordinate, validLetters) == 0) //tikrina ar koordinatė egzistuoja masyve
				{
					return true;
				}
			}
			return false;
		}

		private void validateValidLetters(string validLetters) //tikrina ar nėra vienodų raidžių ir ar nėra ne raidžių t.y. kažkokių simboliu, skaičių ir pan.
		{
			for (int i = 0; i < validLetters.Length; i++)
			{
				for (int j = i + 1; j < validLetters.Length; j++) //tikrina ar nėra vienodų raidžių
				{
					if (validLetters[i] == validLetters[j])
					{
						throw new FormatException();
					}
				}
				if (!char.IsLetter(validLetters[i])) //tikrina ar nėra kažkokių simbolių
				{
					throw new FormatException();
				}
			}
		}

		private bool checkIfCoordinatesNearby(XY coord1, XY coord2) //tikrina ar dvi koordinatės yra šalia viena kitos - išlaikant vieno langelio tarpą tarp koordinačių
		{
			if (Math.Abs(validLetters.IndexOf(coord1.getX()) - validLetters.IndexOf(coord2.getX())) <= 1 && Math.Abs(coord1.getY() - coord2.getY()) <= 1) //dešnė pusė tikrina x ašį, kairė pusė po && tirkina y ašį
			{ //Math.Abs nuima minuso ženklą ir tada palygina ar ta atimtis yra mažiau ar daugiau vienetui
				return true;
			}
			return false;
		}

		private bool checkIfValidShipPlacement(List<Ship> ships, Ship newShip) //eina per visus laivus ir tirkina ar bent vienas laivas atitinka sąlygą, kad koordinatė yra salia laivo, ar du laivai yra vienas šalia kito, ar vienas ant kito neužlipa t.y. ar laivai persipina tarpusavyje ir pan.
		{
			foreach (Ship ship in ships) //eina per visus laivus, kurie jau yra sukurti ir jų koordinates lygina su naujai sukurtų laivų koordinatėmis
			{
				foreach (XY newShipCoord in newShip.getShipCoordinates())
				{
					foreach (XY coord in ship.getShipCoordinates())
					{
						if (checkIfCoordinatesNearby(coord, newShipCoord))
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		private List<Ship> readShipFile(string fileName, int minShipSize, int maxShipSize) //pirmiausia patikrina ar txt failas egzistuoja
		{
			if (!File.Exists(fileName)) //jeigu failas egiztuoja, nuskaito iš jo visas eilutes
			{
				Console.WriteLine("Failas {0} neegzistuoja", fileName);
				return null;
			}

			string[] lines = File.ReadAllLines(fileName); //nuskaito visas eilutes iš txt failo

			List<Ship> ships = new List<Ship>(); //sukuria tuščią laivų masyvą į kurį vėliau dės laivus
			for (int i = 0; i < lines.Length; i++) //eina per kiekvieną eilutę iš txt failo, kurią nuskaitė
			{
				try //ir tada bandys sukurti iš tos eilutės naują laivą
				{
					string[] coordinates = lines[i].Split();
					Ship ship = new Ship(coordinates, validLetters); //iš koordinačių bandys sukurti laivą
					if (ship.Length() < minShipSize || ship.Length() > maxShipSize || !checkIfValidShipPlacement(ships, ship)) //lygins ar sukurto laivo ilgis yra mažesnis nei minimumas arba didensis nei maksimumas ir patikrins ar įmanoma tokį laivą iš viso padėti
					{
						throw new FormatException();
					}
					ships.Add(ship); //pridės prie laivų sąrašo naujai sukurtą laivą
				}
				catch (FormatException)
				{
					Console.WriteLine("{0}-oje eiluteje netinkamas duomenu formatas arba laivai yra per arti vienas kito", i + 1); //jei try kūne bus rasta bent viena klaida, tuomet ją išves kaip exception'ą
				}
			}

			return ships; //grazina laivų masyvą iš txt failo
		}
	}
}
