using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships_Game_Samanta_0510
{ 
	class Ship
    {
	private string validLetters;
	List<XY> shipCoordinates;

	public Ship(string[] input, string validLetters) //klasė naudojama tada kai skaitomas txt failas, kuriame yra nurodyti laivai
	{
		this.validLetters = validLetters; //konstruktorius
		validateInput(input); //kad būtų įmanoma sudaryti tinkamą koordinatę
		XY coord1 = new XY(input[0].ToLower()[0], int.Parse(input[1])); //sukuria naują koordinatę iš to ką padavėme txt faile (minimumas arba koordinatės pradžia)
		XY coord2 = new XY(input[2].ToLower()[0], int.Parse(input[3])); //(maksimumas arba koordinatės pabaiga)
		validateCoordinates(coord1, coord2);
		shipCoordinates = findCoordinates(coord1.CompareTo(coord2, validLetters) > 0 ? coord2 : coord1, coord1.CompareTo(coord2, validLetters) < 0 ? coord2 : coord1);
	}   //coord1.CompareTo lygina dvi koordinates (antrojo cord2, pirmojo cord1), jeigu pirmoji koordinatė yra didesnė už antrąją tuomet parametras pirmasis bus cord2, kitu atveju cord1 ir atvirkščiai
		//skirta tam, kad atrastų minimumą iš dviejų paduotų koordinačių (jei tarkim paduoda a 5 ir a 1, kad atrastų, kuri koordinatė yra mažesnė)

	public int Length() //gražina kiek koordinačių laivas turi arba kitaip, tai kokio ilgio laivas
	{
		return shipCoordinates.Count;
	}

	public void removeCoordinate(XY coordinate) //iš laivo koordinačių ištrina koordinatę, kuri buvo paduota per parametrus
			//eina per visas laivo koordinates ir jei koordinatė yra lygi paduotai koordinatei ją ištrina iš laivo sąrašo 
	{
		for (int i = 0; i < shipCoordinates.Count; i++)
		{
			if (shipCoordinates[i].CompareTo(coordinate, validLetters) == 0) //ieškos ar lygios dvi koordinatės
			{
				shipCoordinates.RemoveAt(i); //ištrins jei lygios koordinatės
				return;
			}
		}
	}

	public List<XY> getShipCoordinates() //grąžina laivo koordinačių masyvą
	{
		return shipCoordinates;
	}

	private List<XY> findCoordinates(XY start, XY end) //suranda visas laivo koordinates tarp min ir max (paduotų reikšmių)
	{
		List<XY> coordinates = new List<XY>();

		for (int i = validLetters.IndexOf(start.getX()); i < validLetters.IndexOf(end.getX()) + 1; i++) //atranda visas koordinates tarp raidžių (tarkim laivas nuo s 1 iki u 1, tai surast tarpą p1)
		{
			int j;
			for (j = start.getY(); j < end.getY(); j++)
			{
				coordinates.Add(new XY(validLetters[i], j));
			}
			coordinates.Add(new XY(validLetters[i], j));
		}
		return coordinates;
	}

	private void validateCoordinates(XY coord1, XY coord2) //patikrina ar laivas nėra padėtas išstrižai, nes laivas turi būti tiesus
	{
		if (coord1.getX() != coord2.getX() && coord1.getY() != coord2.getY()) //tikrina ar dviejų koordinačių x yra ne vienodi, ir ar y ne vienodi, jei bent kažkuris vienas yra vienodas, reiškiasi, kad laivas eina tiesiai (horizontaliai arba vertikaliai, bet tiesiai)
		{
			throw new FormatException();
		}
	}

	private void validateInput(string[] input) //tikrina ar validus tekstas, kurį padavė txt faile 
	{
		if (input.Length != 4) //patikrina stringo ilgį txt faile
		{
			throw new FormatException();
		}

		for (int i = 0; i < input.Length; i += 2) //jeigu paduotų dvi raides ar skaičius mestų klaida (pvz. ss 1 s 1) - tikrina simbolių kiekį
			{
			if (input[i].Length > 1)
			{
				throw new FormatException();
			}
		}
	}
}
}
